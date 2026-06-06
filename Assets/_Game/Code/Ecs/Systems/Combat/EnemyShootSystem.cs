using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(EnemyMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform, timer, enemyState) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRW<ShootTimer>, RefRO<EnemyState>>()
                         .WithAll<EnemyTag>()) {

                if (!enemyState.ValueRO.isInsideArena)
                    continue;

                timer.ValueRW.value -= deltaTime;
                if (timer.ValueRO.value > 0f)
                    continue;

                float3 direction = playerPosition - transform.ValueRO.Position;
                direction.y = 0f;
                if (math.lengthsq(direction) < 0.001f)
                    continue;

                direction = math.normalize(direction);
                quaternion rotation = math.normalize(quaternion.LookRotation(direction, math.up()));

                Entity bullet = ecb.Instantiate(config.enemyBulletPrefab);
                ecb.SetName(bullet, "Bullet (enemy)");
                ecb.SetComponent(bullet, LocalTransform.FromPositionRotation(
                    transform.ValueRO.Position,
                    rotation
                ));
                ecb.SetComponent(bullet, new BulletData {
                    owner = BulletOwner.Enemy,
                    direction = direction,
                    speed = config.enemyBulletSpeed,
                    damage = config.enemyBulletDamage
                });

                timer.ValueRW.value = timer.ValueRO.interval;
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}