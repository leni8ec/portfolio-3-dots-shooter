using Game.Ecs._Refactor.Components.Stats;
using Game.Ecs._Refactor.Systems.Combat;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(DamageDealSystem))]
    [UpdateAfter(typeof(EnemyTouchPlayerSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct DeathSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameState>();
            state.RequireForUpdate<Health>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (health, entity) in
                     SystemAPI.Query<RefRO<Health>>()
                         .WithEntityAccess()) {

                if (health.ValueRO.value > 0)
                    continue;

                ecb.DestroyEntity(entity);
            }
        }
    }
}