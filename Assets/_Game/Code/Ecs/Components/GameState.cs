using Unity.Entities;

namespace Game.Ecs.Components {
    public enum GamePhase : byte {
        Playing = 0,
        Paused = 1,
        GameOver = 2
    }

    public struct GameState : IComponentData {
        public GamePhase phase;
    }
}