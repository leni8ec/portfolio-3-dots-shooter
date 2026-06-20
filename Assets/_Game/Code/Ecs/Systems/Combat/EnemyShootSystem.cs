using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Enemies;
using Game.Ecs._Refactor.Logic;
using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(EnemyMoveToTargetSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<EnemyTag, EnemyTarget>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            var config = SystemAPI.GetSingleton<GameConfig>();
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (enemyTarget, ammoEquipment, timer, transform) in SystemAPI
                         .Query<EnemyTarget, AmmoEquipment, RefRW<ShootTimer>, RefRO<LocalTransform>>()
                         .WithAll<EnemyTag>()) {
                // timer
                timer.ValueRW.value -= deltaTime;
                if (timer.ValueRO.value > 0f)
                    continue;
                timer.ValueRW.value = timer.ValueRO.interval;

                // spawn request
                var fromPosition = transform.ValueRO.Position;
                var targetPosition = transformLookup[enemyTarget.entity].Position;
                var direction = Rotation3D.GetDirection2D(fromPosition, targetPosition);

                var ammoSpawnRequest = ecb.CreateEntity();
                ecb.AddComponent(ammoSpawnRequest, new AmmoSpawnRequest {
                    ownerFaction = Faction.Enemy,
                    ammo = ammoEquipment.value,
                    position = transform.ValueRO.Position,
                    direction = direction
                });
            }
        }
    }
}