using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehaviorNode : ISkillBehaviorNode
{
    public float startTick { get; private set; }
    public float duration { get; private set; }

    public virtual void Begin()
    {
    }

    public virtual void End()
    {
    }

    public virtual void Init(SkillConfig config)
    {
    }

    public virtual void Tick(float deltaTime)
    {
    }
}