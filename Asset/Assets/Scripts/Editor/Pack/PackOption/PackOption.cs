using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pack
{
    class FileItem
    {
        public string shortPath;
        public uint size;
    }

    public class PackConfig
    {
        public string platform;
        public int ClientType;
        public string CDN;
        public string CVM;
        public string ResVersion;
        public bool Development;

        public void Load(string section, bool test)
        {
            INIParser parser = new INIParser();
            parser.Open("./Assets/Scripts/Editor/Pack/PackWindow/PackConfig.ini");
            if (!parser.IsSectionExists(section))
            {
                Debug.LogError(section + @" section not found in Assets\Scripts\Editor\Pack\PackWindow\PackConfig.ini");
                return;
            }
            ClientType = parser.ReadValue(section, "ClientType", 0);
            Development = parser.ReadValue(section, "Development", false);
            platform = GetPlatform();

            //if (ClientType == 0) return;

            CDN = parser.ReadValue(section, "CDN", "");
            CVM = parser.ReadValue(section, "CVM", "");

            if (test)
            {
                CDN += "/0";
                CVM += "/0";
            }
            else
            {
                CDN += "/1";
                CVM += "/1";
            }
        }

        public static string GetPlatform()
        {
            BuildTarget targetPlatform = EditorUserBuildSettings.activeBuildTarget;
            string ret = "Win";
            switch (targetPlatform)
            {
                case BuildTarget.StandaloneWindows64:
                    ret = "Win";
                    break;
                case BuildTarget.iOS:
                    ret = "iOS";
                    break;
                case BuildTarget.Android:
                    ret = "Android";
                    break;
            }
            return ret;
        }
    }

    public class PackOption : ToolOption
    {
        enum Choice
        {
            Test,
            GenerateSpriteAtlas,
            BuildAB,
            //PackLua,
            GenerateResList,
            PackFull,
            ToCDN,
            ToCVM,
        }

        override public string GetName() { return "打包"; }

        string[] _clients;
        string _resVersion = "";

        override public void Init()
        {
            base.Init();

            INIParser parser = new INIParser();
            parser.Open("./Assets/Scripts/Editor/Pack/PackWindow/PackConfig.ini");
            _clients = new string[parser.Sections.Count];
            int i = 0;
            foreach (var kv in parser.Sections)
            {
                _clients[i] = kv.Key;
                i++;
            }
        }

        public override void OnDestroy()
        {
            if (!inited) return;
        }

        public static string GetPlatform()
        {
            BuildTarget targetPlatform = EditorUserBuildSettings.activeBuildTarget;
            string ret = "Win";
            switch (targetPlatform)
            {
                case BuildTarget.StandaloneWindows64:
                    ret = "Win";
                    break;
                case BuildTarget.iOS:
                    ret = "iOS";
                    break;
                case BuildTarget.Android:
                    ret = "Android";
                    break;
            }
            return ret;
        }

        override public void OnGUI(int x, int y)
        {
            string[] choices = System.Enum.GetNames(typeof(Choice));

            GUILayout.Space(50);

            GUILayout.BeginHorizontal();
            {
                //选择目录
                GUILayout.BeginVertical(GUILayout.Width(200));
                {
                    _cache.SelectionGrid("Client", _clients, 1);
                }
                GUILayout.EndVertical();

                GUILayout.Space(20);

                GUILayout.Space(50);

                //CopyItem  选项
                GUILayout.BeginVertical(GUILayout.Width(400));
                {
                    _resVersion = _cache.StrField("ResVersion", "ResVersion", 70);
                    GUILayout.Space(20);

                    for (int i = 0; i < choices.Length; ++i)
                    {
                        string name = choices[i];
                        _cache.Toggle(name, name);
                    }
                    GUILayout.Space(50);

                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("All"))
                        {
                            for (int i = 0; i < choices.Length; ++i)
                            {
                                string name = choices[i];
                                _cache.SetBool(name, true);
                            }
                        }

                        if (GUILayout.Button("None"))
                        {
                            for (int i = 0; i < choices.Length; ++i)
                            {
                                string name = choices[i];
                                _cache.SetBool(name, false);
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Run"))
                    {
                        PackWindow.CloseAndCall(Run);
                    }
                    if (GUILayout.Button("Edit"))
                    {
                        Application.OpenURL(@"Assets/Scripts/Editor/Pack/PackWindow/PackConfig.ini");
                    }
                    if (GUILayout.Button("Build Exe"))
                    {
                        PackWindow.CloseAndCall(BuildExe);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        #region 打包流程

        void BuildExe()
        {
            ProjectSetting.BuildForWin();
        }

        void Run()
        {
            PackConfig config = new PackConfig();
            config.Load(_clients[_cache.GetInt("Client")], GetBool(Choice.Test));
            config.ResVersion = _resVersion;
            if (config.ClientType == 0) return;
            if (GetBool(Choice.GenerateSpriteAtlas))
            {
                AtlasEditor.AutoRegenerateAtlas();
            }
            if (GetBool(Choice.BuildAB))
            {
                PackUtility.BuildAB(config);
            }
            //if (GetBool(Choice.PackLua))
            //{
            //    PackLua(config);
            //}
            if (GetBool(Choice.GenerateResList))
            {
            }
            if (GetBool(Choice.PackFull))
            {
            }
            if (GetBool(Choice.ToCDN))
            {
            }
            if (GetBool(Choice.ToCVM))
            {
            }
        }

        #endregion
    }

}