using UnityEditor;

public class AnimationEditor : AssetPostprocessor
{
    void OnPreprocessAnimation()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        if (modelImporter.clipAnimations.Length == 0)       // 首次导入时clipAnimations为空，需要做特殊处理
            modelImporter.clipAnimations = modelImporter.defaultClipAnimations;
    }
}