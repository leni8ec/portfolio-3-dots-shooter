using UnityEngine;

namespace Game.Ecs._Refactor.Values {
    // todo: use 'AssetId' instead
    public enum UnitIdentity : byte {
        None = 0,

        Player = 1,
        Enemy1 = 2,
        Enemy2 = 3,
    }

    // todo: temp solution (use assets instead)
    public static class UnitIdentityExtensions {
        public static Faction GetFaction(this UnitIdentity identity) {
            switch (identity) {
                case UnitIdentity.Player:
                    return Faction.Player;
                case UnitIdentity.Enemy1:
                case UnitIdentity.Enemy2:
                    return Faction.Enemy;
                default:
                    Debug.LogError($"Unknown identity: {identity}");
                    return Faction.None;
            }
        }
    }
}