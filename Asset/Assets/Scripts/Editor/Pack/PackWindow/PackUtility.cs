using Pack;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PackUtility : Editor
{

    #region AssetBundle

    public static void BuildAB(PackConfig config)
    {
        string path = string.Format("Res/{0}", config.platform);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string srcAB = string.Format("Res/{0}/AB", config.platform);
        string destAB = string.Format("Res/{0}/{0}", config.platform);
        string srcABManifest = string.Format("Res/{0}/AB.manifest", config.platform);
        string destABManifest = string.Format("Res/{0}/{0}.manifest", config.platform);

        MoveFile(srcAB, destAB);
        MoveFile(srcABManifest, destABManifest);

        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
        if (BuildPipeline.BuildAssetBundles(path, options, target) != null)
        {
            Debug.Log("Success to building AssetBundles for " + target.ToString());
        }
        else
        {
            Debug.LogError("error: Failed to building AssetBundles for " + target.ToString());
            throw new Exception("error: Failed to building AssetBundles");
        }

        MoveFile(destAB, srcAB);
        MoveFile(destABManifest, srcABManifest);

        RemoveUnusedAB(path);
        Debug.Log("Remove unused AB finish");
    }

    static void MoveFile(string src, string dest)
    {
        if (File.Exists(src))
        {
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }
            File.Move(src, dest);
        }
    }

    static void RemoveUnusedAB(string path)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(path + "/AB");
        AssetBundleManifest manifest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        string[] abs = manifest.GetAllAssetBundles();
        ab.Unload(true);
        Dictionary<string, int> dic = new Dictionary<string, int>();
        foreach (string s in abs)
        {
            dic[s] = 1;
        }
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        int len = path.Length + 1;
        foreach (string ifile in files)
        {
            string s = ifile.Replace("\\", "/");
            s = s.Substring(len);
            if (s.EndsWith(".manifest")) continue;
            if (s == "AB") continue;
            //if (s == "assets_kgame_sc") continue;
            if (s == "SmallConfig.txt") continue;
            if (s == "ResList.txt") continue;
            if (s == "ServerList.txt") continue;

            if (!dic.ContainsKey(s))
            {
                File.Delete(ifile);
                if (File.Exists(ifile + ".manifest"))
                {
                    File.Delete(ifile + ".manifest");
                }
            }
        }
    }

    #endregion

}