using Game.Ecs._Refactor.Components;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs._Refactor.Systems.Actors.Spawn {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct ActorSpawnSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<GameRandom>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<ActorSpawnRequest>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();
            ref var random = ref SystemAPI.GetSingletonRW<GameRandom>().ValueRW.value;

            foreach (var (request, entity) in SystemAPI.Query<ActorSpawnRequest>().WithEntityAccess()) {
                ecb.DestroyEntity(entity);

                var prefab = config.GetActorPrefab(request.actor);
                var instance = ecb.Instantiate(prefab);
                ecb.SetName(entity, request.actor.ToString()); // debug
                ecb.AddComponent(instance, LocalTransform.FromPosition(request.position));
            }
        }
    }
}