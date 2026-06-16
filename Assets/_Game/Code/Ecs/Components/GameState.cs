using Unity.Entities;

namespace Game.Ecs.Components {
    public enum GamePhase : byte {
        None = 0,
        Playing = 1,
        Paused = 2,
        GameOver = 3
    }

    public struct GameState : IComponentData {
        public GamePhase phase;
    }
}