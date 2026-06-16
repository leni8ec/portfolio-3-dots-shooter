using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Enemies {
    public struct EnemyTarget : IComponentData, IEnableableComponent {
        public Entity value;
    }
}