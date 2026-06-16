using Unity.Entities;
using UnityEngine;

namespace Game.Ui.Gameplay {
    internal class GameUiConfigAuthoring : MonoBehaviour {
        public GameUiConfigAsset uiConfig;

        private sealed class Baker : Baker<GameUiConfigAuthoring> {
            public override void Bake(GameUiConfigAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(entity, new GameUiConfig {
                    healthBarPrefab = authoring.uiConfig.healthBarPrefab,
                });
            }
        }
    }
}