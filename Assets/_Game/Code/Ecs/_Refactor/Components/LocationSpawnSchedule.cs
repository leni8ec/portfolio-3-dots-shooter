using Game.Ecs._Refactor.Values;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct LocationSpawnSchedule : IComponentData {
        public float interval;
        public ArenaLocation location;
        public Actor actor;
    }
}