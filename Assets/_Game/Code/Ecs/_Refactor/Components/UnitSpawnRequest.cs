using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public struct UnitSpawnRequest : IComponentData {
        public AssetId UnitId;
        public float3 position;
    }

}