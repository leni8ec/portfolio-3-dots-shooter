using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct ShotInfo : IComponentData {
        public AssetId ownerFactionId;
        public float3 direction;
    }
}