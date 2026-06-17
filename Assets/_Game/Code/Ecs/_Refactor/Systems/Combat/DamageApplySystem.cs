using Game.Ecs._Refactor.Components;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs._Refactor.Systems.Combat {
    [UpdateAfter(typeof(AmmoHitSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial struct DamageApplySystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<ApplyDamageRequest>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            state.Dependency = new DamageApplyJob {
                HealthLookup = SystemAPI.GetComponentLookup<Health>(),
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct DamageApplyJob : IJobEntity {
            public ComponentLookup<Health> HealthLookup;
            public EntityCommandBuffer Ecb;

            private void Execute(Entity requestEntity, in ApplyDamageRequest request) {
                var targetEntity = request.Target;
                var health = HealthLookup[targetEntity];
                health.value -= request.Value;
                HealthLookup[targetEntity] = health;

                Ecb.DestroyEntity(requestEntity);
            }
        }

    }
}