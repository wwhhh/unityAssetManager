using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class PhysicsController : IActorController
    {
        private CharacterController _character;
        private bool _moving;
        private Vector3 _dir;

        const float SPEED = 5f;

        public override void Init(Actor actor)
        {
            _character = gameObject.AddComponent<CharacterController>();
            _character.height = 2;
            _character.radius = 0.5f;
            _character.center = new Vector3(0, 1, 0);

            this.actor = actor;
        }

        public override void Dispose()
        {
        }

        private void Update()
        {
            if (_moving) _character.Move(_dir * SPEED);
        }

        public void MoveDelta(Vector3 dir)
        {
            _moving = true;
            _dir = dir;
        }

        public void Stop()
        {
            _moving = false;
        }

    }
}