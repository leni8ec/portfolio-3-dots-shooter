using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct ActorPrefabElement : IBufferElementData {
        public AssetId AssetId;
        public Entity Prefab;

        public AssetId Scope;

        public void Deconstruct(out AssetId assetId, out Entity prefab, out AssetId scope) {
            assetId = AssetId;
            prefab = Prefab;
            scope = Scope;
        }
    }
}