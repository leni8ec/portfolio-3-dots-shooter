using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;

namespace Game.Ui.Gameplay {
    [UpdateInGroup(typeof(GameplayUiSystemGroup))]
    internal partial class GameUiSystem : SystemBase {
        private GameUiView uiView;
        private bool isBound;
        private bool isMenuOpen;
        private GamePhase lastPhase;

        protected override void OnCreate() {
            RequireForUpdate<GameState>();
        }

        protected override void OnStartRunning() {
            isMenuOpen = false;
            lastPhase = GamePhase.Playing;
        }

        protected override void OnUpdate() {
            TryBind();

            if (!isBound || !uiView)
                return;

            GameState gameState = SystemAPI.GetSingleton<GameState>();
            if (gameState.phase == lastPhase)
                return;

            lastPhase = gameState.phase;
            ApplyGamePhase(gameState.phase);
        }

        protected override void OnDestroy() {
            Unbind();
        }

        private void TryBind() {
            if (isBound && uiView)
                return;

            if (isBound)
                Unbind();

            uiView = GameUiView.Instance;
            if (!uiView)
                return;

            uiView.PauseButton.clicked += OnPauseClicked;
            uiView.ResumeButton.clicked += OnResumeClicked;
            uiView.RestartButton.clicked += OnRestartClicked;
            uiView.MainMenuButton.clicked += OnMainMenuClicked;

            isBound = true;
        }

        private void Unbind() {
            if (!isBound || !uiView)
                return;

            uiView.PauseButton.clicked -= OnPauseClicked;
            uiView.ResumeButton.clicked -= OnResumeClicked;
            uiView.RestartButton.clicked -= OnRestartClicked;
            uiView.MainMenuButton.clicked -= OnMainMenuClicked;

            isBound = false;
        }

        private void OnPauseClicked() =>
            CreateFlowRequest(GameFlowAction.Pause);

        private void OnResumeClicked() =>
            CreateFlowRequest(GameFlowAction.Resume);

        private void OnRestartClicked() =>
            CreateFlowRequest(GameFlowAction.Restart);

        private void OnMainMenuClicked() =>
            CreateFlowRequest(GameFlowAction.MainMenu);

        private void CreateFlowRequest(GameFlowAction action) {
            Entity request = EntityManager.CreateEntity(typeof(GameFlowRequest));
            EntityManager.SetComponentData(request, new GameFlowRequest {
                action = action
            });
        }

        private void ApplyGamePhase(GamePhase phase) {
            switch (phase) {
                case GamePhase.Playing:
                    CloseMenu();
                    break;

                case GamePhase.Paused:
                    OpenMenu(true);
                    break;

                case GamePhase.GameOver:
                    OpenMenu(false);
                    break;
            }
        }

        private void OpenMenu(bool canResume) {
            isMenuOpen = true;
            uiView.SetMenuVisible(true);
            uiView.SetOpenMenuButtonVisible(false);
            uiView.SetResumeButtonVisible(canResume);
        }

        private void CloseMenu() {
            if (!isMenuOpen) return;
            isMenuOpen = false;

            uiView.SetMenuVisible(false);
            uiView.SetOpenMenuButtonVisible(true);
            uiView.SetResumeButtonVisible(true);
        }

    }
}