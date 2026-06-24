using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct ShotInfo : IComponentData {
        public float3 Direction;
    }
}