using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs._Refactor.Systems.Spawners {
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct UnitSpawnScheduleSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<GameRandom>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<UnitSpawnSchedule, TimerElapsed>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<GameConfig>();
            ref var random = ref SystemAPI.GetSingletonRW<GameRandom>().ValueRW.value;
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (schedule, timer, timerElapsedEnabled) in
                     SystemAPI.Query<UnitSpawnSchedule, RefRW<Timer>, EnabledRefRW<TimerElapsed>>()) {

                // create spawn request
                var entity = ecb.CreateEntity();
                ecb.AddComponent(entity, new UnitSpawnRequest {
                    ControlType = schedule.ControlType,
                    UnitId = schedule.UnitId,
                    FactionId = schedule.FactionId,
                    Position = LocationPositionProvider.Get(
                        schedule.Location, config.arenaMin2D, config.arenaMax2D,
                        config.outsideArenaSpawnOffset, ref random),
                });

                // reset schedule timer
                timer.ValueRW.Value = schedule.Interval;
                timerElapsedEnabled.ValueRW = false;
            }
        }
    }
}