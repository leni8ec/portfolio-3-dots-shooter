using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Units {
    public struct Ammo : IComponentData {
        public AssetId assetId;
        public int damage;
        public float speed;
    }
}