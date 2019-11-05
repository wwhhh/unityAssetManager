using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasEditor : Editor
{
    static string SPRITE_ATLAS_PATH = "Assets/Game/UI/Atlas";
    static string SPRITE_PATH = "Assets/Game/UI/RawData";

    [MenuItem("打包工具/图集处理/UGUISprite自动生成Atlas", false, 5001)]
    public static void AutoGenerateAtlas()
    {
        AutoGenerateAtlas(false);
    }
    [MenuItem("打包工具/图集处理/UGUISprite自动生成Atlas（重新生成）", false, 5002)]
    public static void AutoRegenerateAtlas()
    {
        AutoGenerateAtlas(true);
    }
    public static void AutoGenerateAtlas(bool reset)
    {
        Debug.Log("开始自动生成图集处理。");
        var realFolderList = new List<string>();
        GetRealFolderRecursion(realFolderList, SPRITE_PATH);
        var desFolders = new List<string>(AssetDatabase.GetSubFolders(SPRITE_ATLAS_PATH).Select(x => Path.GetFileName(x)));
        var existAtlases = new List<string>(AssetDatabase.FindAssets("t:spriteatlas").Select(x => AssetDatabase.GUIDToAssetPath(x)).Select(x => Path.GetFileName(x)));
        var i = 0;
        foreach (var item in realFolderList)
        {
            var folderName = Path.GetFileName(item);
            if (!desFolders.Contains(folderName))
                AssetDatabase.CreateFolder(SPRITE_ATLAS_PATH, folderName);

            var folder = SPRITE_ATLAS_PATH + "/" + folderName;
            var targetAtlas = folderName.ToLower() + ".spriteatlas";
            EditorUtility.DisplayProgressBar("自动生成", "正在处理：" + folder + "/" + targetAtlas, i++ * 1f / realFolderList.Count);
            if (existAtlases.Select(x => x == targetAtlas).Where(x => x == true).Count() > 0 && !reset)
            {
                EditorUtility.DisplayProgressBar("自动生成", "正在处理：" + folder + "/" + targetAtlas, i * 1f / realFolderList.Count);
                continue;
            }

            var atlas = ObjectFactory.CreateInstance<SpriteAtlas>();
            atlas.name = folderName.ToLower();
            atlas.Add(new Object[] { AssetDatabase.LoadAssetAtPath<DefaultAsset>(item) });
            var v = atlas.GetPackingSettings();
            v.padding = 2;
            v.enableRotation = false;
            v.enableTightPacking = false;
            atlas.SetPackingSettings(v);
            SetPlatformSettings(atlas);
            AssetDatabase.CreateAsset(atlas, folder + "/" + atlas.name + ".spriteatlas");
            EditorUtility.DisplayProgressBar("自动生成", "正在处理：" + folder + "/" + targetAtlas, i * 1f / realFolderList.Count);
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
        SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.SaveAssets();
        Debug.Log("检查并自动生成图集=>已完成");
    }
    static void GetRealFolderRecursion(List<string> realFolderList, string folder)
    {
        var srcFolders = AssetDatabase.GetSubFolders(folder);
        foreach (var item in srcFolders)
            GetRealFolderRecursion(realFolderList, item);
        if (srcFolders.Length == 0)
            realFolderList.Add(folder);
    }
    static void SetPlatformSettings(SpriteAtlas spriteAtlas)
    {
        spriteAtlas.SetPlatformSettings(new TextureImporterPlatformSettings()
        {
            name = "Standalone",
            overridden = true,
            allowsAlphaSplitting = false,
            compressionQuality = 50,
            maxTextureSize = 2048,
            textureCompression = TextureImporterCompression.Compressed,
            format = TextureImporterFormat.DXT5,
        });
        spriteAtlas.SetPlatformSettings(new TextureImporterPlatformSettings()
        {
            name = "Android",
            overridden = true,
            allowsAlphaSplitting = false,
            compressionQuality = 50,
            maxTextureSize = 2048,
            textureCompression = TextureImporterCompression.Compressed,
            format = TextureImporterFormat.ETC2_RGBA8,
        });
        spriteAtlas.SetPlatformSettings(new TextureImporterPlatformSettings()
        {
            name = "iPhone",
            overridden = true,
            allowsAlphaSplitting = false,
            compressionQuality = 50,
            maxTextureSize = 2048,
            textureCompression = TextureImporterCompression.Compressed,
            format = TextureImporterFormat.ASTC_RGBA_6x6,
        });
    }
}