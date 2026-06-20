using Game.Ecs._Refactor.Values;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public struct UnitSpawnRequest : IComponentData {
        public UnitIdentity Unit;
        public float3 position;
    }

}