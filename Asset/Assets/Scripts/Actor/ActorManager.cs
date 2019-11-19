using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorCore
{
    public class ActorManager : Singleton<ActorManager>
    {

        private long id;
        private Dictionary<long, Actor> dicActors = new Dictionary<long, Actor>();

        public void Clear()
        {

        }

        public void RemoveActor(long id)
        {
            if (dicActors.ContainsKey(id))
            {
                Actor actor = dicActors[id];
                actor.Dispose();
                dicActors.Remove(id);
            }
        }

        public ActorAuthority AddActorAuthority()
        {
            GameObject go = new GameObject();
#if UNITY_EDITOR
            go.name = "ActorAuthority";
#endif
            ActorAuthority actor = go.AddComponent<ActorAuthority>();
            actor.actorName = "test";
            actor.Init();
            actor.id = id++;
            dicActors[actor.id] = actor;
            return actor;
        }

        public ActorEnemy AddActorEnemy()
        {
            GameObject go = new GameObject();
#if UNITY_EDITOR
            go.name = "ActorEnemy";
#endif
            ActorEnemy enemy = go.AddComponent<ActorEnemy>();
            enemy.actorName = "test";
            enemy.Init();
            enemy.id = id++;
            dicActors[enemy.id] = enemy;
            return enemy;
        }

    }
}