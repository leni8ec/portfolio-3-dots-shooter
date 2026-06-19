using Unity.Entities;

namespace Game.Ecs.Components {
    public struct Ammo : IComponentData {
        public int damage;
        public float speed;
    }
}