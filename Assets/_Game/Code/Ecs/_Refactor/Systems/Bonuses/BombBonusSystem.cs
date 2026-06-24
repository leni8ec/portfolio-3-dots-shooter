using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Bonuses;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.Ecs._Refactor.Systems.Bonuses {
    [UpdateAfter(typeof(PickupBonusSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct BombBonusSystem : ISystem {
        private EntityQuery targetsQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Bonus, Bomb, Active>().Build());

            targetsQuery = SystemAPI.QueryBuilder().WithAll<Unit, BotControlTag>().Build();

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var targetEntities = targetsQuery.ToEntityArray(Allocator.Temp);

            foreach (var (bomb, bonusActivatedEnabled, bombEntity) in SystemAPI
                         .Query<Bomb, EnabledRefRW<Active>>().WithEntityAccess()
                         .WithAll<Bonus>()) {
                ecb.DestroyEntity(bombEntity);
                var damageAmount = bomb.DamageAmount;

                foreach (var targetEntity in targetEntities) {
                    var requestEntity = ecb.CreateEntity();
                    ecb.AddComponent(requestEntity, new DealDamageRequest {
                        Target = targetEntity,
                        Amount = damageAmount
                    });
                }
            }
        }
    }
}