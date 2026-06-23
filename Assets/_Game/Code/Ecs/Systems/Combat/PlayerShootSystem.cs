using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Units;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(PlayerMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct PlayerShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Unit, PlayerInput, AmmoEquipment, ShootTimer>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var deltaTime = SystemAPI.Time.DeltaTime;

            var extraShootLookup = SystemAPI.GetComponentLookup<ExtraShootRequest>();

            foreach (var (unit, input, ammoEquipment, shootTimer, transform, entity) in SystemAPI
                         .Query<Unit, PlayerInput, AmmoEquipment, RefRW<ShootTimer>, RefRO<LocalTransform>>().WithEntityAccess()) {

                // shoot timer
                shootTimer.ValueRW.value -= deltaTime;
                if (shootTimer.ValueRO.value <= 0f) {
                    shootTimer.ValueRW.value = shootTimer.ValueRO.interval;
                    CreatePlayerAmmoSpawnRequest(unit.factionId, ammoEquipment.AmmoId, transform.ValueRO.Position, input.AimDirection, ref ecb);
                }

                // extra shoot
                if (extraShootLookup.IsComponentEnabled(entity)) {
                    extraShootLookup.SetComponentEnabled(entity, false);
                    CreatePlayerAmmoSpawnRequest(unit.factionId, extraShootLookup[entity].AmmoId, transform.ValueRO.Position, input.AimDirection, ref ecb);
                }
            }
        }

        private static void CreatePlayerAmmoSpawnRequest(AssetId factionId, AssetId ammoId, float3 position, float3 direction, ref EntityCommandBuffer ecb) {
            var request = ecb.CreateEntity();
            ecb.AddComponent(request, new AmmoSpawnRequest {
                ownerFactionId = factionId,
                ammoId = ammoId,
                position = position,
                direction = direction
            });
        }
    }
}