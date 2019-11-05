using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Pack
{
    public class ToolCache
    {
        Dictionary<string, bool> _dicBool = new Dictionary<string, bool>();
        Dictionary<string, string> _dicString = new Dictionary<string, string>();
        Dictionary<string, int> _dicInt = new Dictionary<string, int>();

        string _fullPath = "";
        public ToolCache(string name)
        {
            _fullPath = "./Cache/" + name + ".txt";
            if (!File.Exists(_fullPath)) return;

            string[] lines = File.ReadAllLines(_fullPath);
            foreach (string line in lines)
            {
                //Debug.Log(line);
                string[] arr = line.Split('|');
                if (arr.Length == 2)
                {
                    _dicString.Add(arr[0], arr[1]);
                }
                else
                {
                    Debug.LogWarning(line + " error");
                }
            }
        }

        public void Save()
        {
            //Debug.Log("save cache");
            StringBuilder sb = new StringBuilder();
            foreach (var kv in _dicBool)
            {
                sb.AppendLine(kv.Key + "|" + (kv.Value ? "1" : "0"));
            }

            foreach (var kv in _dicString)
            {
                sb.AppendLine(kv.Key + "|" + kv.Value);
            }

            foreach (var kv in _dicInt)
            {
                sb.AppendLine(kv.Key + "|" + (kv.Value.ToString()));
            }

            ToolUtility.SaveToFile(_fullPath, sb.ToString());
        }

        public string GetStr(string name, string def = "")
        {
            string ret;
            if (_dicString.TryGetValue(name, out ret))
            {
            }
            else
            {
                ret = def;
            }
            return ret;
        }

        public bool GetBool(string name, bool def = false)
        {
            bool ret;
            if (_dicBool.TryGetValue(name, out ret))
            {
            }
            else
            {
                string str;
                if (_dicString.TryGetValue(name, out str))
                {
                    ret = str == "1";
                    _dicString.Remove(name);
                }
                else
                {
                    ret = def;
                }

                _dicBool.Add(name, ret);
            }

            return ret;
        }

        public int GetInt(string name, int def = 0)
        {
            int ret;
            if (_dicInt.TryGetValue(name, out ret))
            {
            }
            else
            {
                string str;
                if (_dicString.TryGetValue(name, out str))
                {
                    ret = int.Parse(str);
                    _dicString.Remove(name);
                }
                else
                {
                    ret = def;
                }
                _dicInt.Add(name, ret);
            }

            return ret;
        }

        public void SetDefStr(string name, string def)
        {
            if (!_dicString.ContainsKey(name))
            {
                _dicString[name] = def;
            }
        }

        public void SetDefBool(string name, bool def)
        {
            if (!_dicString.ContainsKey(name) && !_dicBool.ContainsKey(name))
            {
                _dicBool[name] = def;
            }
        }

        public void SetDefInt(string name, int def)
        {
            if (!_dicString.ContainsKey(name) && !_dicInt.ContainsKey(name))
            {
                _dicInt[name] = def;
            }
        }

        public void SetStr(string name, string v)
        {
            string old = GetStr(name);
            if (old != v)
            {
                _dicString[name] = v;
                Save();
            }
        }

        public void SetBool(string name, bool v)
        {
            bool old = GetBool(name);
            if (old != v)
            {
                _dicBool[name] = v;
                Save();
            }
        }

        public void SetInt(string name, int v)
        {
            int old = GetInt(name);
            if (old != v)
            {
                _dicInt[name] = v;
                Save();
            }
        }

        public string StrField(string name, string label, float len)
        {
            string ret = "";
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label, GUILayout.Width(len));
                string old = GetStr(name);
                ret = GUILayout.TextField(old);

                _dicString[name] = ret;

                if (old != ret)
                {
                    Save();
                }
            }
            GUILayout.EndHorizontal();
            return ret;
        }

        public int IntField(string name, string label, float len)
        {
            int ret = 0;
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label, GUILayout.Width(len));
                int old = GetInt(name);
                ret = EditorGUILayout.IntField(old);

                _dicInt[name] = ret;

                if (old != ret)
                {
                    Save();
                }
            }
            GUILayout.EndHorizontal();
            return ret;
        }

        //GUILayout.Toggle(_checks[i], ((CopyItem)i).ToString());
        public int SelectionGrid(string name, string[] texts, int xCount)
        {
            if (texts.Length <= 0) return -1;
            int old = GetInt(name);
            if (old >= texts.Length)
            {
                old = 0;
            }
            int ret = GUILayout.SelectionGrid(old, texts, xCount);
            _dicInt[name] = ret;

            if (old != ret)
            {
                Save();
            }
            return ret;
        }

        public bool Toggle(string name, string text)
        {
            bool old = GetBool(name);
            bool ret = GUILayout.Toggle(old, text);
            _dicBool[name] = ret;


            if (old != ret)
            {
                Save();
            }
            return ret;
        }

        public int Toolbar(string name, string[] texts)
        {
            int old = GetInt(name);
            int ret = GUILayout.Toolbar(GetInt(name), texts);
            _dicInt[name] = ret;

            if (old != ret)
            {
                Save();
            }
            return ret;
        }
    }
}