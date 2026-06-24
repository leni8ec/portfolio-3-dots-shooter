using Game.Framework.Assets;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components.Ammos {
    public class AmmoSpawnRequest : IComponentData {
        public Identity OwnerFactionId;
        public Identity AmmoId;
        public float3 Position;
        public float3 Direction;
    }
}