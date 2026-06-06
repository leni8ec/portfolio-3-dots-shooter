using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Collections;
using Unity.Entities;

namespace Game.Ecs.Systems.Combat {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct DeathSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameState>();
            state.RequireForUpdate<Health>();
        }

        public void OnUpdate(ref SystemState state) {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (health, entity) in
                     SystemAPI.Query<RefRO<Health>>()
                         .WithEntityAccess()) {

                if (health.ValueRO.value > 0)
                    continue;

                ecb.DestroyEntity(entity);

                if (SystemAPI.HasComponent<PlayerTag>(entity))
                    SystemAPI.GetSingletonRW<GameState>().ValueRW.isGameOver = true;
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}