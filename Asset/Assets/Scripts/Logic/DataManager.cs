using Asset;

public class DataManager : Singleton<DataManager>
{

    public SkillConfig skillConfig;

    public override void Init()
    {
        base.Init();
        LoadSkillConfig();
    }

    private void LoadSkillConfig()
    {
        AssetLoader<SkillConfig> assetLoader = AssetManager.LoadAsset<SkillConfig>("assets/game/common/skillconfig.asset");
        skillConfig = assetLoader.asset;
    }

}