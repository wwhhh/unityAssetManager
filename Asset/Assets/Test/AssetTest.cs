using UnityEngine;
using UnityEngine.UI;
using Asset;

public class AssetTest : MonoBehaviour
{

    public RawImage image;

    void Start()
    {
        var assetLoader = AssetManager.LoadAsset<Texture>("assets/game/ui/0texture/creat_bg.png");
        image.texture = assetLoader.asset;
    }

}