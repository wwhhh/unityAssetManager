using System.Collections.Generic;
using UnityEngine;

public class SingletonBase
{
    protected static List<SingletonBase> _listSingle = new List<SingletonBase>();
    public static void DestroyAll()
    {
        for (int i = _listSingle.Count - 1; i >= 0; --i)
        {
            SingletonBase s = _listSingle[i];
            s.Destroy();
        }
        if (_listSingle.Count != 0)
        {
            Debug.LogError("s_lst.Count != 0");
        }
        _listSingle.Clear();
    }

    virtual public void Init()
    {

    }

    virtual protected void OnDestroy()
    {

    }

    virtual public void Destroy()
    {
    }

}

public abstract class Singleton<T> : SingletonBase where T : SingletonBase, new()
{
    private static T _instance = null;
    public static T I
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Init();
                _listSingle.Add(_instance);
            }
            return _instance;
        }
    }

    override public void Destroy()
    {
        OnDestroy();
        _listSingle.Remove(_instance);
        Debug.Log("--------------- " + _instance.GetType().ToString());
        _instance = null;
    }
}