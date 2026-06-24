using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Ammos {
    public struct ShootTimer : IComponentData {
        public float Value;
        public float Interval;
    }
}