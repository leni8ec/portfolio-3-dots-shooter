using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Ammos {
    public struct ExtraAmmoEquipment : IComponentData {
        public Identity AmmoId;
    }
}