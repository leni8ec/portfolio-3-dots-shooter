using Game.Configs.Units;
using Game.Ecs._Refactor.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class UnitSpawnerAuthoring : MonoBehaviour {
        public UnitAsset Unit;

        public sealed class Baker : Baker<UnitSpawnerAuthoring> {
            public override void Bake(UnitSpawnerAuthoring authoring) {
                var request = GetEntity(TransformUsageFlags.None);
                AddComponent(request, new UnitSpawnRequest {
                    UnitId = authoring.Unit,
                    position = authoring.transform.position
                });
            }
        }
    }
}