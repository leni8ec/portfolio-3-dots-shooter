using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;

namespace Game.Ecs.Systems.Gameflow {
    [UpdateBefore(typeof(GameplaySystemGroup))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal partial class GameStartCleanupSystem : SystemBase {

        protected override void OnCreate() {
            RequireForUpdate<GameConfig>();
            RequireForUpdate<GameState>();
        }

        protected override void OnUpdate() {
            CleanupRuntimeEntities();
            Enabled = false;
        }

        private void CleanupRuntimeEntities() {
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<EnemyTag>(), ComponentType.Exclude<Prefab>()));
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<BulletData>(), ComponentType.Exclude<Prefab>()));
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(), ComponentType.Exclude<Prefab>()));
        }
    }
}