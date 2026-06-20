using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class UnitSpawnerAuthoring : MonoBehaviour {
        public UnitIdentity Unit;

        public sealed class Baker : Baker<UnitSpawnerAuthoring> {
            public override void Bake(UnitSpawnerAuthoring authoring) {
                var request = GetEntity(TransformUsageFlags.None);
                AddComponent(request, new UnitSpawnRequest {
                    Unit = authoring.Unit,
                    position = authoring.transform.position
                });
            }
        }
    }
}