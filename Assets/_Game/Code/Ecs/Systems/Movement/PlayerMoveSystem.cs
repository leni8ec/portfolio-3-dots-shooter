using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Components.Stats;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct PlayerMoveSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<PlayerControlTag, PlayerInput>().Build());
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, input, speed) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerInput>, RefRO<MoveSpeed>>()
                         .WithAll<PlayerControlTag>()) {

                float3 direction = new float3(input.ValueRO.Move.x, 0f, input.ValueRO.Move.y);

                if (math.lengthsq(direction) > 1f)
                    direction = math.normalize(direction);

                float3 position = transform.ValueRO.Position;
                position += direction * speed.ValueRO.Value * deltaTime;

                position.x = math.clamp(position.x, config.arenaMin2D.x, config.arenaMax2D.x);
                position.z = math.clamp(position.z, config.arenaMin2D.y, config.arenaMax2D.y);

                transform.ValueRW.Position = position;
            }
        }
    }
}