using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class Actor : Entity
    {

        public long id;
        public string actorName;
        public float speed = 6f;

        [System.NonSerialized]
        public AvatarController avatarController;
        [System.NonSerialized]
        public AnimationController animationController;
        [System.NonSerialized]
        public PhysicsController physicsController;
        [System.NonSerialized]
        public RotationController rotationController;
        [System.NonSerialized]
        public StateController stateController;
        [System.NonSerialized]
        public SkillController skillController;

        public void Init()
        {
            InitAvatar();
            InitAnimation();
            InitPhysics();
            InitRotation();
            InitState();
            InitSkill();
        }

        public void OnHit()
        {
            stateController.ChangeStateHit();
        }

        public virtual void PostInit()
        {
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        private void InitAvatar()
        {
            avatarController = gameObject.AddComponent<AvatarController>();
            avatarController.Init(this);
        }

        private void InitAnimation()
        {
            Transform body = avatarController.body;
            if (body == null) return;

            animationController = body.gameObject.AddComponent<AnimationController>();
            animationController.Init(this);
        }

        private void InitPhysics()
        {
            physicsController = gameObject.AddComponent<PhysicsController>();
            physicsController.Init(this);
        }

        private void InitRotation()
        {
            rotationController = gameObject.AddComponent<RotationController>();
            rotationController.Init(this);
        }

        protected virtual void InitState()
        {
            stateController = gameObject.AddComponent<StateController>();
            stateController.Init(this);
        }

        #region 动画

        public void Play(string name, bool hasMove, float blendTime = 0.2f)
        {
            animationController.Play(name, hasMove, blendTime);
        }

        #endregion


        private void InitSkill()
        {
            skillController = gameObject.AddComponent<SkillController>();
            skillController.Init(this);
        }

    }

}