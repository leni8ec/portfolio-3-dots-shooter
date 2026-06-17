using Game.Ecs._Refactor.Components.Enemies;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyMoveToTargetSystem : ISystem {
        private EntityQuery playersQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<EnemyTag, EnemyTarget>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;

            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);

            foreach (var (transform, speed, target, targetEnabled) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<EnemyTarget>, EnabledRefRW<EnemyTarget>>()
                         .WithAll<EnemyTag>()) {

                // reset target if target no longer exists
                var targetEntity = target.ValueRO.entity;
                if (targetEntity == Entity.Null || !transformLookup.HasComponent(targetEntity)) {
                    target.ValueRW.entity = default;
                    targetEnabled.ValueRW = false;
                    continue;
                }

                // move to target
                var fromPosition = transform.ValueRO.Position;
                var targetPosition = transformLookup[targetEntity].Position;
                transform.ValueRW.Position = Position3D.MoveTowards2D(
                    fromPosition, targetPosition, speed.ValueRO.value, deltaTime);
            }

        }


    }
}