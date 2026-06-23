using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public class AmmoSpawnRequest : IComponentData {
        public AssetId ownerFactionId;
        public AssetId ammoId;
        public float3 position;
        public float3 direction;
    }
}