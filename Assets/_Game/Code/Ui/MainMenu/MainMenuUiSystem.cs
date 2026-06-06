using Game.Ecs.Components;
using Unity.Entities;

namespace Game.Ui.MainMenu {
    internal partial class MainMenuUiSystem : SystemBase {
        private MainMenuUiView uiView;
        private bool isBound;

        protected override void OnUpdate() {
            if (isBound)
                return;

            uiView = MainMenuUiView.Instance;
            if (!uiView)
                return;

            uiView.PlayButton.clicked += OnPlayClicked;
            uiView.ExitButton.clicked += OnExitClicked;

            isBound = true;
        }

        protected override void OnDestroy() {
            if (!uiView || !isBound)
                return;

            uiView.PlayButton.clicked -= OnPlayClicked;
            uiView.ExitButton.clicked -= OnExitClicked;
        }

        private void OnPlayClicked() =>
            CreateFlowRequest(GameFlowAction.PlayGame);

        private void OnExitClicked() =>
            CreateFlowRequest(GameFlowAction.Quit);

        private void CreateFlowRequest(GameFlowAction action) {
            Entity request = EntityManager.CreateEntity(typeof(GameFlowRequest));
            EntityManager.SetComponentData(request, new GameFlowRequest {
                action = action
            });
        }
    }
}