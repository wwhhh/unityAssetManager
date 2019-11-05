using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pack
{
    public class ToolOption
    {
        public PackWindow window;
        public bool inited = false;
        protected ToolCache _cache;

        virtual public string GetName()
        {
            Debug.LogError("Get Name Error");
            return "";
        }
        virtual public void Init()
        {
            _cache = new ToolCache(GetName());
        }

        virtual public void OnGUI(int x, int y)
        {

        }

        virtual public void OnDestroy()
        {

        }

        protected bool GetBool(object e)
        {
            return _cache.GetBool(e.ToString());
        }

        protected int GetInt(object e)
        {
            return _cache.GetInt(e.ToString());
        }
    }
}