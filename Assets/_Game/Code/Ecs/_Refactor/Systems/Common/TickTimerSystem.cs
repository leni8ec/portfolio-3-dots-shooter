using Game.Ecs._Refactor.Components.Common;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs._Refactor.Systems.Common {
    internal partial struct TickTimerSystem : ISystem {
        private EntityQuery query;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            query = new EntityQueryBuilder(state.WorldUpdateAllocator)
                .WithAll<Timer>()
                .WithNone<TimerElapsed>()
                .Build(ref state);

            state.RequireForUpdate(query);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;

            var job = new TickTimerJob { deltaTime = deltaTime };
            state.Dependency = job.ScheduleParallel(query, state.Dependency);
        }

        [BurstCompile]
        public partial struct TickTimerJob : IJobEntity {
            public float deltaTime;

            public void Execute(ref Timer timer, EnabledRefRW<TimerElapsed> timerElapsedEnabled) {
                timer.value -= deltaTime;

                if (timer.value <= 0)
                    timerElapsedEnabled.ValueRW = true;
            }
        }
    }
}