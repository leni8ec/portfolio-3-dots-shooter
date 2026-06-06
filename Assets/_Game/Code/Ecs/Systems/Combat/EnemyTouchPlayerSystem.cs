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
    internal partial struct EnemyTouchPlayerSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
            RefRW<Health> playerHealth = SystemAPI.GetComponentRW<Health>(playerEntity);
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (enemyTransform, enemyEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<EnemyTag>().WithEntityAccess()) {

                float distanceSq = math.distancesq(playerPosition, enemyTransform.ValueRO.Position);
                if (distanceSq > config.enemyTouchDistanceSq)
                    continue;

                playerHealth.ValueRW.value -= config.enemyTouchDamage;
                ecb.DestroyEntity(enemyEntity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}