using Game.Configs.Factions;
using Game.Configs.Units;
using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class UnitSpawnerAuthoring : MonoBehaviour {
        public ControlType ControlType;
        public UnitAsset Unit;
        public FactionAsset Faction;

        public sealed class Baker : Baker<UnitSpawnerAuthoring> {
            public override void Bake(UnitSpawnerAuthoring authoring) {
                var request = GetEntity(TransformUsageFlags.None);
                AddComponent(request, new UnitSpawnRequest {
                    ControlType = authoring.ControlType,
                    UnitId = authoring.Unit,
                    FactionId = authoring.Faction,
                    Position = authoring.transform.position
                });
            }
        }
    }
}