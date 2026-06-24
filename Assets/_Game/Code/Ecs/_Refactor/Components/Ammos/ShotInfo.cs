using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components.Ammos {
    public struct ShotInfo : IComponentData {
        public float3 Direction;
    }
}