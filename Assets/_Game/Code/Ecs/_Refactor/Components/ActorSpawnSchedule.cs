using Game.Ecs._Refactor.Values;
using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct ActorSpawnSchedule : IComponentData {
        public Identity ActorId;

        public float Interval;
        public ArenaLocation Location;
    }
}