using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Controls {
    public struct BotTarget : IComponentData, IEnableableComponent {
        public Entity entity;
    }
}