using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

// todo: make universal scheduler for all spawners
namespace Game.Ecs._Refactor.Systems.Spawners {
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct ActorSpawnScheduleSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PrefabCatalog>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<GameRandom>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<ActorSpawnSchedule, TimerElapsed>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();
            var actorCatalog = SystemAPI.GetSingleton<PrefabCatalog>().Actors;
            ref var random = ref SystemAPI.GetSingletonRW<GameRandom>().ValueRW.value;

            foreach (var (schedule, timer, timerElapsedEnabled) in
                     SystemAPI.Query<ActorSpawnSchedule, RefRW<Timer>, EnabledRefRW<TimerElapsed>>()) {

                var prefab = actorCatalog.Get(schedule.ActorId, default);
                var position = LocationPositionProvider.Get(
                    schedule.Location, config.arenaMin2D, config.arenaMax2D,
                    config.outsideArenaSpawnOffset, ref random);

                var instance = ecb.Instantiate(prefab);
                ecb.SetComponent(instance, LocalTransform.FromPosition(position));

                // reset schedule timer
                timer.ValueRW.Value = schedule.Interval;
                timerElapsedEnabled.ValueRW = false;
            }
        }
    }
}