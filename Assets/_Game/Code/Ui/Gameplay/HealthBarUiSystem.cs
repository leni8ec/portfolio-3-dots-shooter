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
                .WithAny<Actor, ActorUi>().Build());
        }

        protected override void OnUpdate() {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(World.Unmanaged);
            var uiConfig = SystemAPI.ManagedAPI.GetSingleton<GameUiConfig>();

            Bind();

            // create
            foreach (var (actor, entity) in SystemAPI
                         .Query<Actor>()
                         .WithNone<ActorUi>().WithEntityAccess()) {
                var healthBarInstance = Object.Instantiate(uiConfig.GetHealthBarPrefab(actor.role));
                ecb.AddComponent(entity, new ActorUi { healthBar = healthBarInstance });
            }

            // move
            foreach (var (health, actorUi, localTransform) in SystemAPI
                         .Query<Health, ActorUi, RefRO<LocalTransform>>()
                         .WithAll<Actor>()) {

                var healthBar = actorUi.healthBar;
                var position = (Vector3) localTransform.ValueRO.Position + healthBar.WorldOffset;
                healthBar.SetHealth(health.value);
                healthBar.SetPosition(position);
                if (camera)
                    healthBar.FaceCamera(camera);
            }

            // cleanup
            foreach (var (actorUi, entity) in SystemAPI
                         .Query<ActorUi>()
                         .WithNone<Actor>().WithEntityAccess()) {

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