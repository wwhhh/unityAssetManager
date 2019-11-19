using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{

    public class AvatarController : IActorController
    {

        public Transform body;

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
            if (asset.asset == null) return;

            GameObject go = Instantiate(asset.asset);
            body = go.transform;
            body.SetParent(transform);
            asset.Unload();

            actor.PostInit();
        }

    }
}