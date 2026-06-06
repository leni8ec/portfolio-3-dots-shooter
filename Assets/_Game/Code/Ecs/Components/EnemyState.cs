using Unity.Entities;

namespace Game.Ecs.Components {
    public struct EnemyState : IComponentData {
        public bool isInsideArena;
    }
}