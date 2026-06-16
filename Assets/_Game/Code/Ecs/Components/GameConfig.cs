using Game.Ecs._Refactor.Values;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Ecs.Components {
    public struct GameConfig : IComponentData {
        public Entity playerPrefab;
        public Entity enemy1Prefab;
        public Entity enemy2Prefab;
        public Entity playerBulletPrefab;
        public Entity enemyBulletPrefab;

        public float2 arenaMin2D;
        public float2 arenaMax2D;
        public float2 arenaCenter2D;

        public float outsideArenaSpawnOffset;

        public float playerBulletSpeed;
        public float enemyBulletSpeed;

        public int playerBulletDamage;
        public int enemyBulletDamage;
        public int enemyTouchDamage;

        public float bulletHitDistanceSq;
        public float enemyTouchDistanceSq;


        public readonly Entity GetActorPrefab(Actor actor) {
            switch (actor) {
                case Actor.Player:
                    return playerPrefab;
                case Actor.Enemy1:
                    return enemy1Prefab;
                case Actor.Enemy2:
                    return enemy2Prefab;
                default:
                    Debug.LogError($"Prefab not found for actor: {actor}.");
                    return Entity.Null;
            }
        }
    }
}