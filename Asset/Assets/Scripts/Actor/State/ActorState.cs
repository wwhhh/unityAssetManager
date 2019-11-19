using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class ActorState
    {

        public Actor actor { get; set; }

        public int ID = 0;

        public virtual void OnEnter(int preState) { }

        public virtual void OnExist(int nextState) { }

        public virtual void Tick() { }

        protected bool HasMoveInput()
        {
            bool b = InputManager.I.JoystickUse;
            if (!b) return false;

            return b;
        }

        protected Vector3 TickPositionAuthority()
        {
            Vector3 delta = actor.rotationController.currForward * actor.speed * Time.deltaTime;
            actor.physicsController.MoveDelta(actor.speed, actor.rotationController.currForward);
            return delta;
        }

        protected void TickRotationAuthority()
        {
            if (HasMoveInput())
            {
                DoTickRotation(InputManager.I.GetMoveForward());
            }
            else
            {
                DoTickRotation(actor.rotationController.currForward);
            }
        }

        protected void DoTickRotation(Vector3 targetForward)
        {
            targetForward.y = 0;

            if (targetForward != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetForward);
                actor.rotationController.To(targetRot);
            }
        }

        protected bool TryIdle()
        {
            int curState = actor.stateController.GetCurStateId();

            if (curState == EActorState.Idle)
            {
                return false;
            }
            else
            {
                if (!HasMoveInput())
                {
                    actor.stateController.ChangeStateIdle();
                    return true;
                }
            }
            return false;
        }

        protected bool TryMove()
        {
            if (HasMoveInput())
            {
                actor.stateController.ChangeStateMove();
                return true;
            }

            return false;
        }

        protected bool TrySkill()
        {
            return actor.skillController.TrySkill();
        }

    }

}