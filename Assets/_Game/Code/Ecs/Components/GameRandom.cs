using Unity.Entities;
using Unity.Mathematics;

namespace Game.Ecs.Components {
    public struct GameRandom : IComponentData {
        public Random value;
    }
}