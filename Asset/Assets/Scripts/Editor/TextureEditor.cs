using UnityEditor;

public class TextureEditor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        if (!ImportSettingEditor.ValidByProjectFilter(assetPath))
            return;

        TextureImporter ti = assetImporter as TextureImporter;
    }

}