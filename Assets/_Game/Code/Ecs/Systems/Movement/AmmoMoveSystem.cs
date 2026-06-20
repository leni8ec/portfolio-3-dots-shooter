using Game.Ecs._Refactor.Components.Units;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Combat;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateAfter(typeof(PlayerShootSystem))]
    [UpdateAfter(typeof(EnemyShootSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct AmmoMoveSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<Ammo, ShotInfo>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, ammo, shotInfo) in SystemAPI
                         .Query<RefRW<LocalTransform>, Ammo, ShotInfo>()) {
                transform.ValueRW.Position += shotInfo.direction * ammo.speed * deltaTime;
            }
        }
    }
}