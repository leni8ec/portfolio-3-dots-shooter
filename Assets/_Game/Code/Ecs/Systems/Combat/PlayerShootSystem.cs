using Game.Ecs.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Combat {
    public partial struct PlayerShootSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform, input, timer) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerInputData>, RefRW<ShootTimer>>()
                         .WithAll<PlayerTag>()) {

                timer.ValueRW.value -= deltaTime;
                if (timer.ValueRO.value > 0f)
                    continue;

                Entity bullet = ecb.Instantiate(config.playerBulletPrefab);
                float3 bulletPos = transform.ValueRO.Position;
                quaternion bulletRot = math.normalize(quaternion.LookRotation(input.ValueRO.aimDirection, math.up()));
                ecb.SetComponent(bullet, LocalTransform.FromPositionRotation(
                    bulletPos,
                    bulletRot
                ));
                ecb.SetComponent(bullet, new BulletData {
                    owner = BulletOwner.Player,
                    direction = input.ValueRO.aimDirection,
                    speed = config.playerBulletSpeed,
                    damage = config.playerBulletDamage
                });

                timer.ValueRW.value = timer.ValueRO.interval;
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}