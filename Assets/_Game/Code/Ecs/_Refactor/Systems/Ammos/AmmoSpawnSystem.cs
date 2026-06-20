using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs._Refactor.Systems.Ammos {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoSpawnSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<AmmoSpawnRequest>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();

            foreach (var (request, requestEntity) in SystemAPI
                         .Query<AmmoSpawnRequest>().WithEntityAccess()) {
                ecb.DestroyEntity(requestEntity);

                var position = request.position;
                var rotation = Rotation3D.FromDirection(request.direction);

                var entity = ecb.Instantiate(config.GetAmmoPrefab(request.ammo, request.ownerFaction));
                ecb.SetName(entity, $"{request.ammo} ({request.ownerFaction})");
                ecb.SetComponent(entity, LocalTransform.FromPositionRotation(
                    position,
                    rotation
                ));
                ecb.AddComponent(entity, new ShotInfo {
                    ownerFaction = request.ownerFaction,
                    direction = request.direction
                });
            }
        }
    }
}