using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Units {
    public struct Ammo : IComponentData {
        public int damage;
        public float speed;
    }
}