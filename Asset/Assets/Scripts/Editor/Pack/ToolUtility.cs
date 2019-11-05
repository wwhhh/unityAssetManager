using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace Pack
{
    public class ToolUtility
    {
        public static void DelayFrameCall(int frame, EditorApplication.CallbackFunction a)
        {
            if (frame < 1) return;

            int total = frame;

            EditorApplication.CallbackFunction up = null;
            up = () =>
            {
                total--;
                if (total <= 0)
                {
                    EditorApplication.update -= up;
                    a();
                }
            };

            EditorApplication.update += up;
        }

        public static void SaveToFile(string fullPath, string content)
        {
            string path = System.IO.Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(path);
            StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8);
            sw.Write(content);
            sw.Close();
        }

        public static void ClearDirectory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists) return;

            foreach (var i in info.GetDirectories())
            {
                i.Delete(true);
            }
            foreach (var i in info.GetFiles())
            {
                i.Delete();
            }
        }

        static Texture2D _iconWarning;
        static public Texture2D GetWarningIcon()
        {
            if (!_iconWarning)
            {
                _iconWarning = EditorGUIUtility.FindTexture("console.warnicon");
            }
            return _iconWarning;
        }

        static public string GetKbMb(long size)
        {
            if (size > 1024 * 1024)
            {
                float s = size / 1024f / 1024f;
                s = ((int)(s * 100)) / 100f;
                return s.ToString() + "M";
            }
            else if (size > 1024)
            {
                float s = size / 1024f;
                s = ((int)(s * 100)) / 100f;
                return s.ToString() + "K";
            }
            else
            {
                return size.ToString();
            }
        }

        static public void CopyToClipboard(string str)
        {
            TextEditor textEd = new TextEditor();
            textEd.text = str;//Òª¸´ÖÆµÄ×Ö·û´®
            textEd.OnFocus();
            textEd.Copy();
        }

        static public bool ValidateAsset(string name, Object obj)
        {
            string ext = System.IO.Path.GetExtension(name);
            if (ext == ".cs" || ext == ".dll")
                return false;
            if (name == "Assets/Timeline/Cinemachine/Base/Editor/Res/cm_logo_sm.png")
                return false;
            if (obj.name == "GUI/Text Shader") return false;//TODO
            return true;
        }

    }
}