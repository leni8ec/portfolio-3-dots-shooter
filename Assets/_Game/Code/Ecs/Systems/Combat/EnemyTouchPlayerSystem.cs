using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyTouchPlayerSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<EnemyTag>();
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();
            var players = new NativeList<PlayerTouchData>(Allocator.Temp);

            // collect players
            foreach (var (playerTransform, playerEntity) in SystemAPI
                         .Query<RefRO<LocalTransform>>()
                         .WithAll<PlayerTag>().WithEntityAccess()) {
                players.Add(new PlayerTouchData(playerEntity, playerTransform.ValueRO.Position));
            }

            // players x enemies
            var healthLookup = SystemAPI.GetComponentLookup<Health>();
            foreach (var (enemyTransform, enemyEntity) in SystemAPI
                         .Query<RefRO<LocalTransform>>()
                         .WithAll<EnemyTag>().WithEntityAccess()) {
                var enemyPosition = enemyTransform.ValueRO.Position;

                for (var i = 0; i < players.Length; i++) {
                    var player = players[i];

                    var distanceSq = math.distancesq(player.position, enemyPosition);
                    if (distanceSq > config.enemyTouchDistanceSq)
                        continue;

                    var health = healthLookup[player.entity];
                    health.value -= config.enemyTouchDamage;
                    healthLookup[player.entity] = health;

                    ecb.DestroyEntity(enemyEntity);
                }
            }

            players.Dispose();
        }

        private readonly struct PlayerTouchData {
            public readonly Entity entity;
            public readonly float3 position;

            public PlayerTouchData(Entity entity, float3 position) {
                this.entity = entity;
                this.position = position;
            }
        }

    }
}