using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Combat;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateAfter(typeof(BulletHitSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
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

                bool isOutsideArena = !Arena2D.IsInside(transform.ValueRO.Position,config.arenaMin2D, config.arenaMax2D);
                if (isOutsideArena)
                    ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}