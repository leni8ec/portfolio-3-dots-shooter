using Game.Ecs.Components;
using Game.Ecs.Systems.Bootstrap;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Authoring {
    public sealed class GameConfigAuthoring : MonoBehaviour {
        [Header("Prefabs")]
        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public GameObject playerBulletPrefab;
        public GameObject enemyBulletPrefab;

        [Header("Arena")]
        public Vector2 arenaMin = new(-14f, -14f);
        public Vector2 arenaMax = new(14f, 14f);
        public float enemySpawnOffset = 2f;

        [Header("Enemy Spawn")]
        public float enemySpawnInterval = 2f;

        [Header("Bullets")]
        public float playerBulletSpeed = 12f;
        public float enemyBulletSpeed = 8f;
        public int playerBulletDamage = 1;
        public int enemyBulletDamage = 1;

        [Header("Contact")]
        public int enemyTouchDamage = 2;
        public float bulletHitDistance = 0.6f;
        public float enemyTouchDistance = 0.8f;

        private sealed class Baker : Baker<GameConfigAuthoring> {
            public override void Bake(GameConfigAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new GameConfig {
                    playerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
                    enemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                    playerBulletPrefab = GetEntity(authoring.playerBulletPrefab, TransformUsageFlags.Dynamic),
                    enemyBulletPrefab = GetEntity(authoring.enemyBulletPrefab, TransformUsageFlags.Dynamic),

                    arenaMin = new float2(authoring.arenaMin.x, authoring.arenaMin.y),
                    arenaMax = new float2(authoring.arenaMax.x, authoring.arenaMax.y),

                    enemySpawnInterval = authoring.enemySpawnInterval,
                    enemySpawnOffset = authoring.enemySpawnOffset,

                    playerBulletSpeed = authoring.playerBulletSpeed,
                    enemyBulletSpeed = authoring.enemyBulletSpeed,

                    playerBulletDamage = authoring.playerBulletDamage,
                    enemyBulletDamage = authoring.enemyBulletDamage,
                    enemyTouchDamage = authoring.enemyTouchDamage,

                    bulletHitDistanceSq = authoring.bulletHitDistance * authoring.bulletHitDistance,
                    enemyTouchDistanceSq = authoring.enemyTouchDistance * authoring.enemyTouchDistance
                });
                AddComponent(entity, new EnemySpawnTimer {
                    value = authoring.enemySpawnInterval
                });
                AddComponent(entity, new RandomState {
                    value = Random.CreateFromIndex(12345)
                });
                AddComponent(entity, new GameState {
                    isGameOver = false,
                });
            }
        }
    }
}