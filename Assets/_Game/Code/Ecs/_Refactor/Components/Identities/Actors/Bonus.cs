using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Identities.Actors {
    public struct Bonus : IComponentData {
        public Entity OwnerEntity;
    }
}