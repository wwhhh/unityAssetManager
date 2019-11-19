using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class StateController : IActorController
    {
#if UNITY_EDITOR
        public string CurState { get { return EActorState.ToString(_curState.ID); } }
#endif
        private ActorState _curState = null;

        private Dictionary<int, ActorState> _dicStates = new Dictionary<int, ActorState>();

        public override void Init(Actor actor)
        {
            this.actor = actor;
            InitStates();
        }

        public override void Dispose()
        {
        }

        protected virtual void InitStates()
        {
            AddState(new ActorStateIdle(EActorState.Idle));
            AddState(new ActorStateMove(EActorState.Move));
            AddState(new ActorStateSkill(EActorState.Skill));
        }

        public void ChangeState(int id)
        {
            _dicStates.TryGetValue(id, out ActorState nextState);
            if (nextState == null)
            {
                Debug.LogError(id + " state not found");
            }

            int preId = EActorState.None;
            if (_curState != null)
            {
                preId = _curState.ID;
                _curState.OnExist(id);
            }

            Debug.Log("State Changed From id=" + EActorState.ToString(preId) + " To id=" + EActorState.ToString(nextState.ID) + " time:" + Time.time);

            _curState = nextState;
            _curState.OnEnter(preId);
        }

        public int GetCurStateId()
        {
            return _curState.ID;
        }

        protected void AddState(ActorState state)
        {
            state.actor = actor;
            _dicStates.Add(state.ID, state);
        }

        public override void Update()
        {
            if (_curState != null)
                _curState.Tick();
        }

        #region 状态切换

        public void ChangeStateIdle()
        {
            ChangeState(EActorState.Idle);
        }

        public void ChangeStateSkill()
        {
            ChangeState(EActorState.Skill);
        }

        public void ChangeStateMove()
        {
            ChangeState(EActorState.Move);
        }

        public void ChangeStateHit()
        {
            ChangeState(EActorState.Hit);
        }

        #endregion

    }

    public class StateControllerAuthority : StateController
    {

        protected override void InitStates()
        {
            AddState(new ActorStateIdleAuthority(EActorState.Idle));
            AddState(new ActorStateMoveAuthority(EActorState.Move));
            AddState(new ActorStateSkillAuthority(EActorState.Skill));
            AddState(new ActorStateHitAuthority(EActorState.Hit));
        }

    }

    public class StateControllerEnemy : StateController
    {

        protected override void InitStates()
        {
            AddState(new ActorStateIdleEnemy(EActorState.Idle));
            AddState(new ActorStateMoveEnemy(EActorState.Move));
            AddState(new ActorStateSkillEnemy(EActorState.Skill));
            AddState(new ActorStateHitEnemy(EActorState.Hit));
        }

    }

}