using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs._Refactor.Systems.Bonuses {
    [UpdateAfter(typeof(PlayerMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct BonusPickupSystem : ISystem {
        private EntityQuery unitsQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            unitsQuery = SystemAPI.QueryBuilder().WithAll<Unit, LocalTransform>().Build();

            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate(unitsQuery);
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Bonus>().WithDisabled<Active>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var pickupDistanceSq = SystemAPI.GetSingleton<GameConfig>().unitTouchDistanceSq;
            var pickupDistance = math.sqrt(pickupDistanceSq);

            var playerLookup = SystemAPI.GetComponentLookup<PlayerControlTag>();
            using var unitTransforms = unitsQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            using var unitEntities = unitsQuery.ToEntityArray(Allocator.Temp);

            foreach (var (bonus, active, bonusTransform) in SystemAPI
                         .Query<RefRW<Bonus>, EnabledRefRW<Active>, RefRO<LocalTransform>>()
                         .WithDisabled<Active>().WithAll<Bonus>()) {

                for (var i = 0; i < unitTransforms.Length; i++) {
                    if (bonus.ValueRO.ForPlayerOnly) {
                        if (!playerLookup.HasComponent(unitEntities[i]))
                            continue;
                    }

                    var playerTransform = unitTransforms[i];
                    // XZ axis only
                    var delta = playerTransform.Position.xz - bonusTransform.ValueRO.Position.xz;

                    // Cheap reject before squared distance.
                    if (math.abs(delta.x) > pickupDistance ||
                        math.abs(delta.y) > pickupDistance)
                        continue;

                    if (math.lengthsq(delta) > pickupDistanceSq)
                        continue;

                    bonus.ValueRW.OwnerEntity = unitEntities[i];
                    active.ValueRW = true;

                    break;
                }
            }
        }

    }
}