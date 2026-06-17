using UnityEngine;

namespace Game.Ecs._Refactor.Values {
    // todo: use 'AssetId' instead
    public enum ActorIdentity : byte {
        None = 0,

        Player = 1,
        Enemy1 = 2,
        Enemy2 = 3,
    }

    public static class ActorIdentityExtensions {
        public static ActorRole GetRole(this ActorIdentity identity) {
            switch (identity) {
                case ActorIdentity.Player:
                    return ActorRole.Player;
                case ActorIdentity.Enemy1:
                case ActorIdentity.Enemy2:
                    return ActorRole.Enemy;
                default:
                    Debug.LogError($"Unknown identity: {identity}");
                    return ActorRole.None;
            }
        }
    }
}