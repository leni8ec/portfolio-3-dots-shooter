using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Spawn {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct EnemySpawnSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<EnemySpawnTimer>();
            state.RequireForUpdate<RandomState>();
        }

        public void OnUpdate(ref SystemState state) {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();

            RefRW<EnemySpawnTimer> timer = SystemAPI.GetSingletonRW<EnemySpawnTimer>();
            RefRW<RandomState> randomState = SystemAPI.GetSingletonRW<RandomState>();

            float deltaTime = SystemAPI.Time.DeltaTime;
            timer.ValueRW.value -= deltaTime;
            if (timer.ValueRO.value > 0f)
                return;

            Random random = randomState.ValueRO.value;
            int side = random.NextInt(0, 4);

            float x = 0f;
            float z = 0f;

            switch (side) {
                case 0:
                    x = random.NextFloat(config.arenaMin.x, config.arenaMax.x);
                    z = config.arenaMax.y + config.enemySpawnOffset;
                    break;

                case 1:
                    x = random.NextFloat(config.arenaMin.x, config.arenaMax.x);
                    z = config.arenaMin.y - config.enemySpawnOffset;
                    break;

                case 2:
                    x = config.arenaMin.x - config.enemySpawnOffset;
                    z = random.NextFloat(config.arenaMin.y, config.arenaMax.y);
                    break;

                default:
                    x = config.arenaMax.x + config.enemySpawnOffset;
                    z = random.NextFloat(config.arenaMin.y, config.arenaMax.y);
                    break;
            }

            randomState.ValueRW.value = random;

            Entity enemy = state.EntityManager.Instantiate(config.enemyPrefab);
            state.EntityManager.SetComponentData(enemy, LocalTransform.FromPosition(
                new float3(x, 0f, z)
            ));

            timer.ValueRW.value = config.enemySpawnInterval;
        }
    }
}