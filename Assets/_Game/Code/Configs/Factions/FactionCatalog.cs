using Game.Framework.Assets;
using Game.Framework.Unity.Attributes;
using UnityEngine;

namespace Game.Configs.Factions {
    [CreateScriptableObjectAsset]
    public class FactionCatalog : AssetCatalog<FactionAsset, GameObject, NoScopeAsset> { }
}