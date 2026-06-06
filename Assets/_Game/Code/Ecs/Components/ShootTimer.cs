using Unity.Entities;

namespace Game.Ecs.Components {
    public struct ShootTimer : IComponentData {
        public float value;
        public float interval;
    }
}