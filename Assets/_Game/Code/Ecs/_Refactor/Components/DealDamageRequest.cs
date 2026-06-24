using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    internal struct DealDamageRequest : IComponentData {
        public Entity Target;
        public int Amount;
    }
}