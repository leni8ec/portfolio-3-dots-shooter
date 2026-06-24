using Game.Ecs._Refactor.Components.Ammos;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Combat;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateAfter(typeof(PlayerShootSystem))]
    [UpdateAfter(typeof(BotShootSystem))]
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
                transform.ValueRW.Position += shotInfo.Direction * ammo.Speed * deltaTime;
            }
        }
    }
}