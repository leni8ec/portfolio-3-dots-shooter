using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Identities.Actors {
    public struct Ammo : IComponentData {
        public Identity AmmoId;
        public int Damage;
        public float Speed;
    }
}