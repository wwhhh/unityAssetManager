using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class IActorController : MonoBehaviour
    {

        protected Actor actor;

        public virtual void Init(Actor actor) { }

        public virtual void Update() { }

        public virtual void Dispose() { }

    }

}