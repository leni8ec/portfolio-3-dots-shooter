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


        public readonly Entity GetUnitPrefab(UnitIdentity unit) {
            switch (unit) {
                case UnitIdentity.Player:
                    return playerPrefab;
                case UnitIdentity.Enemy1:
                    return enemy1Prefab;
                case UnitIdentity.Enemy2:
                    return enemy2Prefab;
                default:
                    Debug.LogError($"Unit prefab not found for Unit: {unit}.");
                    return Entity.Null;
            }
        }

        public readonly Entity GetAmmoPrefab(AmmoIdentity ammo, Faction faction) {
            switch (faction, ammo) {
                case (Faction.Player, AmmoIdentity.Bullet):
                    return playerBulletPrefab;
                case (Faction.Player, AmmoIdentity.Missile):
                    return playerMissilePrefab;
                case (Faction.Enemy, AmmoIdentity.Bullet):
                    return enemyBulletPrefab;
                case (Faction.Enemy, AmmoIdentity.Missile):
                    return enemyMissilePrefab;

                default:
                    Debug.LogError($"Ammo prefab not found for ammo ({ammo}) and faction ({faction}).");
                    return Entity.Null;
            }
        }
    }
}