using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Ammos;
using Game.Ecs._Refactor.Components.Identities.Traits;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs._Refactor.Systems.Ammos {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoSpawnSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PrefabCatalog>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<AmmoSpawnRequest>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var prefabCatalog = SystemAPI.GetSingleton<PrefabCatalog>();

            foreach (var (request, requestEntity) in SystemAPI
                         .Query<AmmoSpawnRequest>().WithEntityAccess()) {
                ecb.DestroyEntity(requestEntity);

                var ammoId = request.ammoId;
                var factionId = request.ownerFactionId;
                var position = request.position;
                var direction = request.direction;
                var rotation = Rotation3D.FromDirection(direction);

                // Debug.Log($"ammo: {ammoId}, faction: {factionId}");
                var prefab = prefabCatalog.Actors.Get(ammoId, factionId);
                var entity = ecb.Instantiate(prefab);
                ecb.SetName(entity, $"{ammoId} ({factionId})");
                ecb.SetComponent(entity, LocalTransform.FromPositionRotation(position, rotation));
                ecb.AddComponent(entity, new Faction { FactionId = factionId });
                ecb.AddComponent(entity, new ShotInfo { Direction = direction });
            }
        }
    }
}