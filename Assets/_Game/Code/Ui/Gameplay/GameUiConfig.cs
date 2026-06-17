using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Ui.Gameplay {
    internal class GameUiConfig : IComponentData {
        public HealthBarUiView playerHealthBarPrefab;
        public HealthBarUiView enemyHealthBarPrefab;

        public HealthBarUiView GetHealthBarPrefab(Actor actor) {
            switch (actor) {
                case Actor.Player:
                    return playerHealthBarPrefab;
                case Actor.Enemy1:
                case Actor.Enemy2:
                    return enemyHealthBarPrefab;
                default:
                    Debug.LogError($"Unknown actor {actor}");
                    return null;
            }
        }
    }
}