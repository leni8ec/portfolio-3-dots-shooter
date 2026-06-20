using Unity.Entities;

namespace Game.Ecs.Components {
    public enum GameFlowAction : byte {
        None = 0,

        PlayGame = 1,
        Pause = 2,
        Resume = 3,
        Restart = 4,
        MainMenu = 5,
        Quit = 6
    }

    public struct GameFlowRequest : IComponentData {
        public GameFlowAction action;
    }
}