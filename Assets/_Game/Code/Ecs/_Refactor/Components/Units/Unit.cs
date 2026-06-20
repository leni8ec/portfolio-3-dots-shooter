using Game.Ecs._Refactor.Values;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components.Units {
    public struct Unit : IComponentData {
        public UnitIdentity identity;
        public Faction faction;
    }
}