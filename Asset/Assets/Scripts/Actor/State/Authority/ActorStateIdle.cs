using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class ActorStateIdle : ActorState
    {
        public ActorStateIdle(int id) { ID = id; }
    }

    public class ActorStateIdleAuthority : ActorState
    {

        public ActorStateIdleAuthority(int id) { ID = id; }

        public override void OnEnter(int preState)
        {
            actor.animationController.Play("unarmed-idle", false);
        }

        public override void OnExist(int nextState)
        {
        }

        public override void Tick()
        {
            if (TryMove()) return;
        }
    }

    public class ActorStateIdleEnemy : ActorState
    {

        public ActorStateIdleEnemy(int id)
        {
            ID = id;
        }

        public override void OnEnter(int preState)
        {
            actor.animationController.Play("unarmed-idle", false);
        }

        public override void OnExist(int nextState)
        {
        }

        public override void Tick()
        {
        }
    }

}