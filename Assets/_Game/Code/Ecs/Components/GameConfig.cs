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
        public Entity playerMissilePrefab;
        public Entity enemyBulletPrefab;
        public Entity enemyMissilePrefab;

        public float2 arenaMin2D;
        public float2 arenaMax2D;
        public float2 arenaCenter2D;

        public float outsideArenaSpawnOffset;

        public int enemyTouchDamage;

        public float ammoHitDistanceSq;
        public float enemyTouchDistanceSq;


        public readonly Entity GetActorPrefab(ActorIdentity actor) {
            switch (actor) {
                case ActorIdentity.Player:
                    return playerPrefab;
                case ActorIdentity.Enemy1:
                    return enemy1Prefab;
                case ActorIdentity.Enemy2:
                    return enemy2Prefab;
                default:
                    Debug.LogError($"Actor prefab not found for actor: {actor}.");
                    return Entity.Null;
            }
        }

        public readonly Entity GetAmmoPrefab(AmmoIdentity ammo, ActorRole actorRole) {
            switch (actor: actorRole, ammo) {
                case (ActorRole.Player, AmmoIdentity.Bullet):
                    return playerBulletPrefab;
                case (ActorRole.Player, AmmoIdentity.Missile):
                    return playerMissilePrefab;
                case (ActorRole.Enemy, AmmoIdentity.Bullet):
                    return enemyBulletPrefab;
                case (ActorRole.Enemy, AmmoIdentity.Missile):
                    return enemyMissilePrefab;

                default:
                    Debug.LogError($"Ammo prefab not found for ammo ({ammo}) and actor role ({actorRole}).");
                    return Entity.Null;
            }
        }
    }
}