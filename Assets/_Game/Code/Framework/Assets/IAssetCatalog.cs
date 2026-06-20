using UnityEngine;

namespace Game.Framework.Assets {
    public interface IAssetCatalog<in T> where T : ScriptableObject {
        GameObject Get(T asset);
    }
}