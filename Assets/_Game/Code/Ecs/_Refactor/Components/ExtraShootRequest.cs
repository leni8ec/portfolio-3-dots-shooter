using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct ExtraShootRequest : IComponentData, IEnableableComponent {
        public Identity AmmoId;
    }
}