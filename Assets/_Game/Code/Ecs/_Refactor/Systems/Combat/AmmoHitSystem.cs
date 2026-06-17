using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs._Refactor.Systems.Combat {
    /// <summary>
    /// Uses simple MxN algorithm with AABB (Axis-Aligned Bounding Box), without "Spatial Hashing" or "Quadtrees".
    /// Also use parallel schedule for fobs (this may not be optimal for a small number of entities, but it serves to show the algorithm itself).
    /// </summary>
    [UpdateAfter(typeof(AmmoMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoHitSystem : ISystem {
        private EntityQuery ammoQuery;
        private EntityQuery playersQuery;
        private EntityQuery enemiesQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();

            ammoQuery = SystemAPI.QueryBuilder()
                .WithAll<Ammo, ShotInfo, LocalTransform>().Build();

            playersQuery = SystemAPI.QueryBuilder()
                .WithAll<PlayerTag, Health, LocalTransform>().Build();

            enemiesQuery = SystemAPI.QueryBuilder()
                .WithAll<EnemyTag, Health, LocalTransform>().Build();

            state.RequireForUpdate(ammoQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<GameConfig>();

            var hitDistanceSq = config.ammoHitDistanceSq;
            var hitDistance = math.sqrt(hitDistanceSq);

            var playerEntities = playersQuery.ToEntityArray(Allocator.TempJob);
            var playerTransforms = playersQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);

            var enemyEntities = enemiesQuery.ToEntityArray(Allocator.TempJob);
            var enemyTransforms = enemiesQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var playerAmmoHandle = new AmmoHitPassJob {
                AmmoOwner = ActorRole.Player,
                TargetEntities = enemyEntities,
                TargetTransforms = enemyTransforms,
                HitDistanceSq = hitDistanceSq,
                HitDistance = hitDistance,
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel(ammoQuery, state.Dependency);

            var enemyAmmoHandle = new AmmoHitPassJob {
                AmmoOwner = ActorRole.Enemy,
                TargetEntities = playerEntities,
                TargetTransforms = playerTransforms,
                HitDistanceSq = hitDistanceSq,
                HitDistance = hitDistance,
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel(ammoQuery, state.Dependency);

            state.Dependency = JobHandle.CombineDependencies(playerAmmoHandle, enemyAmmoHandle);
        }

        [BurstCompile]
        private partial struct AmmoHitPassJob : IJobEntity {
            public ActorRole AmmoOwner;

            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<Entity> TargetEntities;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<LocalTransform> TargetTransforms;

            public float HitDistanceSq;
            public float HitDistance;

            public EntityCommandBuffer.ParallelWriter Ecb;

            private void Execute([ChunkIndexInQuery] int chunkIndex,
                Entity ammoEntity,
                in Ammo ammo,
                in ShotInfo shotInfo,
                in LocalTransform ammoTransform
            ) {
                if (shotInfo.owner != AmmoOwner)
                    return;
                if (TargetEntities.Length == 0)
                    return;

                var ammoPosition = ammoTransform.Position;

                for (var i = 0; i < TargetEntities.Length; i++) {
                    var targetPosition = TargetTransforms[i].Position;

                    // XZ axis only
                    var delta = targetPosition.xz - ammoPosition.xz;

                    // Cheap reject before squared distance.
                    if (math.abs(delta.x) > HitDistance ||
                        math.abs(delta.y) > HitDistance)
                        continue;

                    if (math.lengthsq(delta) > HitDistanceSq)
                        continue;

                    var damageRequest = Ecb.CreateEntity(chunkIndex);
                    Ecb.AddComponent(chunkIndex, damageRequest, new ApplyDamageRequest {
                        Target = TargetEntities[i],
                        Value = ammo.damage
                    });
                    Ecb.DestroyEntity(chunkIndex, ammoEntity);

                    break;
                }
            }
        }
    }
}