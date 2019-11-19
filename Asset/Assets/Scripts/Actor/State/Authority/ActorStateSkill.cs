using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class ActorStateSkill : ActorState
    {
        public ActorStateSkill(int id)
        {
            ID = id;
        }
    }

    public class ActorStateSkillAuthority : ActorState
    {
        public ActorStateSkillAuthority(int id)
        {
            ID = id;
        }

        public override void OnEnter(int preState)
        {
        }

        public override void OnExist(int nextState)
        {
        }

        public override void Tick()
        {

            if (TrySkill()) return;

            if (actor.skillController.IsFinish())
            {
                actor.stateController.ChangeStateIdle();
            }
        }
    }

    public class ActorStateSkillEnemy : ActorState
    {
        public ActorStateSkillEnemy(int id)
        {
            ID = id;
        }
    }

}