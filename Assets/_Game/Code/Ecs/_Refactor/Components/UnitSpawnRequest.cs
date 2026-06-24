using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public struct UnitSpawnRequest : IComponentData {
        public Identity UnitId;
        public Identity FactionId;
        public float3 Position;
    }
}