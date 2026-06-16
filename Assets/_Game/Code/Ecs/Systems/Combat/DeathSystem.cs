using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(BulletHitSystem))]
    [UpdateAfter(typeof(EnemyTouchPlayerSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct DeathSystem : ISystem {
        private EntityQuery playersQuery;

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameState>();
            state.RequireForUpdate<Health>();

            playersQuery = SystemAPI.QueryBuilder().WithAll<PlayerTag>().Build();
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (health, entity) in
                     SystemAPI.Query<RefRO<Health>>()
                         .WithEntityAccess()) {

                if (health.ValueRO.value > 0)
                    continue;

                ecb.DestroyEntity(entity);

                if (SystemAPI.HasComponent<PlayerTag>(entity)) {
                    if (playersQuery.CalculateEntityCount() <= 1)
                        SystemAPI.GetSingletonRW<GameState>().ValueRW.phase = GamePhase.GameOver;
                }
            }
        }
    }
}