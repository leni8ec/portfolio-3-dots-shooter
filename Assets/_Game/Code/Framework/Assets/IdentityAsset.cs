using Game.Framework.Unity.Extensions;
using TriInspector;
using UnityEngine;

namespace Game.Framework.Assets {
    /// <summary>
    /// Identity Asset - it's a ScriptableObject-based Enum (with asset id backing field)
    /// </summary>
    public abstract class IdentityAsset : ScriptableObject, IIdentityAsset {
        [PropertySpace]
        [InfoBox("By default file name is the actual `AssetId`")]
        [Tooltip("Use custom asset id, instead of the file name \n(otherwise the file name will be used)")]
        [OnValueChanged(nameof(InitAssetIdIfEmpty))]
        [SerializeField] private bool useCustomId;

        [LabelText("Asset Id")]
        [ShowIf(nameof(useCustomId))]
        [ValidateInput(nameof(ValidateAssetId))]
        [SerializeField] private string customAssetId;

        [HideIf(nameof(useCustomId))]
        [ShowInInspector] private string AssetId =>
            useCustomId ? customAssetId : name;

        public override string ToString() =>
            AssetId;

        public Identity AsIdentity() =>
            new(AssetId);

        public static implicit operator Identity(IdentityAsset asset) =>
            asset != null ? asset.AsIdentity() : default;


        #region Editor

        private void InitAssetIdIfEmpty() {
            if (string.IsNullOrWhiteSpace(customAssetId))
                Reset();
        }

        private void Reset() {
            this.MarkDirty();
            customAssetId = name;
        }

        private TriValidationResult ValidateAssetId() {
            if (useCustomId && string.IsNullOrWhiteSpace(customAssetId))
                return TriValidationResult.Error("Custom AssetID cannot be empty");

            return TriValidationResult.Valid;
        }

        #endregion

    }
}