using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs._Refactor.Systems.Actors.Spawn {
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct ActorSpawnScheduleSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<GameRandom>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<ActorSpawnSchedule, TimerElapsed>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<GameConfig>();
            ref var random = ref SystemAPI.GetSingletonRW<GameRandom>().ValueRW.value;
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (schedule, timer, timerElapsedEnabled) in
                     SystemAPI.Query<ActorSpawnSchedule, RefRW<Timer>, EnabledRefRW<TimerElapsed>>()) {

                // create spawn request
                var entity = ecb.CreateEntity();
                ecb.AddComponent(entity, new ActorSpawnRequest {
                    actor = schedule.actor,
                    position = SpawnPositionProvider.Get(
                        schedule.location, config.arenaMin2D, config.arenaMax2D,
                        config.outsideArenaSpawnOffset, ref random),
                });

                // reset schedule timer
                timer.ValueRW.value = schedule.interval;
                timerElapsedEnabled.ValueRW = false;
            }
        }
    }
}