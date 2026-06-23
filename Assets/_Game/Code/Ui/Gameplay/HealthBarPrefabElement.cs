using Game.Framework.Assets;
using Unity.Entities;
using UnityEngine;

namespace Game.Ui.Gameplay {
    internal struct HealthBarPrefabElement : IBufferElementData {
        public Identity FactionId;
        public UnityObjectRef<HealthBarUiView> HealthBarPrefab;
    }

    internal static class HealthBarPrefabBufferExtensions {
        public static HealthBarUiView Get(this DynamicBuffer<HealthBarPrefabElement> entries, Identity factionId) {
            foreach (var element in entries)
                if (element.FactionId == factionId)
                    return element.HealthBarPrefab.Value;

            Debug.LogError($"Prefab not found for faction: {factionId}");
            return null;
        }
    }
}