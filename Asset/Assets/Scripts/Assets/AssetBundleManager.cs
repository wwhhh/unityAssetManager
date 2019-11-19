using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset
{
    public class AssetBundleManager : Singleton<AssetBundleManager>
    {
        private bool binit
        {
            get
            {
                return _assetBundleManifest != null ? true : false;
            }
        }
        private string _rootPath; // LoadFromFile时使用的资源路径
        private AssetBundleManifest _assetBundleManifest;

        private Dictionary<string, LoadedAssetBundleRef> _dicRefs = new Dictionary<string, LoadedAssetBundleRef>();

        public override void Init()
        {
            InitPath();
            AssetBundle ab = AssetBundle.LoadFromFile(GetABPath("AB"));
            if (ab != null)
            {
                _assetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                ab.Unload(false);
            }

            if (_assetBundleManifest == null)
                Debug.LogError("AssetBundleManager init failed becase of asset bundle manifest == null");
        }

        protected override void OnDestroy()
        {
            //UnloadAll();
            if (_assetBundleManifest != null)
            {
                Resources.UnloadAsset(_assetBundleManifest);        // 卸载Asset-Object
                _assetBundleManifest = null;
            }

            base.OnDestroy();
        }

        private AssetBundle LoadAssetBundleFromFile(string InAssetBundleName)
        {
            if (!binit)
            {
                Debug.LogError("AssetBundleManager::LoadAssetBundle -- Please init AssetBundleManager...");
                return null;
            }

            AssetBundle ab = AssetBundle.LoadFromFile(GetABPath(InAssetBundleName));
            return ab;
        }

        public void UnLoadAssetBundle(AssetBundle ab)
        {
            if (!binit)
            {
                Debug.LogError("AssetBundleManager::LoadAssetBundle -- Please init AssetBundleManager...");
                return;
            }

            ab.Unload(false);
        }

        public AssetBundle Load(string InAssetBundleName)
        {
            LoadedAssetBundleRef abRef = null;
            if (!_dicRefs.ContainsKey(InAssetBundleName))
            {
                AssetBundle ab = LoadAssetBundleFromFile(InAssetBundleName);
                if (ab == null) Debug.LogError("LoadAssetBundle failed to load asset bundle: " + GetABPath(InAssetBundleName));
                abRef = LoadedAssetBundleRef.Get(InAssetBundleName);
                abRef.assetBundle = ab;
                _dicRefs.Add(InAssetBundleName, abRef);
            }
            else
            {
                abRef = _dicRefs[InAssetBundleName];
                // 同时对一个AB包进行异步和同步加载可能出现此种情况，需要避免
                if (!abRef.assetBundle) Debug.LogError("AssetBundleManager::LoadAssetBundle -- {0} is loading：" + InAssetBundleName); 
                abRef.IncreaseRefs();
            }

            return abRef.assetBundle;
        }

        public void UnLoad(string InAssetBundleName)
        {
            if (_dicRefs.ContainsKey(InAssetBundleName))
            {
               LoadedAssetBundleRef abRef = _dicRefs[InAssetBundleName];
                int refCount = abRef.DecreaseRefs();
                if (refCount < 0 && !abRef.bPersistent)
                {
                    _dicRefs.Remove(InAssetBundleName);
                    LoadedAssetBundleRef.Release(abRef);
                }
            }
        }

        public string[] GetAllDependencies(string InAssetBundleName)
        {
            if (!binit)
            {
                Debug.LogError("AssetBundleManager::GetAllDependencies -- Please init AssetBundleManager...");
                return null;
            }
            return _assetBundleManifest.GetAllDependencies(InAssetBundleName);
        }

        private void InitPath()
        {
#if UNITY_EDITOR
            _rootPath = "Res/Win/";
#elif UNITY_STANDALONE
            _rootPath = Application.streamingAssetsPath + "/Res/";
            if (!System.IO.Directory.Exists(_rootPath))
            {
                _rootPath = "../Res/Win/";
            }
#endif
            Debug.Log(string.Format("AssetBundleManager:        _rootPath[{0}]", _rootPath));
        }

        private string GetABPath(string abName)
        {
#if UNITY_EDITOR
        AllLoaded.Add(abName);
#endif
            return _rootPath + abName;
        }

#if UNITY_EDITOR
    public static HashSet<string> AllLoaded = new HashSet<string>();
#endif
}
}