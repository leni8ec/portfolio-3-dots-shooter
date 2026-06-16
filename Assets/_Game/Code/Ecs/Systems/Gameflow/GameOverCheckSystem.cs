using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;

namespace Game.Ecs.Systems.Gameflow {
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup), OrderLast = true)]
    internal partial struct GameOverCheckSystem : ISystem, ISystemStartStop {
        private EntityQuery playersQuery;
        private bool hasPlayers; // hook

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameState>();
            playersQuery = SystemAPI.QueryBuilder().WithAll<PlayerTag>().Build();
        }

        [BurstCompile]
        public void OnStartRunning(ref SystemState state) {
            hasPlayers = false;
        }

        [BurstCompile]
        public void OnStopRunning(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            if (!playersQuery.IsEmpty) {
                hasPlayers = true;
                return;
            }
            if (!hasPlayers)
                return;

            // update game state
            var gameState = SystemAPI.GetSingletonRW<GameState>();
            if (gameState.ValueRO.phase == GamePhase.Playing)
                gameState.ValueRW.phase = GamePhase.GameOver;

            hasPlayers = false;
        }

    }
}