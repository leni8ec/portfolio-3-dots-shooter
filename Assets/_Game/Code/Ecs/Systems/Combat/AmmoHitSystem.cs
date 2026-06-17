using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(AmmoMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoHitSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Ammo, ShotInfo>().Build());
        }

        // todo: flatten it (nested query)
        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();

            foreach (var (ammo, shotInfo, ammoTransform, ammoEntity) in SystemAPI
                         .Query<Ammo, ShotInfo, RefRO<LocalTransform>>().WithEntityAccess()) {

                var ammoPosition = ammoTransform.ValueRO.Position;
                var isPlayerAmmo = shotInfo.owner == ActorRole.Player;

                foreach (var (enemyTransform, enemyHealth, entity) in
                         SystemAPI.Query<RefRO<LocalTransform>, RefRW<Health>>().WithEntityAccess()) {

                    var isValidTarget = isPlayerAmmo
                        ? SystemAPI.HasComponent<EnemyTag>(entity)
                        : SystemAPI.HasComponent<PlayerTag>(entity);
                    if (!isValidTarget)
                        continue;

                    var distanceSq = math.distancesq(ammoPosition, enemyTransform.ValueRO.Position);
                    if (distanceSq > config.ammoHitDistanceSq)
                        continue;

                    enemyHealth.ValueRW.value -= ammo.damage;
                    ecb.DestroyEntity(ammoEntity);
                    break;
                }
            }
        }
    }
}