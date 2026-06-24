using Game.Ecs._Refactor.Values;
using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct UnitSpawnSchedule : IComponentData {
        public ControlType ControlType;
        public Identity UnitId;
        public Identity FactionId;
        public float interval;
        public ArenaLocation location;
    }
}