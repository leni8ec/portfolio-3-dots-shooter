using Game.Ecs._Refactor.Values;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct ActorSpawnSchedule : IComponentData {
        public float interval;
        public ArenaLocation location;
        public ActorIdentity actor;
    }
}