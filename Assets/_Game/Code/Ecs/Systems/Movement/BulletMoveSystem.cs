using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Combat;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateAfter(typeof(PlayerShootSystem))]
    [UpdateAfter(typeof(EnemyShootSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct BulletMoveSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BulletData>();
        }

        public void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, bullet) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<BulletData>>()) {
                transform.ValueRW.Position += bullet.ValueRO.direction * bullet.ValueRO.speed * deltaTime;
            }
        }
    }
}