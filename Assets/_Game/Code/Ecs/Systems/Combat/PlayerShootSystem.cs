using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Entities;
using Unity.Mathematics;
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

            var extraShootLookup = SystemAPI.GetComponentLookup<ExtraShootRequest>();

            foreach (var (input, ammoEquipment, shootTimer, transform, entity) in SystemAPI
                         .Query<PlayerInputData, AmmoEquipment, RefRW<ShootTimer>, RefRO<LocalTransform>>()
                         .WithAll<PlayerTag>().WithEntityAccess()) {

                // shoot timer
                shootTimer.ValueRW.value -= deltaTime;
                if (shootTimer.ValueRO.value <= 0f) {
                    shootTimer.ValueRW.value = shootTimer.ValueRO.interval;
                    CreatePlayerAmmoSpawnRequest(ammoEquipment.value, transform.ValueRO.Position, input.aimDirection, ref ecb);
                }

                // extra shoot
                if (extraShootLookup.IsComponentEnabled(entity)) {
                    extraShootLookup.SetComponentEnabled(entity, false);
                    CreatePlayerAmmoSpawnRequest(extraShootLookup[entity].Ammo, transform.ValueRO.Position, input.aimDirection, ref ecb);
                }
            }
        }

        private static void CreatePlayerAmmoSpawnRequest(AmmoIdentity ammo, float3 position, float3 direction, ref EntityCommandBuffer ecb) {
            var request = ecb.CreateEntity();
            ecb.AddComponent(request, new AmmoSpawnRequest {
                ownerFaction = Faction.Player,
                ammo = ammo,
                position = position,
                direction = direction
            });
        }
    }
}