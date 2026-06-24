using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Common {
    public struct Timer : IComponentData {
        public float Value;
    }

    public struct TimerElapsed : IComponentData, IEnableableComponent { }
}