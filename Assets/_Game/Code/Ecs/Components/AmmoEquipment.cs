using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs.Components {
    public struct AmmoEquipment : IComponentData {
        public Identity AmmoId;
    }
}