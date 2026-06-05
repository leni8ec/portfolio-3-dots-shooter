using Game.Ecs.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Ecs.Systems.Spawn {
    internal partial struct PlayerSpawnSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GameConfig>();
        }

        public void OnUpdate(ref SystemState state) {
            if (SystemAPI.HasSingleton<PlayerTag>()) return;

            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            Entity player = ecb.Instantiate(config.playerPrefab);

            ecb.SetName(player, "Player");
            ecb.SetComponent(player, LocalTransform.FromPosition(
                new float3(0, 0, 0)
            ));
            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            state.Enabled = false;
        }

    }
}