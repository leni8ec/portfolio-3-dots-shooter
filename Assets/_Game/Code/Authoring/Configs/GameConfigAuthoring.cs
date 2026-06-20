using Game.Configs.Game;
using Game.Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Authoring.Configs {
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
                    enemy1Prefab = GetEntity(config.enemy1Prefab, TransformUsageFlags.Dynamic),
                    enemy2Prefab = GetEntity(config.enemy2Prefab, TransformUsageFlags.Dynamic),

                    playerBulletPrefab = GetEntity(config.playerBulletPrefab, TransformUsageFlags.Dynamic),
                    playerMissilePrefab = GetEntity(config.playerMissilePrefab, TransformUsageFlags.Dynamic),
                    enemyBulletPrefab = GetEntity(config.enemyBulletPrefab, TransformUsageFlags.Dynamic),
                    enemyMissilePrefab = GetEntity(config.enemyMissilePrefab, TransformUsageFlags.Dynamic),

                    arenaMin2D = new float2(config.arenaMin.x, config.arenaMin.y),
                    arenaMax2D = new float2(config.arenaMax.x, config.arenaMax.y),
                    arenaCenter2D = new float2(
                        (config.arenaMax.x + config.arenaMin.x) / 2,
                        (config.arenaMax.y + config.arenaMin.y) / 2),

                    outsideArenaSpawnOffset = config.outsideArenaSpawnOffset,

                    enemyTouchDamage = config.enemyTouchDamage,

                    ammoHitDistanceSq = config.ammoHitDistance * config.ammoHitDistance,
                    enemyTouchDistanceSq = config.enemyTouchDistance * config.enemyTouchDistance
                });
                AddComponent(entity, new GameRandom {
                    value = Random.CreateFromIndex(12345)
                });
                AddComponent(entity, new GameState {
                    phase = GamePhase.Playing
                });
            }
        }
    }
}