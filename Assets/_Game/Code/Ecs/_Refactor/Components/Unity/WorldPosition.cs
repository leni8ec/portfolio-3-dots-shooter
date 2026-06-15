using Unity.Entities;
using Unity.Mathematics;

// ReSharper disable once CheckNamespace
namespace Game.Ecs._Refactor.Components.Unity {
    public struct WorldPosition : IComponentData {
        public float3 value;
    }

}