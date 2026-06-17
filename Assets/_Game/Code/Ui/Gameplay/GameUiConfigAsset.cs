using UnityEngine;

namespace Game.Ui.Gameplay {
    [CreateAssetMenu(menuName = "Game/UI Config", fileName = "GameUiConfig")]
    internal class GameUiConfigAsset : ScriptableObject {
        [Header("Health Bars")]
        public HealthBarUiView playerHealthBarPrefab;
        public HealthBarUiView enemyHealthBarPrefab;
    }
}