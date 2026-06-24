using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Identities.Traits {
    public struct Faction : IComponentData {
        public Identity FactionId;
    }
}