using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XSkillBehavior
{
    public int id;

    public XSkillBehaviorAnim anim;
}

[System.Serializable]
public class XSkillBehaviorBase
{
    public float startTick;
    public float duration;
    public int id;
    public float lockTime;
}

[System.Serializable]
public class XSkillBehaviorAnim : XSkillBehaviorBase
{
    public string animName;
}

[CreateAssetMenu(menuName = "创建技能配置", fileName = "SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public List<XSkillBehavior> skills;

    //public static SkillConfig Load(string path)
    //{

    //}

}