using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Asset
{
    // 所有资源的加载入口
    public class AssetManager
    {
        #region 同步
        static public AssetLoader<T> LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            AssetLoader<T> loader = new AssetLoader<T>(assetPath);
            return loader;
        }

        static public void UnloadAsset<T>(AssetLoader<T> loader) where T : UnityEngine.Object
        {
            if (loader != null)
                loader.Unload();
        }
        #endregion

    }
}