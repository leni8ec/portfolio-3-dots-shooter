using Unity.Entities;

namespace Game.Ecs.Systems.Bootstrap {
    public struct GameState : IComponentData {
        public bool isGameOver;
        public bool isPaused;
    }
}