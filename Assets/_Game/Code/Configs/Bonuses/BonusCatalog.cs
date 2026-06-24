using Game.Framework.Assets;
using Game.Framework.Unity.Attributes;
using UnityEngine;

namespace Game.Configs.Bonuses {
    [CreateScriptableObjectAsset]
    public class BonusCatalog : AssetCatalog<BonusAsset, GameObject, NoScopeAsset> { }
}