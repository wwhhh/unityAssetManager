using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : ISkillNode
{

    int _skillId;
    float _startTime, _endTime;
    float _curTime;
    LinkedList<SkillBehaviorNode> _behaviorNodes = new LinkedList<SkillBehaviorNode>();

    SkillConfig _config;

    public SkillNode()
    {

    }

    public void Begin()
    {
        _startTime = Time.time;
    }

    public void End()
    {
    }

    public void Tick(float deltaTime)
    {
        
    }

}