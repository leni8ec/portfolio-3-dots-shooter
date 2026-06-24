using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Bonuses {
    public struct Bomb : IComponentData {
        public byte DamageAmount;
    }
}