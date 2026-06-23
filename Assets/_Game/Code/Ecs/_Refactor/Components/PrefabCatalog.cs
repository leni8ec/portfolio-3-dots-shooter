using Game.Ecs._Refactor.Utils;
using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct PrefabCatalog : IComponentData {

        public ScopedNativeParallelHashMap<AssetId, Entity> Actors;

    }

}