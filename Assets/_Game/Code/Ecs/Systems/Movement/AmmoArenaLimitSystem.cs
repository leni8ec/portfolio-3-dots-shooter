using Game.Ecs._Refactor.Logic;
using Game.Ecs._Refactor.Systems.Combat;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateAfter(typeof(AmmoHitSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoArenaLimitSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<Ammo>();
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();

            foreach (var (transform, entity) in SystemAPI
                         .Query<RefRO<LocalTransform>>()
                         .WithAll<Ammo>().WithEntityAccess()) {

                var isOutsideArena = !Arena2D.IsInside(transform.ValueRO.Position, config.arenaMin2D, config.arenaMax2D);
                if (isOutsideArena)
                    ecb.DestroyEntity(entity);
            }
        }
    }
}