using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class SkillBehaviorBaseNode
    {

        XSkillBehaviorBaseNode _xNodeBase;
        protected Skill _skill;
        protected bool _begin = false;
        protected bool _end = false;
        protected float _startTime;

        public virtual void Init(XSkillBehaviorBaseNode xNode, Skill skill)
        {
            _xNodeBase = xNode;
            _skill = skill;
            _startTime = Time.time;
        }

        private void CheckBegin()
        {
            if (_begin) return;
            if (Time.time - _startTime >= _xNodeBase.startTick)
            {
                _begin = true;
                OnBegin();
            }
        }

        private void CheckEnd()
        {
            if (!_begin) return;
            if (_end) return;
            if (Time.time - _startTime - _xNodeBase.startTick >= _xNodeBase.duration)
            {
                _end = true;
                OnEnd();
            }
        }

        public void Tick()
        {
            CheckBegin();
            CheckEnd();
            if (_begin && !_end)
                OnTick();
        }

        public bool IsFinish()
        {
            return _end;
        }

        public void Stop()
        {
            OnEnd();
            _begin = false;
            _end = false;

            OnRelease();
        }

        protected virtual void OnBegin() { }
        protected virtual void OnEnd() { }
        protected virtual void OnTick() { }
        protected virtual void OnRelease() { }
    }

    public class SkillBehaviorAnimNode : SkillBehaviorBaseNode
    {
        XSkillBehaviorAnimNode _xNode;

        public override void Init(XSkillBehaviorBaseNode xNode, Skill skill)
        {
            base.Init(xNode, skill);

            _xNode = xNode as XSkillBehaviorAnimNode;
        }

        protected override void OnBegin()
        {
            base.OnBegin();

            _skill.actor.animationController.Play(_xNode.animName, false);
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        protected override void OnEnd()
        {
            base.OnEnd();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        #region Pool
        static ObjectPool<SkillBehaviorAnimNode> _pool;
        public static SkillBehaviorAnimNode Get()
        {
            if (_pool == null) _pool = new ObjectPool<SkillBehaviorAnimNode>();
            SkillBehaviorAnimNode ret = _pool.Get();
            return ret;
        }
        public static void Release(SkillBehaviorAnimNode sm)
        {
            if (_pool != null) _pool.Release(sm);
        }
        #endregion
    }

    public class SkillBehaviorProcessNode : SkillBehaviorBaseNode
    {
        XSkillBehaviorProcessNode _xNode;

        public override void Init(XSkillBehaviorBaseNode xNode, Skill skill)
        {
            base.Init(xNode, skill);

            _xNode = xNode as XSkillBehaviorProcessNode;
        }

        protected override void OnBegin()
        {
            base.OnBegin();

            
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        protected override void OnEnd()
        {
            base.OnEnd();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        #region Pool
        static ObjectPool<SkillBehaviorProcessNode> _pool;
        public static SkillBehaviorProcessNode Get()
        {
            if (_pool == null) _pool = new ObjectPool<SkillBehaviorProcessNode>();
            SkillBehaviorProcessNode ret = _pool.Get();
            return ret;
        }
        public static void Release(SkillBehaviorProcessNode sm)
        {
            if (_pool != null) _pool.Release(sm);
        }
        #endregion
    }

}