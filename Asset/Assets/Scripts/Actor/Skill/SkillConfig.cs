using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//伤害判定形状
public enum EShapeType
{
    None,//使用锁定的目标
    Rect,//矩形
}

[System.Serializable]
public class XSkillBehavior
{
    public int id;
    public float lockTime;
    public XSkillBehaviorAnimNode anim;
    public XSkillBehaviorProcessNode process;
}

[System.Serializable]
public class XSkillBehaviorBaseNode
{
    public float startTick;
    public float duration;
}

[System.Serializable]
public class XSkillBehaviorAnimNode : XSkillBehaviorBaseNode
{
    [Tooltip("动画名称")]
    public string animName;
}

[System.Serializable]
public class XSkillBehaviorProcessNode : XSkillBehaviorBaseNode
{
    [Tooltip("形状")]
    public EShapeType shapeType;
    [Tooltip("中心点偏移")]
    public Vector2 offset;
    [Tooltip("矩形宽")]
    public float width;
    [Tooltip("矩形高 目标和施放者高度差")]
    public float height;
    [Tooltip("矩形长")]
    public float length;
}

[CreateAssetMenu(menuName = "创建技能配置", fileName = "SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public List<XSkillBehavior> skills;

    public XSkillBehavior GetSkill(int id)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].id == id) return skills[i];
        }

        return null;
    }

}