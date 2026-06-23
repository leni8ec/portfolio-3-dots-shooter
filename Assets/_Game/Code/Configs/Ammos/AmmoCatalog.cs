using Game.Configs.Factions;
using Game.Framework.Assets;
using Game.Framework.Unity.Attributes;
using UnityEngine;

namespace Game.Configs.Ammos {
    [CreateScriptableObjectAsset]
    public class AmmoCatalog : AssetCatalog<AmmoAsset, GameObject, FactionAsset> { }
}