using Unity.Entities;

namespace Game.Ecs.Groups {
    [UpdateAfter(typeof(GameplaySystemGroup))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class GameplayUiSystemGroup : ComponentSystemGroup { }
}