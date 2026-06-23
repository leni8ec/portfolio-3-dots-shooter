using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Units {
    public struct Unit : IComponentData {
        public AssetId unitId;
        public AssetId factionId;
    }
}