using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(PlayerMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct PlayerShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (input, ammoEquipment, timer, transform) in SystemAPI
                         .Query<PlayerInputData, AmmoEquipment, RefRW<ShootTimer>, RefRO<LocalTransform>>()
                         .WithAll<PlayerTag>()) {
                // timer
                timer.ValueRW.value -= deltaTime;
                if (timer.ValueRO.value > 0f)
                    continue;
                timer.ValueRW.value = timer.ValueRO.interval;

                // spawn request
                var ammoSpawnRequest = ecb.CreateEntity();
                ecb.AddComponent(ammoSpawnRequest, new AmmoSpawnRequest {
                    owner = ActorRole.Player,
                    ammo = ammoEquipment.value,
                    position = transform.ValueRO.Position,
                    direction = input.aimDirection
                });
            }
        }
    }
}