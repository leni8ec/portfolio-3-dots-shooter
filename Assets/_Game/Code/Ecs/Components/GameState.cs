using Unity.Entities;

namespace Game.Ecs.Components {
    public struct GameState : IComponentData {
        public bool isGameOver;
        public bool isPaused;
    }
}