using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillBehaviorNode
{

    void Init(SkillConfig config);

    void Begin();

    void End();

    void Tick(float deltaTime);

}