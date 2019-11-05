using UnityEditor;
using UnityEngine;

public class CommonEditor
{

    [MenuItem("CommonEditor/复制当前资源路径")]
    static void CopyPath()
    {
        Object o = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets)[0];
        if (o == null) return;

        string path = AssetDatabase.GetAssetPath(o);
        TextEditor text = new TextEditor();
        text.text = path.ToLower();
        text.SelectAll();
        text.Copy();
    }
}
