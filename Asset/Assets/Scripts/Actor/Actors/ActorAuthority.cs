using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class ActorAuthority : Actor
    {

        protected override void InitState()
        {
            stateController = gameObject.AddComponent<StateControllerAuthority>();
            stateController.Init(this);
        }

        public override void PostInit()
        {
        }

    }

}