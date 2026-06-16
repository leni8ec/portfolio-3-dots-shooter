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

            RequireForUpdate(playerQuery);
        }

        protected override void OnUpdate() {
            if (!Bind())
                return;

            foreach (var (playerTag, health, localTransform) in
                     SystemAPI.Query<PlayerTag, Health, LocalTransform>()) {
                var position = (Vector3) localTransform.Position + view.WorldOffset;

                view.SetHealth(health.value);
                view.SetPosition(position);

                if (camera)
                    view.FaceCamera(camera);
            }
        }

        protected override void OnDestroy() {
            Unbind();
        }

        private bool Bind() {
            if (!camera)
                camera = Camera.main;
            if (view)
                return true;

            view = PlayerHealthBarUiView.Instance;
            return view != null;
        }

        private void Unbind() {
            view = null;
            camera = null;
        }
    }
}