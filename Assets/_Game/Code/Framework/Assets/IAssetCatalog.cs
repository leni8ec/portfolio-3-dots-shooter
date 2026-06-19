using UnityEngine;

namespace Game.Framework.Assets {
    public interface IAssetCatalog<in T> where T : IAsset {
        GameObject Get(T asset);
    }
}