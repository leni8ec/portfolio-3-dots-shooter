using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct PlayerTag : IComponentData { }

    public struct EnemyTag : IComponentData { }

    public struct Health : IComponentData {
        public int value;
    }

    public struct MoveSpeed : IComponentData {
        public float value;
    }

    public struct ShootTimer : IComponentData {
        public float value;
        public float interval;
    }

    public struct PlayerInputData : IComponentData {
        public float2 move;
        public float3 aimDirection;
    }

    public struct EnemyState : IComponentData {
        public bool isInsideArena;
    }

    public enum BulletOwner : byte {
        Player = 0,
        Enemy = 1
    }

    public struct BulletData : IComponentData {
        public BulletOwner owner;
        public float3 direction;
        public float speed;
        public int damage;
    }

    public struct EnemySpawnTimer : IComponentData {
        public float value;
    }

    public struct RandomState : IComponentData {
        public Random value;
    }
}