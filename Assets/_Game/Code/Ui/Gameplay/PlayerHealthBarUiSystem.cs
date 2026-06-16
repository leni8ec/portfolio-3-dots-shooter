using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Ui.Gameplay {
    [UpdateInGroup(typeof(GameplayUiSystemGroup))]
    internal partial class PlayerHealthBarUiSystem : SystemBase {
        private EntityQuery newPlayersQuery;
        private Camera camera;

        protected override void OnCreate() {
            RequireForUpdate<GameUiConfig>();
            RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAny<PlayerTag, PlayerUi>().Build());
        }

        protected override void OnUpdate() {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(World.Unmanaged);
            var uiConfig = SystemAPI.ManagedAPI.GetSingleton<GameUiConfig>();

            Bind();

            // create
            foreach (var (_, entity) in SystemAPI.Query<PlayerTag>()
                         .WithNone<PlayerUi>().WithEntityAccess()) {
                var healthBarInstance = Object.Instantiate(uiConfig.healthBarPrefab);
                ecb.AddComponent(entity, new PlayerUi { healthBar = healthBarInstance });
            }

            // move
            foreach (var (health, localTransform, playerUi) in
                     SystemAPI.Query<Health, LocalTransform, PlayerUi>()
                         .WithAll<PlayerTag>()) {

                var healthBar = playerUi.healthBar;
                var position = (Vector3) localTransform.Position + healthBar.WorldOffset;
                healthBar.SetHealth(health.value);
                healthBar.SetPosition(position);
                if (camera)
                    healthBar.FaceCamera(camera);
            }

            // cleanup
            foreach (var (playerUi, entity) in
                     SystemAPI.Query<PlayerUi>().WithNone<PlayerTag>().WithEntityAccess()) {

                if (playerUi.healthBar != null)
                    Object.Destroy(playerUi.healthBar.gameObject);

                ecb.RemoveComponent<PlayerUi>(entity);
            }
        }

        protected override void OnDestroy() {
            Unbind();
        }

        private void Bind() {
            if (!camera)
                camera = Camera.main;
        }

        private void Unbind() {
            camera = null;
        }
    }
}