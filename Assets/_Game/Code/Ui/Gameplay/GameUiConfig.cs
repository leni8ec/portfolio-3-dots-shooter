using Unity.Entities;

namespace Game.Ui.Gameplay {
    internal class GameUiConfig : IComponentData {
        public PlayerHealthBarUiView healthBarPrefab;
    }
}