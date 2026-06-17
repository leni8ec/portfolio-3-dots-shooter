using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Ui.Gameplay {
    internal class GameUiConfig : IComponentData {
        public HealthBarUiView playerHealthBarPrefab;
        public HealthBarUiView enemyHealthBarPrefab;

        public HealthBarUiView GetHealthBarPrefab(ActorRole actor) {
            switch (actor) {
                case ActorRole.Player:
                    return playerHealthBarPrefab;
                case ActorRole.Enemy:
                    return enemyHealthBarPrefab;
                default:
                    Debug.LogError($"Unknown actor {actor}");
                    return null;
            }
        }
    }
}