using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class ActorEnemy : Actor
    {
        protected override void InitState()
        {
            stateController = gameObject.AddComponent<StateControllerEnemy>();
            stateController.Init(this);
        }

        public override void PostInit()
        {
        }

    }

}