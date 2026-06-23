using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public class AmmoSpawnRequest : IComponentData {
        public Identity ownerFactionId;
        public Identity ammoId;
        public float3 position;
        public float3 direction;
    }
}