using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class ActorStateMove : ActorState
    {
        public ActorStateMove(int id) { ID = id; }
    }

    public class ActorStateMoveAuthority : ActorState
    {
        public ActorStateMoveAuthority(int id)
        {
            ID = id;
        }

        public override void OnEnter(int preState)
        {
            base.OnEnter(preState);
            actor.animationController.Play("unarmed-run-forward", false);
        }

        public override void OnExist(int nextState)
        {
            base.OnExist(nextState);
        }

        public override void Tick()
        {
            base.Tick();

            TickPositionAuthority();
            TickRotationAuthority();

            if (TryIdle()) return;
        }
    }

    public class ActorStateMoveEnemy : ActorState
    {
        public ActorStateMoveEnemy(int id)
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

        }
    }

}