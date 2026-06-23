using Game.Ecs._Refactor.Values;
using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct UnitSpawnSchedule : IComponentData {
        public AssetId UnitId;
        public float interval;
        public ArenaLocation location;
    }
}