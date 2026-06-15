using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Unity {
    public struct PrefabReference : IComponentData {
        public Entity value;
    }
}