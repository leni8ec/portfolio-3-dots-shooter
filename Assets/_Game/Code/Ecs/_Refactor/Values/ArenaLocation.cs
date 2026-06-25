namespace Game.Ecs._Refactor.Values {
    public enum ArenaLocation : byte {
        None = 0,

        /// From related transform
        Related = 1,
        InsideArena = 2,
        OutsideArena = 3
    }
}