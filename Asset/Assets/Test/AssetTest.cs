using UnityEngine;
using Asset;

public class AssetTest : MonoBehaviour
{

    void Start()
    {
        var assetLoader = AssetManager.LoadAsset<GameObject>("assets/game/character/lancem2/lancem2@skin.prefab");
        GameObject go = Instantiate(assetLoader.asset);
    }

}