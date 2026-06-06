using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct BulletHitSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<BulletData>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (bulletTransform, bullet, bulletEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<BulletData>>()
                         .WithEntityAccess()) {

                float3 bulletPosition = bulletTransform.ValueRO.Position;
                bool isPlayerBullet = bullet.ValueRO.owner == BulletOwner.Player;

                foreach (var (enemyTransform, enemyHealth, entity) in
                         SystemAPI.Query<RefRO<LocalTransform>, RefRW<Health>>().WithEntityAccess()) {

                    bool isValidTarget = isPlayerBullet
                        ? SystemAPI.HasComponent<EnemyTag>(entity)
                        : SystemAPI.HasComponent<PlayerTag>(entity);
                    if (!isValidTarget)
                        continue;

                    float distanceSq = math.distancesq(bulletPosition, enemyTransform.ValueRO.Position);
                    if (distanceSq > config.bulletHitDistanceSq)
                        continue;

                    enemyHealth.ValueRW.value -= bullet.ValueRO.damage;
                    ecb.DestroyEntity(bulletEntity);
                    break;
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}