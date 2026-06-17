using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Ui.Gameplay {
    [UpdateInGroup(typeof(GameplayUiSystemGroup))]
    internal partial class HealthBarUiSystem : SystemBase {
        private Camera camera;

        protected override void OnCreate() {
            RequireForUpdate<GameUiConfig>();
            RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAny<ActorMember, ActorUi>().Build());
        }

        protected override void OnUpdate() {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(World.Unmanaged);
            var uiConfig = SystemAPI.ManagedAPI.GetSingleton<GameUiConfig>();

            Bind();

            // create
            foreach (var (actorMember, entity) in SystemAPI
                         .Query<ActorMember>()
                         .WithNone<ActorUi>().WithEntityAccess()) {
                var healthBarInstance = Object.Instantiate(uiConfig.GetHealthBarPrefab(actorMember.value));
                ecb.AddComponent(entity, new ActorUi { healthBar = healthBarInstance });
            }

            // move
            foreach (var (health, localTransform, actorUi) in SystemAPI
                         .Query<Health, LocalTransform, ActorUi>()
                         .WithAll<ActorMember>()) {

                var healthBar = actorUi.healthBar;
                var position = (Vector3) localTransform.Position + healthBar.WorldOffset;
                healthBar.SetHealth(health.value);
                healthBar.SetPosition(position);
                if (camera)
                    healthBar.FaceCamera(camera);
            }

            // cleanup
            foreach (var (actorUi, entity) in SystemAPI
                         .Query<ActorUi>()
                         .WithNone<ActorMember>().WithEntityAccess()) {

                if (actorUi.healthBar != null)
                    Object.Destroy(actorUi.healthBar.gameObject);

                ecb.RemoveComponent<ActorUi>(entity);
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