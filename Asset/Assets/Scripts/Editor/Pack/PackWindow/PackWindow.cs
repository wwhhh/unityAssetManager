using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Pack
{
    public class PackWindow : EditorWindow
    {
        static PackWindow _instance;
        public static void SwitchTools()
        {
            if (_instance == null)
            {
                OpenTools();
            }
            else
            {
                CloseTools();
            }
        }

        static void OpenTools()
        {
            if (_instance != null) return;
            List<ToolOption> list = new List<ToolOption>()
            {
                new PackOption()
            };

            if (EditorApplication.isCompiling)
            {
                Debug.LogWarning("is compiling! Hold on!");
                return;
            }
            _instance = (PackWindow)GetWindow(typeof(PackWindow), true, "PackWindow");

            _instance.Show();
            _instance.Init(list);
        }

        static void CloseTools()
        {
            if (_instance == null) return;
            _instance.Close();
            _instance = null;
        }

        ToolCache _cache;
        List<ToolOption> _tools;
        public void Init(List<ToolOption> tools)
        {
            _tools = tools;
            _cache = new ToolCache("Tools");
            foreach (var tool in tools)
            {
                tool.window = this;
            }
        }

        void OnDestroy()
        {
            foreach (var tool in _tools)
            {
                //if (tool.inited)
                {
                    tool.OnDestroy();
                }
            }
        }

        void OnGUI()
        {
            if (EditorApplication.isCompiling) return;

            string[] toolTexts = new string[_tools.Count];
            for (int i = 0; i < _tools.Count; ++i)
            {
                toolTexts[i] = _tools[i].GetName();
            }

            int toolSel = 0;

            const int interval = 3;
            const int leftWidth = 150;
            const int totalHeight = 1000;
            const int rightWidth = 1700;

            GUILayout.Space(interval);

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(EditorStyles.helpBox/*box*/, GUILayout.Width(leftWidth), GUILayout.Height(totalHeight));
                {
                    toolSel = _cache.SelectionGrid("toolSel", toolTexts, 1);
                }
                GUILayout.EndVertical();

                GUILayout.Space(interval);

                GUILayout.BeginVertical("box", GUILayout.Width(rightWidth), GUILayout.Height(totalHeight));
                {
                    if (toolSel >= _tools.Count)
                        toolSel = 0;
                    ToolOption tool = _tools[toolSel];
                    if (!tool.inited)
                    {
                        tool.inited = true;
                        tool.Init();
                    }
                    tool.OnGUI(interval + leftWidth, interval);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        public static void ShowNotify(string str)
        {
            if (_instance == null) return;
            _instance.ShowNotification(new GUIContent(str));
        }

        /// <summary>
        /// 关闭  Tools  窗口后再回调，否则会有错误
        /// </summary>
        /// <param name="a"></param>
        public static void CloseAndCall(EditorApplication.CallbackFunction a)
        {
            EditorApplication.delayCall += a;
            CloseTools();
        }

        [MenuItem("打包工具/打包窗口 %t")]
        static void Tools()
        {
            Debug.Log("Tools");
            SwitchTools();
        }
    }
}