using Game.Ecs._Refactor.Components.Ammos;
using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs._Refactor.Components.Identities.Traits;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// todo: split responsibilities (base shoot, extra shoot) ?
namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(PlayerMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct PlayerShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Unit, Faction, PlayerInput, AmmoEquipment, ShootTimer>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var deltaTime = SystemAPI.Time.DeltaTime;

            // extra ammo - not necessarily present on the entity
            var extraShootLookup = SystemAPI.GetComponentLookup<ExtraShootRequest>();
            var extraAmmoLookup = SystemAPI.GetComponentLookup<ExtraAmmoEquipment>();

            foreach (var (faction, input, ammoEquipment, shootTimer, transform, entity) in SystemAPI
                         .Query<Faction, PlayerInput, AmmoEquipment, RefRW<ShootTimer>, RefRO<LocalTransform>>()
                         .WithAll<Unit>().WithEntityAccess()) {

                // shoot timer
                shootTimer.ValueRW.Value -= deltaTime;
                if (shootTimer.ValueRO.Value <= 0f) {
                    shootTimer.ValueRW.Value = shootTimer.ValueRO.Interval;
                    CreatePlayerAmmoSpawnRequest(faction.FactionId, ammoEquipment.AmmoId, transform.ValueRO.Position, input.AimDirection, ref ecb);
                }

                // extra shoot
                if (extraAmmoLookup.HasComponent(entity) && extraShootLookup.IsComponentEnabled(entity)) {
                    extraShootLookup.SetComponentEnabled(entity, false);
                    CreatePlayerAmmoSpawnRequest(faction.FactionId, extraAmmoLookup[entity].AmmoId, transform.ValueRO.Position, input.AimDirection, ref ecb);
                }
            }
        }

        private static void CreatePlayerAmmoSpawnRequest(Identity factionId, Identity ammoId, float3 position, float3 direction, ref EntityCommandBuffer ecb) {
            var request = ecb.CreateEntity();
            ecb.AddComponent(request, new AmmoSpawnRequest {
                OwnerFactionId = factionId,
                AmmoId = ammoId,
                Position = position,
                Direction = direction
            });
        }
    }
}