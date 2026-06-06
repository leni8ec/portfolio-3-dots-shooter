using UnityEngine;

namespace Game.Authoring {
    [CreateAssetMenu(menuName = "Game/Config", fileName = "GameConfig")]
    public sealed class GameConfigAsset : ScriptableObject {
        [Header("Prefabs")]
        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public GameObject playerBulletPrefab;
        public GameObject enemyBulletPrefab;

        [Header("Arena")]
        public Vector2 arenaMin = new(-14.5f, -10f);
        public Vector2 arenaMax = new(14.5f, 10f);
        public float enemySpawnOffset = 2f;

        [Header("Enemy Spawn")]
        public float enemySpawnInterval = 2f;

        [Header("Bullets")]
        public float playerBulletSpeed = 12f;
        public float enemyBulletSpeed = 8f;
        public int playerBulletDamage = 1;
        public int enemyBulletDamage = 1;

        [Header("Contact")]
        public int enemyTouchDamage = 2;
        public float bulletHitDistance = 0.8f;
        public float enemyTouchDistance = 1f;
    }
}