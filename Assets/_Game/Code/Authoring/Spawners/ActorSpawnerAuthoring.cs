using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class ActorSpawnerAuthoring : MonoBehaviour {
        public ActorIdentity actor;

        public sealed class Baker : Baker<ActorSpawnerAuthoring> {
            public override void Bake(ActorSpawnerAuthoring authoring) {
                var request = GetEntity(TransformUsageFlags.None);
                AddComponent(request, new ActorSpawnRequest {
                    actor = authoring.actor,
                    position = authoring.transform.position
                });
            }
        }
    }
}