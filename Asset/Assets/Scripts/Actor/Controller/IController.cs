using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public abstract class IActorController : MonoBehaviour
    {

        protected Actor actor;

        public abstract void Init(Actor actor);

        public abstract void Dispose();

    }

}