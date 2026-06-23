using Game.Framework.Assets;
using Game.Framework.Unity.Attributes;
using UnityEngine;

namespace Game.Configs.Units {
    [CreateScriptableObjectAsset]
    public class UnitCatalog : AssetCatalog<UnitAsset, GameObject, NoScopeAsset> { }
}