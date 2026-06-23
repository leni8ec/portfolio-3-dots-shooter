using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Units;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Movement;
using Game.Framework.Assets;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// todo: considerations
//  - change simple MxN to optimized algorithms (Spatial Hash, Quadtree)
namespace Game.Ecs._Refactor.Systems.Combat {
    /// <summary>
    /// Uses simple MxN algorithm with AABB (Axis-Aligned Bounding Box), without "Spatial Hashing" or "Quadtrees".
    /// Also use parallel schedule for jobs (this may not be optimal for a small number of entities, but it serves to show the algorithm itself).
    /// </summary>
    [UpdateAfter(typeof(AmmoMoveSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoHitSystem : ISystem {
        private EntityQuery ammoQuery;
        private EntityQuery unitsQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameConfig>();

            ammoQuery = SystemAPI.QueryBuilder()
                .WithAll<Ammo, ShotInfo, LocalTransform>().Build();

            unitsQuery = SystemAPI.QueryBuilder()
                .WithAll<Unit, Health, LocalTransform>().Build();

            state.RequireForUpdate(ammoQuery);
            state.RequireForUpdate(unitsQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<GameConfig>();
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var hitDistanceSq = config.ammoHitDistanceSq;
            var hitDistance = math.sqrt(hitDistanceSq);

            var targets = new NativeArray<UnitHitTarget>(unitsQuery.CalculateEntityCount(), Allocator.TempJob);

            state.Dependency = new CollectTargetsJob {
                Targets = targets,
            }.ScheduleParallel(unitsQuery, state.Dependency);

            state.Dependency = new AmmoHitPassJob {
                Ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                HitDistanceSq = hitDistanceSq,
                HitDistance = hitDistance,
                Targets = targets,
            }.ScheduleParallel(ammoQuery, state.Dependency);
        }

        [BurstCompile]
        private partial struct AmmoHitPassJob : IJobEntity {
            public EntityCommandBuffer.ParallelWriter Ecb;

            public float HitDistanceSq;
            public float HitDistance;

            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<UnitHitTarget> Targets;

            private void Execute([ChunkIndexInQuery] int chunkIndex,
                Entity ammoEntity,
                in Ammo ammo,
                in ShotInfo shotInfo,
                in LocalTransform ammoTransform
            ) {
                var ammoPositionXZ = ammoTransform.Position.xz;

                for (var i = 0; i < Targets.Length; i++) {
                    var target = Targets[i];

                    if (shotInfo.ownerFactionId == target.FactionId)
                        continue;

                    // XZ axis only
                    var delta = target.Position.xz - ammoPositionXZ;

                    // Cheap reject before squared distance.
                    if (math.abs(delta.x) > HitDistance ||
                        math.abs(delta.y) > HitDistance)
                        continue;

                    if (math.lengthsq(delta) > HitDistanceSq)
                        continue;

                    var damageRequest = Ecb.CreateEntity(chunkIndex);
                    Ecb.AddComponent(chunkIndex, damageRequest, new ApplyDamageRequest {
                        Target = target.Entity,
                        Value = ammo.damage
                    });

                    Ecb.DestroyEntity(chunkIndex, ammoEntity);
                    break;
                }
            }
        }

        [BurstCompile]
        private partial struct CollectTargetsJob : IJobEntity {
            public NativeArray<UnitHitTarget> Targets;

            private void Execute(
                [EntityIndexInQuery] int index,
                Entity entity,
                in Unit unit,
                in LocalTransform transform
            ) {
                Targets[index] = new UnitHitTarget {
                    Entity = entity,
                    Position = transform.Position,
                    FactionId = unit.factionId
                };
            }
        }

        private struct UnitHitTarget {
            public Entity Entity;
            public float3 Position;
            public Identity FactionId;
        }
    }
}