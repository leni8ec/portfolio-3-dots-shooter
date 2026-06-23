using Game.Configs.Factions;
using Game.Framework.Assets;
using Game.Framework.Unity.Attributes;

namespace Game.Ui.Gameplay {
    [CreateScriptableObjectAsset]
    internal class HealthBarCatalogAsset : AssetCatalog<FactionAsset, HealthBarUiView, NoScopeAsset> { }
}