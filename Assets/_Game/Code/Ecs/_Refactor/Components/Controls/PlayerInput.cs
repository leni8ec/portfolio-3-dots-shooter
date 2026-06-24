using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components.Controls {
    public struct PlayerInput : IComponentData {
        public float2 Move;
        public float3 AimDirection;
    }
}