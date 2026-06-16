using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    // [UpdateAfter(typeof(EnemyEnterArenaSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyTouchPlayerSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<EnemyTag>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<GameConfig>();
            var players = new NativeList<PlayerTouchData>(Allocator.Temp);
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            // collect players
            foreach (var (playerTransform, playerEntity) in
                     SystemAPI.Query<LocalTransform>()
                         .WithAll<PlayerTag>().WithEntityAccess()) {
                players.Add(new PlayerTouchData(playerEntity, playerTransform.Position));
            }

            // players x enemies
            var healthLookup = SystemAPI.GetComponentLookup<Health>();
            foreach (var (enemyTransform, enemyEntity) in
                     SystemAPI.Query<LocalTransform>()
                         .WithAll<EnemyTag>().WithEntityAccess()) {
                var enemyPosition = enemyTransform.Position;

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