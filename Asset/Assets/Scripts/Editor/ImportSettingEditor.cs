using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class ImportSettingEditor : AssetPostprocessor
{

    #region ab包名自动处理
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

        foreach (string asset in importedAssets)
        {
            if (!asset.Contains("RawData"))
            {
                if (asset.Contains(" "))
                {
                    if (IsPassByWhiteList(asset) && !IsBlockedByBlackList(asset))
                    {
                        EditorUtility.DisplayDialog("错误", asset + " 命名包含空格。请导入后修正", "OK");
                        Debug.LogError(asset + " 命名包含空格");
                    }
                }
            }
            else
            {
                Debug.Log("RawData Pass : " + asset);
            }
        }

        for (int i = 0; i < importedAssets.Length; ++i)
        {
            UpdateAssetBundleName(importedAssets[i]);
        }

        for (int i = 0; i < movedAssets.Length; ++i)
        {
            UpdateAssetBundleName(movedAssets[i]);
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    // 路径白名单列表，仅对以下路径下的资源做处理
    static private string[] path_white_list = { "Assets/Game" };
    /// <summary>
    /// 根据路径判断是否需要处理
    /// </summary>
    static public bool IsPassByWhiteList(string assetPath)
    {
        for (int i = 0; i < path_white_list.Length; ++i)
        {
            if (assetPath.Contains(path_white_list[i]))
                return true;
        }

        return false;
    }

    // 文件夹黑名单列表，遍历文件夹时忽略 
    static private string[] folder_black_list = { "Plugins", "streamingassets", "editor", "resources", "scenes", "scripts",
                                                      "RawData", "[EffectMesh]", "[EffectParticle]", "[EffectSample]", "[Resources]"};
    static public bool IsBlockedByBlackList(string assetPath)
    {
        //List<string> blackListPath = new List<string>(folder_black_list);
        string[] folderNames = assetPath.Split('/');
        foreach (string path in folder_black_list)
        {
            for (int i = 0; i < folderNames.Length; ++i)
            {
                if (string.Compare(path, folderNames[i], true) == 0)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 更新文件的assetbundle name，将其设置为所在文件夹名称
    /// </summary>
    /// <param name="filePath">文件的完整路径</param>
    static void UpdateAssetBundleName(string filePath)
    {
        if (!ValidExtension(filePath))
        {
            return;
        }

        AssetImporter importer = AssetImporter.GetAtPath(filePath);
        if (importer != null)
        {
            if (Directory.Exists(filePath))
            {
                importer.assetBundleName = "";
                return;
            }

            if (!ValidByProjectFilter(filePath))
            {
                importer.assetBundleName = "";
                return;
            }

            if (IsBlockedBySpecial(filePath))
            {
                importer.assetBundleName = "";
                return;
            }

            int position = filePath.LastIndexOf(@"/");
            string assetBundlePath = filePath.Substring(0, position);
            importer.assetBundleName = assetBundlePath.Replace('/', '_');
            importer.assetBundleVariant = "";
        }
    }

    static public bool ValidExtension(string filePath)
    {
        if (filePath.EndsWith(".meta", System.StringComparison.OrdinalIgnoreCase) || filePath.EndsWith(".cs", System.StringComparison.OrdinalIgnoreCase))
            return false;
        return true;
    }

    // 资源路径是否有效
    static public bool ValidByProjectFilter(string assetPath)
    {
        if (IsPassByWhiteList(assetPath) && !IsBlockedByBlackList(assetPath))
            return true;
        return false;
    }

    /// <summary>
    /// 特殊过滤规则
    /// 动画文件不发布所以不设置asset bundle name，只发布提取出来的数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    static bool IsBlockedBySpecial(string filePath)
    {
        if (filePath.LastIndexOf("KGame/Character", StringComparison.OrdinalIgnoreCase) != -1 && filePath.LastIndexOf("@", StringComparison.OrdinalIgnoreCase) != -1)
        {
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(filePath);
            foreach (UnityEngine.Object obj in objs)
            {
                if (obj is AnimationClip)
                {
                    return true;
                }
            }
        }

        string extension = Path.GetExtension(filePath);
        if (string.Compare(extension, ".spm", true) == 0)
            return true;

        return false;
    }
    #endregion
}