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
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, input, speed) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerInputData>, RefRO<MoveSpeed>>()
                         .WithAll<PlayerTag>()) {

                float3 direction = new float3(input.ValueRO.move.x, 0f, input.ValueRO.move.y);

                if (math.lengthsq(direction) > 1f)
                    direction = math.normalize(direction);

                float3 position = transform.ValueRO.Position;
                position += direction * speed.ValueRO.value * deltaTime;

                position.x = math.clamp(position.x, config.arenaMin2D.x, config.arenaMax2D.x);
                position.z = math.clamp(position.z, config.arenaMin2D.y, config.arenaMax2D.y);

                transform.ValueRW.Position = position;
            }
        }
    }
}