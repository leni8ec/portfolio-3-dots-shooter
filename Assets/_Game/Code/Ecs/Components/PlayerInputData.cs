using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct PlayerInputData : IComponentData {
        public float2 move;
        public float3 aimDirection;
    }
}