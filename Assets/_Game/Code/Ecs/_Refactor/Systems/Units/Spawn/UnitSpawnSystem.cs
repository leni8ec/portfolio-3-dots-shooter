using Game.Ecs._Refactor.Capabilities;
using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Identities.Traits;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs._Refactor.Systems.Units.Spawn {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct UnitSpawnSystem : ISystem {

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PrefabCatalog>();
            state.RequireForUpdate<GameRandom>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<UnitSpawnRequest>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var prefabCatalog = SystemAPI.GetSingleton<PrefabCatalog>();
            ref var random = ref SystemAPI.GetSingletonRW<GameRandom>().ValueRW.value;

            foreach (var (request, requestEntity) in SystemAPI.Query<UnitSpawnRequest>().WithEntityAccess()) {
                ecb.DestroyEntity(requestEntity);

                // Debug.Log($"Spawn unit: {request.UnitId}, scope: {default}");
                var prefab = prefabCatalog.Actors.Get(request.UnitId, default);
                var instance = ecb.Instantiate(prefab);
                ecb.SetName(instance, request.UnitId.ToFixedString()); // debug
                ecb.SetComponent(instance, LocalTransform.FromPosition(request.Position));
                ecb.AddComponent(instance, new Faction { FactionId = request.FactionId });

                ControlCapability.Grant(request.ControlType, instance, ecb);
            }
        }
    }
}