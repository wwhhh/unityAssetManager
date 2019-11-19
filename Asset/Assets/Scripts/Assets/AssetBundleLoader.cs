using System.Collections.Generic;
using UnityEngine;

namespace Asset
{
    public class AssetBundleLoader
    {
        public AssetBundle assetBundle { get; private set; }

        private LinkedList<AssetBundle> _dic_AssetBundles = new LinkedList<AssetBundle>();

        public void Load(string inABName)
        {
            string[] Dependencies = AssetBundleManager.I.GetAllDependencies(inABName);
            if (Dependencies != null && Dependencies.Length > 0)
            {
                for (int i = 0; i < Dependencies.Length; i++)
                {
                    AssetBundle ab = AssetBundleManager.I.Load(Dependencies[i]);
                    _dic_AssetBundles.AddLast(ab);
                }
            }
            assetBundle = AssetBundleManager.I.Load(inABName);
        }

        public void Unload()
        {
            if (assetBundle != null)
            {
                AssetBundleManager.I.UnLoad(assetBundle.name);
            }

            if (_dic_AssetBundles != null)
            {
                foreach (AssetBundle Ref in _dic_AssetBundles)
                {
                    AssetBundleManager.I.UnLoad(Ref.name);
                }
                _dic_AssetBundles.Clear();
            }
        }


        #region 池
        static private ObjectPool<AssetBundleLoader> _pool;
        static public AssetBundleLoader Get(string InAssetBundleName)
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<AssetBundleLoader>();
            }

            AssetBundleLoader abLoader = _pool.Get();
            if (abLoader != null)
                abLoader.Load(InAssetBundleName);
            return abLoader;
        }

        static public void Release(AssetBundleLoader data)
        {
            if (_pool == null)
                return;

            data.Unload();
            _pool.Release(data);
        }

        #endregion
    }
}