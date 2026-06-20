using Game.Ecs._Refactor.Components.Units;
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
                .WithAny<Unit, UnitUi>().Build());
        }

        protected override void OnUpdate() {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(World.Unmanaged);
            var uiConfig = SystemAPI.ManagedAPI.GetSingleton<GameUiConfig>();

            Bind();

            // create
            foreach (var (unit, entity) in SystemAPI
                         .Query<Unit>()
                         .WithNone<UnitUi>().WithEntityAccess()) {
                var healthBarInstance = Object.Instantiate(uiConfig.GetHealthBarPrefab(unit.faction));
                ecb.AddComponent(entity, new UnitUi { healthBar = healthBarInstance });
            }

            // move
            foreach (var (health, unitUi, localTransform) in SystemAPI
                         .Query<Health, UnitUi, RefRO<LocalTransform>>()
                         .WithAll<Unit>()) {

                var healthBar = unitUi.healthBar;
                var position = (Vector3) localTransform.ValueRO.Position + healthBar.WorldOffset;
                healthBar.SetHealth(health.value);
                healthBar.SetPosition(position);
                if (camera)
                    healthBar.FaceCamera(camera);
            }

            // cleanup
            foreach (var (unitUi, entity) in SystemAPI
                         .Query<UnitUi>()
                         .WithNone<Unit>().WithEntityAccess()) {

                if (unitUi.healthBar != null)
                    Object.Destroy(unitUi.healthBar.gameObject);

                ecb.RemoveComponent<UnitUi>(entity);
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