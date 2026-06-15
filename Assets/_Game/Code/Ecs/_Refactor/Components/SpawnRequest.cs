using Game.Ecs._Refactor.Values;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Components {
    public struct SpawnRequest : IComponentData {
        public Actor actor;
        public float3 position;
    }

}