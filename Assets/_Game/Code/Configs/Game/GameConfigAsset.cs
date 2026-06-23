using UnityEngine;

namespace Game.Configs.Game {
    [CreateAssetMenu(menuName = "Game/Config", fileName = "GameConfig")]
    public sealed class GameConfigAsset : ScriptableObject {

        [Header("Arena")]
        public Vector2 arenaMin = new(-14.5f, -10f);
        public Vector2 arenaMax = new(14.5f, 10f);
        public float outsideArenaSpawnOffset = 2f;

        [Header("Contact")]
        public int enemyTouchDamage = 2;
        public float ammoHitDistance = 0.8f;
        public float enemyTouchDistance = 1f;
    }
}