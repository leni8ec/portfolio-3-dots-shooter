using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct GameConfig : IComponentData {
        public float2 arenaMin2D;
        public float2 arenaMax2D;
        public float2 arenaCenter2D;

        public float outsideArenaSpawnOffset;

        public int enemyTouchDamage;

        public float ammoHitDistanceSq;
        public float unitTouchDistanceSq;
    }
}