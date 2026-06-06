using Game.Ecs.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    internal partial struct BulletArenaLimitSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<BulletData>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform, entity) in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<BulletData>()
                         .WithEntityAccess()) {

                float x = transform.ValueRO.Position.x;
                float z = transform.ValueRO.Position.z;

                bool isOutsideArena =
                    x < config.arenaMin.x ||
                    x > config.arenaMax.x ||
                    z < config.arenaMin.y ||
                    z > config.arenaMax.y;

                if (isOutsideArena)
                    ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}