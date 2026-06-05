using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct GameConfig : IComponentData {
        public Entity playerPrefab;
        public Entity enemyPrefab;
        public Entity playerBulletPrefab;
        public Entity enemyBulletPrefab;

        public float2 arenaMin;
        public float2 arenaMax;

        public float enemySpawnInterval;
        public float enemySpawnOffset;

        public float playerBulletSpeed;
        public float enemyBulletSpeed;

        public int playerBulletDamage;
        public int enemyBulletDamage;
        public int enemyTouchDamage;

        public float bulletHitDistanceSq;
        public float enemyTouchDistanceSq;
    }
}