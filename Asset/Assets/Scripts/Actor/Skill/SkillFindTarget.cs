using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActorCore;
using System;

public static class SkillFindTarget
{

    public static List<Actor> targets = new List<Actor>();
    static List<Actor> _lstSearch = new List<Actor>();

    public static void FindTargets(XSkillBehaviorProcessNode xNode, Skill skill)
    {
        targets.Clear();
        _lstSearch.Clear();

        switch (xNode.shapeType)
        {
            case EShapeType.None:
                {
                    if (skill.target) targets.Add(skill.target);
                }
                break;
            case EShapeType.Rect:
                FindTargetsInRect(xNode, skill);
                break;
            default:
                break;
        }
    }

    static void FindTargetsInRect(XSkillBehaviorProcessNode xNode, Skill skill)
    {
        float width = xNode.width;
        float length = xNode.length;
        Vector3 pos = GetCenter(xNode, skill);

        Vector3 right = skill.actor.transform.right;
        Vector3 forward = skill.actor.transform.forward;
        right.y = 0f;
        forward.y = 0f;
        right.Normalize();
        forward.Normalize();

        Vector3 halfright = right * width / 2f;
        Vector3 halforward = forward * length / 2f;
        Vector3 p0 = pos - halfright - halforward;
        Vector3 p1 = pos + halfright - halforward;
        Vector3 p2 = pos - halfright + halforward;
        Vector3 ex = p1 - p0;
        Vector3 ez = p2 - p0;
        ex.y = 0f;
        ez.y = 0f;


    }

    static Vector3 GetCenter(XSkillBehaviorProcessNode xNode, Skill skill)
    {
        Vector3 pos = skill.actor.transform.position;
        return pos;
    }

}