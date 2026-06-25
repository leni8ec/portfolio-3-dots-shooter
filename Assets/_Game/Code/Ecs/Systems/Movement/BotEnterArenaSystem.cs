using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Components.Stats;
using Game.Ecs._Refactor.Logic;
using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

// todo: rewrite to use factions
namespace Game.Ecs.Systems.Movement {
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    internal partial struct BotEnterArenaSystem : ISystem {
        private EntityQuery playersQuery;

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
            state.RequireForUpdate<GameRandom>();
            state.RequireForUpdate<BotControlTag>();

            playersQuery = SystemAPI.QueryBuilder()
                .WithAll<PlayerControlTag, LocalTransform>().Build();
        }

        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<GameConfig>();
            ref var random = ref SystemAPI.GetSingletonRW<GameRandom>().ValueRW.value;
            var deltaTime = SystemAPI.Time.DeltaTime;

            var players = default(NativeArray<Entity>);
            var playersLoaded = false;

            foreach (var (moveSpeed, transform, entity) in
                     SystemAPI.Query<RefRO<MoveSpeed>, RefRW<LocalTransform>>()
                         .WithAll<BotControlTag>()
                         .WithDisabled<BotTarget>()
                         .WithEntityAccess()) {
                var position = transform.ValueRO.Position;
                var isInsideArena = Arena2D.IsInside(position, config.arenaMin2D, config.arenaMax2D);

                // move to arena center
                if (!isInsideArena) {
                    var targetPosition = Position3D.From2D(config.arenaCenter2D);
                    transform.ValueRW.Position = Position3D.MoveTowards2D(
                        position, targetPosition, moveSpeed.ValueRO.Value, deltaTime);
                    continue;
                }

                // check players
                if (!playersLoaded) {
                    playersQuery.ToEntityArray(Allocator.Temp);

                    players = playersQuery.ToEntityArray(Allocator.Temp);
                    playersLoaded = true;
                }
                if (players.Length == 0)
                    continue;

                // select target player as target
                var randomPlayerIndex = random.NextInt(players.Length);
                var targetPlayer = players[randomPlayerIndex];

                SystemAPI.SetComponent(entity, new BotTarget { Entity = targetPlayer });
                SystemAPI.SetComponentEnabled<BotTarget>(entity, true);
            }

            if (playersLoaded)
                players.Dispose();
        }
    }
}