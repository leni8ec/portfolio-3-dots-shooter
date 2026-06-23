using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct ShotInfo : IComponentData {
        public Identity ownerFactionId;
        public float3 direction;
    }
}