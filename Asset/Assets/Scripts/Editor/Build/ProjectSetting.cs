using Pack;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProjectSetting : Editor
{

    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene == null)
                continue;
            if (scene.enabled && !string.IsNullOrEmpty(scene.path))
            {
                names.Add(scene.path);
            }
        }
        return names.ToArray();
    }

    public static void BuildForWin()
    {
        PackConfig config = new PackConfig();
        config.ResVersion = Application.version;
        config.Load("win", false);
        EditorUserBuildSettings.development = config.Development;//貌似无效
        EditorUserBuildSettings.connectProfiler = config.Development;
        EditorUserBuildSettings.allowDebugging = config.Development;
        BuildOptions option = config.Development ? BuildOptions.Development | BuildOptions.AllowDebugging : BuildOptions.None;//这个有效
        AtlasEditor.AutoRegenerateAtlas();
        PackUtility.BuildAB(config);
        //PackWindow.PackLua(config);
        string path = "./Exe/Client.exe";
        string dir = Path.GetDirectoryName(path);
        ToolUtility.ClearDirectory(dir);
        Directory.CreateDirectory(dir);
        BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.StandaloneWindows64, option);
    }

    static string GetVersion()
    {
        return "0.0.0";
    }

    static string GetParam(string name, string def = "")
    {
        string[] arguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; i++)
        {
            if (arguments[i].StartsWith(name))
            {
                return arguments[i].Split("-"[0])[1];
            }
        }
        return def;
    }

}