using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;

// todo: purpose of this system?
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
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<Unit>(), ComponentType.Exclude<Prefab>()));
            EntityManager.DestroyEntity(GetEntityQuery(ComponentType.ReadOnly<Ammo>(), ComponentType.Exclude<Prefab>()));
        }
    }
}