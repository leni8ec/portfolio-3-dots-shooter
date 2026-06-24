using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Identities.Actors {
    public struct Unit : IComponentData {
        public Identity UnitId;
    }
}