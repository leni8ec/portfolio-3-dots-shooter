using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct BulletData : IComponentData {
        public BulletOwner owner;
        public float3 direction;
        public float speed;
        public int damage;
    }
}