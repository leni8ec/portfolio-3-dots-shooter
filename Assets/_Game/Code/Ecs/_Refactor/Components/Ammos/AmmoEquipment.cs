using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Ammos {
    public struct AmmoEquipment : IComponentData {
        public Identity AmmoId;
    }
}