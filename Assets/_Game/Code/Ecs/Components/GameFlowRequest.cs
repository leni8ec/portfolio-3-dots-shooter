using Unity.Entities;

namespace Game.Ecs.Components {
    public enum GameFlowAction : byte {
        PlayGame = 0,
        Pause = 1,
        Resume = 2,
        Restart = 3,
        MainMenu = 4,
        Quit = 5
    }

    public struct GameFlowRequest : IComponentData {
        public GameFlowAction action;
    }
}