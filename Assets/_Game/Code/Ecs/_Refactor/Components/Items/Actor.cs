using Game.Ecs._Refactor.Values;
using Unity.Entities;

namespace Game.Ecs.Components {
    public struct Actor : IComponentData {
        public ActorIdentity identity;
        public ActorRole role;
    }
}