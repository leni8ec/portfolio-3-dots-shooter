using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Bonuses {
    public struct Heal : IComponentData {
        public byte Amount;
    }
}