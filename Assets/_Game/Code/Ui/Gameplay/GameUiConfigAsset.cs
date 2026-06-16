using UnityEngine;

namespace Game.Ui.Gameplay {
    [CreateAssetMenu(menuName = "Game/UI Config", fileName = "GameUiConfig")]
    internal class GameUiConfigAsset : ScriptableObject {
        public PlayerHealthBarUiView healthBarPrefab;
    }
}