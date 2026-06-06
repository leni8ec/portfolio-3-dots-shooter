using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game.Ui.Gameplay {
    [UpdateInGroup(typeof(GameplayUiSystemGroup))]
    internal partial class PlayerHealthBarUiSystem : SystemBase {
        private PlayerHealthBarUiView view;
        private EntityQuery playerQuery;
        private Camera camera;

        protected override void OnCreate() {
            playerQuery = GetEntityQuery(
                ComponentType.ReadOnly<PlayerTag>(),
                ComponentType.ReadOnly<Health>(),
                ComponentType.ReadOnly<LocalTransform>()
            );
        }

        protected override void OnUpdate() {
            Bind();

            if (!view || playerQuery.IsEmpty)
                return;

            Entity playerEntity = playerQuery.GetSingletonEntity();

            Health health = SystemAPI.GetComponent<Health>(playerEntity);
            LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);

            Vector3 position = (Vector3) playerTransform.Position + view.WorldOffset;

            view.SetHealth(health.value);
            view.SetPosition(position);

            if (!camera) camera = Camera.main;
            view.FaceCamera(camera);
        }

        protected override void OnDestroy() {
            Unbind();
        }

        private void Bind() {
            if (view) return;
            view = PlayerHealthBarUiView.Instance;
        }

        private void Unbind() {
            view = null;
            camera = null;
        }
    }
}