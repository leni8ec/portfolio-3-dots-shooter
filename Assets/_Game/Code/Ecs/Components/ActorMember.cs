using Game.Ecs._Refactor.Values;
using Unity.Entities;

namespace Game.Ecs.Components {
    public struct ActorMember : IComponentData {
        public Actor value;
    }
}