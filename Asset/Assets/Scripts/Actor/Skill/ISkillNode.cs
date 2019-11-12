using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillNode
{

    void Begin();

    void End();

    void Tick(float deltaTime);

}