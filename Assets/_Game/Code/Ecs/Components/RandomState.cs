using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct RandomState : IComponentData {
        public Random value;
    }
}