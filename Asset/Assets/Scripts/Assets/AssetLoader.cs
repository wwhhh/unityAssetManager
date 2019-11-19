using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Asset
{

    public static class AssetLoaderManager
    {
        public enum LoaderType
        {
            FromResource,
            FromEditor,
            FromAB,
        }
        static public LoaderType loaderType = LoaderType.FromEditor;

        struct AssetName
        {
            public string abName;
            public string assetName;
        }
        static Dictionary<string, AssetName> s_dic = new Dictionary<string, AssetName>();

        // 从资源路径解析出其所在的 AssetBundle Name 和 Asset Name
        public static bool ParseAssetPath(string path, out string abName, out string assetName)
        {
            if (string.IsNullOrEmpty(path))
            {
                abName = null;
                assetName = null;
                return false;
            }

            if (s_dic.TryGetValue(path, out AssetName an))
            {
                abName = an.abName;
                assetName = an.assetName;
                return true;
            }

            abName = null;
            assetName = null;

            if (string.IsNullOrEmpty(path))
                return false;

            int index = path.LastIndexOf(@"/");
            if (index == -1)
                return false;

            assetName = path.Substring(index + 1);
            abName = path.Substring(0, index).Replace("/", "_");

            an = new AssetName();
            an.abName = abName;
            an.assetName = assetName;
            s_dic.Add(path, an);

            return true;
        }
    }

    public class AssetLoader<T> where T : Object
    {
        private AssetBundleLoader _loader;
        public T asset { get; private set; }
        public AssetLoader(string assetPath)
        {
            _loader = null;
            asset = default(T);
            LoadAsset(assetPath);
        }

        private void LoadAsset(string assetPath)
        {
#if UNITY_EDITOR
            if (assetPath != assetPath.ToLower())
                Debug.LogError("资源地址存在大写: " + assetPath);

            switch (AssetLoaderManager.loaderType)
            {
                case AssetLoaderManager.LoaderType.FromEditor:
                    asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    break;
                case AssetLoaderManager.LoaderType.FromResource:
                    asset = Resources.Load<T>(assetPath);
                    break;
                case AssetLoaderManager.LoaderType.FromAB:
                    LoadAssetFromAB(assetPath);
                    break;
            }
#else
            switch (AssetLoaderManager.loaderType)
            {
                case AssetLoaderManager.LoaderType.FromAB:
                case AssetLoaderManager.LoaderType.FromEditor:
                    LoadAssetFromAB(assetPath);
                    break;
                 case AssetLoaderManager.LoaderType.FromResource:
                    asset = Resources.Load<T>(assetPath);
                    break;
           }
#endif
        }

        private void LoadAssetFromAB(string assetPath)
        {
            if (!AssetLoaderManager.ParseAssetPath(assetPath, out string abName, out string assetName))
            {
                Debug.LogWarningFormat("AssetLoader -- Failed to reslove assetbundle name: {0} {1}", assetPath, typeof(T));
                return;
            }

            _loader = AssetBundleLoader.Get(abName);
            if (_loader.assetBundle != null)
            {
                asset = _loader.assetBundle.LoadAsset<T>(assetName);
                if (asset == null)
                {
                    Unload();
                }
            }
            else
            {
                Unload();
            }
        }

        public void Unload()
        {
            if (_loader != null)
            {
                AssetBundleLoader.Release(_loader);
                _loader = null;
            }
        }
    }
}