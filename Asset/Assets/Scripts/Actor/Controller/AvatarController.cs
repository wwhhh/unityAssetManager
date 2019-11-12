using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class AvatarController : IActorController
    {

        public override void Dispose()
        {
        }

        public override void Init(Actor actor)
        {
            this.actor = actor;
            LoadAvatar();
        }

        private void LoadAvatar()
        {
            var asset = AssetManager.LoadAsset<GameObject>("assets/game/character/test/"+actor.actorName+".prefab");
            Instantiate(asset.asset);
            asset.Unload();
        }

    }
}