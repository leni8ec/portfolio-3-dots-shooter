using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Bonuses {
    public struct Wipe : IComponentData {
        public byte DamageAmount;
    }
}