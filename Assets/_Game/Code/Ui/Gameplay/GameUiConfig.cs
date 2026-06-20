using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Ui.Gameplay {
    internal class GameUiConfig : IComponentData {
        public HealthBarUiView playerHealthBarPrefab;
        public HealthBarUiView enemyHealthBarPrefab;

        public HealthBarUiView GetHealthBarPrefab(Faction faction) {
            switch (faction) {
                case Faction.Player:
                    return playerHealthBarPrefab;
                case Faction.Enemy:
                    return enemyHealthBarPrefab;
                default:
                    Debug.LogError($"Unknown faction: {faction}");
                    return null;
            }
        }
    }
}