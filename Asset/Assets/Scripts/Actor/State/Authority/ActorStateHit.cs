using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class ActorStateHit : ActorState
    {
        protected bool MoveTo(Vector3 target, ref Vector3 delta)
        {
            Vector3 dist = target - actor.transform.position;
            dist.y = 0;
            if (dist.sqrMagnitude > 0.0001f)
            {
                delta = Vector3.MoveTowards(Vector3.zero, dist, actor.speed * Time.deltaTime);
                return false;
            }
            return true;
        }
    }

    public class ActorStateHitAuthority : ActorStateHit
    {
        Vector3 delta;
        Vector3 dest; // 目标点
        float distance = 3f; // 击退距离

        public ActorStateHitAuthority(int id) { ID = id; }

        public override void OnEnter(int preState)
        {
            dest = actor.transform.position - actor.transform.forward * distance;
            actor.animationController.Play("unarmed-gethit-b1", true);
        }

        public override void OnExist(int nextState)
        {
        }

        public override void Tick()
        {
            bool valid = MoveTo(dest, ref delta);
            if (!valid)
            {
                actor.physicsController.MoveDelta(delta);
            }
            else
            {
                actor.stateController.ChangeStateIdle();
            }
        }
    }

    public class ActorStateHitEnemy : ActorStateHit
    {
        public ActorStateHitEnemy(int id) { ID = id; }

        public override void OnEnter(int preState)
        {
            actor.animationController.Play("unarmed-idle", false);
        }

        public override void OnExist(int nextState)
        {
        }

        public override void Tick()
        {
            if (actor.animationController.IsFinished())
            {
                actor.stateController.ChangeStateIdle();
            }
        }
    }
}