using Game.Framework.Unity.Extensions;
using TriInspector;
using UnityEngine;

namespace Game.Framework.Assets {
    /// <summary>
    /// ScriptableObject-based Enum
    /// </summary>
    public abstract class Asset : ScriptableObject, IAsset
    {
        [InfoBox("By default file name is the actual `AssetId`")]
        [PropertySpace]
        [Tooltip("Use custom AssetId instead of the file name \n(otherwise the file name will be used)")]
        [OnValueChanged(nameof(InitAssetIdIfEmpty))]
        [SerializeField] private bool useCustomId;

        [ShowIf(nameof(useCustomId))]
        [ValidateInput(nameof(ValidateAssetId))]
        [SerializeField] private string assetId;

        public AssetId AssetId => new(useCustomId ? assetId : name);

        private void InitAssetIdIfEmpty()
        {
            if (string.IsNullOrWhiteSpace(assetId))
                Reset();
        }

        private void Reset()
        {
            this.MarkDirty();
            assetId = name;
        }

        private TriValidationResult ValidateAssetId()
        {
            if (useCustomId && string.IsNullOrWhiteSpace(assetId))
                return TriValidationResult.Error("Custom AssetID cannot be empty");

            return TriValidationResult.Valid;
        }
    }
}