using Game.Ecs._Refactor.Values;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public class AmmoSpawnRequest : IComponentData {
        public ActorRole owner;
        public AmmoIdentity ammo;
        public float3 position;
        public float3 direction;
    }
}