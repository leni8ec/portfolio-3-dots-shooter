using Unity.Entities;

namespace Game.Ecs.Groups {
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class GameplaySystemGroup : ComponentSystemGroup {}
}