using Unity.Entities;
using UnityEngine;

namespace Game.Ui.Gameplay {
    internal class GameUiConfigAuthoring : MonoBehaviour {
        public HealthBarCatalogAsset HealthBarCatalogAsset;

        private sealed class Baker : Baker<GameUiConfigAuthoring> {
            public override void Bake(GameUiConfigAuthoring authoring) {
                DependsOn(authoring.HealthBarCatalogAsset);

                var entity = GetEntity(TransformUsageFlags.None);
                var healthBarBuffer = AddBuffer<HealthBarPrefabElement>(entity);

                foreach (var entry in authoring.HealthBarCatalogAsset) {
                    healthBarBuffer.Add(new HealthBarPrefabElement {
                        FactionId = entry.AssetId,
                        HealthBarPrefab = entry.Prefab
                    });
                }
            }
        }
    }
}