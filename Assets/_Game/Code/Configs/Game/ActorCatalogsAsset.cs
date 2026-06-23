using System.Collections.Generic;
using Game.Framework.Assets;
using Game.Framework.Unity.Attributes;
using UnityEngine;

namespace Game.Configs.Game {
    [CreateScriptableObjectAsset]
    public sealed class ActorCatalogsAsset : ScriptableObject {
        public List<AssetCatalog<GameObject>> Catalogs;
    }
}