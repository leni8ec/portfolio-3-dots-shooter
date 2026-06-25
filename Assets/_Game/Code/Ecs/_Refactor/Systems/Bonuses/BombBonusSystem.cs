using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Bonuses;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs._Refactor.Components.Stats;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs._Refactor.Systems.Bonuses {
    [UpdateAfter(typeof(BonusPickupSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct BombBonusSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Bonus, Bomb, Active>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            var playerHealthLookup = SystemAPI.GetComponentLookup<Health>();

            foreach (var (bonus, bomb, bonusActivatedEnabled, bombEntity) in SystemAPI
                         .Query<Bonus, Bomb, EnabledRefRW<Active>>().WithEntityAccess()) {
                ecb.DestroyEntity(bombEntity);

                var requestEntity = ecb.CreateEntity();
                ecb.AddComponent(requestEntity, new DealDamageRequest {
                    Target = bonus.OwnerEntity,
                    Amount = bomb.DamageAmount
                });
            }
        }
    }
}