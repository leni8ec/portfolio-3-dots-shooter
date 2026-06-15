using Game.Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Authoring {
    public sealed class GameConfigAuthoring : MonoBehaviour {
        public GameConfigAsset config;

        private sealed class Baker : Baker<GameConfigAuthoring> {
            public override void Bake(GameConfigAuthoring authoring) {
                GameConfigAsset config = authoring.config;
                if (!config) {
                    Debug.LogError("GameConfigAuthoring requires GameConfigAsset.", authoring);
                    return;
                }

                DependsOn(config);
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new GameConfig {
                    playerPrefab = GetEntity(config.playerPrefab, TransformUsageFlags.Dynamic),
                    enemyPrefab = GetEntity(config.enemyPrefab, TransformUsageFlags.Dynamic),
                    playerBulletPrefab = GetEntity(config.playerBulletPrefab, TransformUsageFlags.Dynamic),
                    enemyBulletPrefab = GetEntity(config.enemyBulletPrefab, TransformUsageFlags.Dynamic),

                    arenaMin = new float2(config.arenaMin.x, config.arenaMin.y),
                    arenaMax = new float2(config.arenaMax.x, config.arenaMax.y),

                    outsideArenaSpawnOffset = config.outsideArenaSpawnOffset,

                    playerBulletSpeed = config.playerBulletSpeed,
                    enemyBulletSpeed = config.enemyBulletSpeed,

                    playerBulletDamage = config.playerBulletDamage,
                    enemyBulletDamage = config.enemyBulletDamage,
                    enemyTouchDamage = config.enemyTouchDamage,

                    bulletHitDistanceSq = config.bulletHitDistance * config.bulletHitDistance,
                    enemyTouchDistanceSq = config.enemyTouchDistance * config.enemyTouchDistance
                });
                AddComponent(entity, new RandomState {
                    value = Random.CreateFromIndex(12345)
                });
                AddComponent(entity, new GameState {
                    phase = GamePhase.Playing
                });
            }
        }
    }
}