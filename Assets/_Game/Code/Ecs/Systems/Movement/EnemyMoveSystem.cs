using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Spawn;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Movement {
    [UpdateAfter(typeof(EnemySpawnSystem))]
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemyMoveSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<PlayerTag>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            foreach (var (transform, speed, enemyState) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<EnemyState>>()
                         .WithAll<EnemyTag>()) {
                float3 position = transform.ValueRO.Position;

                bool isInsideArena =
                    position.x >= config.arenaMin.x &&
                    position.x <= config.arenaMax.x &&
                    position.z >= config.arenaMin.y &&
                    position.z <= config.arenaMax.y;

                float3 targetPosition = isInsideArena
                    ? playerPosition
                    : new float3(0f, 0f, 0f);

                if (isInsideArena)
                    enemyState.ValueRW.isInsideArena = true;

                float3 direction = targetPosition - position;
                direction.y = 0f;
                if (math.lengthsq(direction) < 0.001f)
                    continue;

                direction = math.normalize(direction);
                transform.ValueRW.Position += direction * speed.ValueRO.value * deltaTime;
            }
        }
    }
}