using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class Actor : Entity
    {

        public string actorName;

        AvatarController _avatarController;
        AnimationController _animationController;
        PhysicsController _physicalController;
        RotationController _rotationController;

        public void Init()
        {
            InitAvatar();
            InitAnimation();
            InitPhysics();
            InitRotation();
        }

        public void PostInit()
        {

        }

        public void Dispose()
        {

        }

        private void InitAvatar()
        {
            _avatarController = gameObject.AddComponent<AvatarController>();
            _avatarController.Init(this);
        }

        private void InitAnimation()
        {
            _animationController = gameObject.AddComponent<AnimationController>();
            _animationController.Init(this);
        }

        private void InitPhysics()
        {
            _physicalController = gameObject.AddComponent<PhysicsController>();
            _physicalController.Init(this);
        }

        private void InitRotation()
        {
            _rotationController = gameObject.AddComponent<RotationController>();
            _rotationController.Init(this);
        }

    }

}