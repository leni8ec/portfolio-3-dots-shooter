using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;

namespace Game.Ecs.Systems.Gameflow {
    [UpdateBefore(typeof(GameplaySystemGroup))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal partial class GameplayGateSystem : SystemBase {
        private GameplaySystemGroup gameplaySystemGroup;

        protected override void OnCreate() {
            RequireForUpdate<GameState>();
        }

        protected override void OnStartRunning() {
            gameplaySystemGroup = World.GetExistingSystemManaged<GameplaySystemGroup>();
        }

        protected override void OnUpdate() {
            GameState gameState = SystemAPI.GetSingleton<GameState>();
            bool enableGameplaySystems = gameState.phase == GamePhase.Playing;

            if (gameplaySystemGroup != null && gameplaySystemGroup.Enabled != enableGameplaySystems)
                gameplaySystemGroup.Enabled = enableGameplaySystems;
        }

    }
}