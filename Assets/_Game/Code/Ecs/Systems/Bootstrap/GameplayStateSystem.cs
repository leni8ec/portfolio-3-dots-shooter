using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;

namespace Game.Ecs.Systems.Bootstrap {
    [UpdateBefore(typeof(GameplaySystemGroup))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal partial class GameplayStateSystem : SystemBase {
        private GameplaySystemGroup gameplaySystemGroup;

        protected override void OnCreate() {
            RequireForUpdate<GameState>();
        }

        protected override void OnStartRunning() {
            gameplaySystemGroup = World.GetExistingSystemManaged<GameplaySystemGroup>();
            CleanupRuntimeEntities();
        }

        protected override void OnUpdate() {
            GameState gameState = SystemAPI.GetSingleton<GameState>();
            bool enableGameplaySystems = !gameState.isGameOver && !gameState.isPaused;

            if (gameplaySystemGroup != null && gameplaySystemGroup.Enabled != enableGameplaySystems)
                gameplaySystemGroup.Enabled = enableGameplaySystems;
        }

        private void CleanupRuntimeEntities() {
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<EnemyTag>()));
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<BulletData>()));
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<PlayerTag>()));
        }
    }
}