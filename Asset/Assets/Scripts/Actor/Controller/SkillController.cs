using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ActorCore
{
    public class SkillController : IActorController
    {

        public int SkillId = -1;
        Dictionary<int, Skill> _caches = new Dictionary<int, Skill>();
        Skill _curSkill;

        public override void Init(Actor actor)
        {
            this.actor = actor;
        }

        public override void Update()
        {
            if (_curSkill == null) return;
            _curSkill.Tick();
        }

        public bool TrySkill()
        {
            int skillId = SkillId;
            if (skillId == -1) return false;//没有技能

            Skill skill = GetSkill(skillId);
            skill.Execute();
            SkillId = -1;

            _curSkill = skill;
            return true;
        }

        public bool IsFinish()
        {
            if (_curSkill == null) return true;
            return _curSkill.IsFinish();
        }

        Skill GetSkill(int skillId)
        {
            if (_caches.TryGetValue(skillId, out Skill ret))
            {

            }
            else
            {
                ret = Skill.Get();
                ret.Init(actor, skillId);
                _caches.Add(skillId, ret);
            }
            return ret;
        }

    }

}