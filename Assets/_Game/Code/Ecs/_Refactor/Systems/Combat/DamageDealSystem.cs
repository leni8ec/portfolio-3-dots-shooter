using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Stats;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Systems.Combat {
    [UpdateAfter(typeof(AmmoHitSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct DamageDealSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<DealDamageRequest>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            state.Dependency = new DamageApplyJob {
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                HealthLookup = SystemAPI.GetComponentLookup<Health>()
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct DamageApplyJob : IJobEntity {
            public EntityCommandBuffer Ecb;
            public ComponentLookup<Health> HealthLookup;

            private void Execute(Entity requestEntity, in DealDamageRequest request) {
                Ecb.DestroyEntity(requestEntity);

                var targetEntity = request.Target;
                if (!HealthLookup.HasComponent(targetEntity))
                    return;

                var health = HealthLookup[targetEntity];
                health.value = math.max(0, health.value - request.Amount);
                HealthLookup[targetEntity] = health;
            }
        }

    }
}