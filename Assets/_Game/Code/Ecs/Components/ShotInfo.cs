using Game.Ecs._Refactor.Values;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct ShotInfo : IComponentData {
        public ActorRole owner;
        public float3 direction;
    }
}