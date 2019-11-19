using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class PhysicsController : IActorController
    {
        public float SPEED = 6f;

        public override void Init(Actor actor)
        {
            this.actor = actor;
        }

        public override void Dispose()
        {
        }

        public void MoveDelta(Vector3 delta)
        {
            transform.localPosition += delta;
        }

        public void MoveDelta(float speed, Vector3 directionXZ)
        {
            Vector3 delta = (directionXZ * speed) * Time.deltaTime;
            transform.localPosition += delta;
        }

    }
}