using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    internal struct ApplyDamageRequest : IComponentData {
        public Entity Target;
        public int Value;
    }
}