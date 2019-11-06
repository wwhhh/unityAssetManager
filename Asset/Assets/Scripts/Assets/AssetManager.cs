using UnityEditor;
using UnityEngine;

namespace Asset
{
    // 所有资源的加载入口
    public class AssetManager : Singleton<AssetManager>
    {
        enum LoaderType
        {
            FromResource,
            FromEditor,
            FromAB,
        }

        public const string RES_PATH = "assets/assetbundle/";

    }
}