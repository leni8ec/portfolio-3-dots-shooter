using Game.Ecs._Refactor.Values;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct ExtraShootRequest : IComponentData, IEnableableComponent {
        public AmmoIdentity Ammo;
    }
}