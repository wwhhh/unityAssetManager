using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class Skill
    {

        public Actor actor;

        public Actor target;//目标
        public Vector3 targetPos;//目标点
        public float targetRadian;//朝向

        int _skillId;
        bool _finish;
        float _startTime;
        XSkillBehavior _behaviorConfig;
        List<SkillBehaviorBaseNode> _nodes = new List<SkillBehaviorBaseNode>();

        public void Init(Actor actor, int skillId)
        {
            this.actor = actor;
            _skillId = skillId;
            _finish = true;

            _behaviorConfig = DataManager.I.skillConfig.GetSkill(_skillId);
        }

        public void Execute()
        {
            _finish = false;
            _startTime = Time.time;
            InitNodes();
        }

        public void Tick()
        {
            if (_finish) return;
            bool allFinish = true;
            for (int i = 0, imax = _nodes.Count; i < imax; ++i)
            {
                SkillBehaviorBaseNode node = _nodes[i];
                node.Tick();
                if (!node.IsFinish()) allFinish = false;
            }

            if (allFinish)
            {
                _nodes.Clear();
                _finish = true;
            }
        }

        public bool IsFinish()
        {
            return _finish;
        }

        private void ClearNodes()
        {
            for (int i = 0, imax = _nodes.Count; i < imax; ++i)
            {
                SkillBehaviorBaseNode node = _nodes[i];
                node.Stop();
            }
            _nodes.Clear();
        }

        private void InitNodes()
        {
            ClearNodes();

            XSkillBehaviorAnimNode xNode = _behaviorConfig.anim;
            SkillBehaviorAnimNode node = SkillBehaviorAnimNode.Get();
            node.Init(xNode, this);
            _nodes.Add(node);
        }

        #region Pool
        static ObjectPool<Skill> _pool;
        public static Skill Get()
        {
            if (_pool == null) _pool = new ObjectPool<Skill>();
            return _pool.Get();
        }
        public static void Release(Skill sm)
        {
            if (_pool != null) _pool.Release(sm);
        }
        #endregion

    }
}