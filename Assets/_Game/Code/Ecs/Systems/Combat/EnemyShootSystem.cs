using Game.Ecs._Refactor.Components.Enemies;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    [UpdateAfter(typeof(EnemyMoveToTargetSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<EnemyTag, EnemyTarget>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var config = SystemAPI.GetSingleton<GameConfig>();
            var deltaTime = SystemAPI.Time.DeltaTime;
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);

            foreach (var (enemyTarget, timer, transform) in
                     SystemAPI.Query<RefRO<EnemyTarget>, RefRW<ShootTimer>, RefRO<LocalTransform>>()
                         .WithAll<EnemyTag>()) {

                timer.ValueRW.value -= deltaTime;
                if (timer.ValueRO.value > 0f)
                    continue;

                var fromPosition = transform.ValueRO.Position;
                var targetPosition = transformLookup[enemyTarget.ValueRO.value].Position;
                var rotation = Rotation3D.LookRotation2D(fromPosition, targetPosition);

                var bullet = ecb.Instantiate(config.enemyBulletPrefab);
                ecb.SetName(bullet, "Bullet (enemy)");
                ecb.SetComponent(bullet, LocalTransform.FromPositionRotation(
                    transform.ValueRO.Position,
                    rotation
                ));
                ecb.SetComponent(bullet, new BulletData {
                    owner = BulletOwner.Enemy,
                    direction = Rotation3D.GetDirection2D(rotation),
                    speed = config.enemyBulletSpeed,
                    damage = config.enemyBulletDamage
                });

                timer.ValueRW.value = timer.ValueRO.interval;
            }
        }
    }
}