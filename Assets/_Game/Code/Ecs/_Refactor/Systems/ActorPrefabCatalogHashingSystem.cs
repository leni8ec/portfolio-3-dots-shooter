using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Utils;
using Game.Framework.Assets;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.Ecs._Refactor.Systems {
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct ActorPrefabCatalogHashingSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<ActorPrefabElement>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            using var ecb = new EntityCommandBuffer(Allocator.Temp);
            var entity = SystemAPI.GetSingletonEntity<ActorPrefabElement>();
            var buffer = state.EntityManager.GetBuffer<ActorPrefabElement>(entity);

            // create actual map
            var prefabScopedMap = new ScopedNativeParallelHashMap<Identity, Entity>(buffer.Length, Allocator.Persistent);
            foreach (var (identity, prefab, scope) in buffer)
                if (!prefabScopedMap.TryAdd(identity, prefab, scope))
                    Debug.LogError($"Couldn't add prefab (identity: {identity}, scope: {scope}) to hash catalog");

            // clear old map (if exists)
            if (state.EntityManager.HasComponent<PrefabCatalog>(entity)) {
                var catalog = state.EntityManager.GetComponentData<PrefabCatalog>(entity);
                if (catalog.Actors.IsCreated)
                    catalog.Actors.Clear();
            } else {
                ecb.AddComponent<PrefabCatalog>(entity);
            }

            // Debug.Log($"Create Map! (count: {prefabScopedMap.Count()})");
            ecb.SetComponent(entity, new PrefabCatalog { Actors = prefabScopedMap });
            ecb.RemoveComponent<ActorPrefabElement>(entity); // remove buffer
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) {
            foreach (var catalog in SystemAPI.Query<RefRW<PrefabCatalog>>()) {
                if (catalog.ValueRO.Actors.IsCreated)
                    catalog.ValueRW.Actors.Dispose();
            }
        }
    }
}