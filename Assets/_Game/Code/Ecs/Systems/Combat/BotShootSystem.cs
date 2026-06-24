using Game.Ecs._Refactor.Components.Ammos;
using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs._Refactor.Components.Identities.Traits;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(BotMoveToTargetSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct BotShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Unit, BotControlTag, Faction, BotTarget, AmmoEquipment, ShootTimer>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            var config = SystemAPI.GetSingleton<GameConfig>();
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (faction, botTarget, ammoEquipment, timer, transform) in SystemAPI
                         .Query<Faction, BotTarget, AmmoEquipment, RefRW<ShootTimer>, RefRO<LocalTransform>>()
                         .WithAll<Unit, BotControlTag>()) {
                // timer
                timer.ValueRW.Value -= deltaTime;
                if (timer.ValueRO.Value > 0f)
                    continue;
                timer.ValueRW.Value = timer.ValueRO.Interval;

                // spawn request
                var fromPosition = transform.ValueRO.Position;
                var targetPosition = transformLookup[botTarget.Entity].Position;
                var direction = Rotation3D.GetDirection2D(fromPosition, targetPosition);

                var ammoSpawnRequest = ecb.CreateEntity();
                ecb.AddComponent(ammoSpawnRequest, new AmmoSpawnRequest {
                    OwnerFactionId = faction.FactionId,
                    AmmoId = ammoEquipment.AmmoId,
                    Position = transform.ValueRO.Position,
                    Direction = direction
                });
            }
        }
    }
}