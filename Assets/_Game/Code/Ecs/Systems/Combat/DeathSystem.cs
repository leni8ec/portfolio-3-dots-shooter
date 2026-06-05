using Game.Ecs.Components;
using Unity.Collections;
using Unity.Entities;

namespace Game.Ecs.Systems.Combat {
    public partial struct DeathSystem : ISystem {

        public void OnCreate(ref SystemState state) {
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
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}