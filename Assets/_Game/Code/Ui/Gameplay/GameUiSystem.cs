using Game.Ecs.Components;
using Game.Ecs.Groups;
using Game.Ecs.Systems.Bootstrap;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Game.Ui.Gameplay {
    [UpdateInGroup(typeof(GameplayUiSystemGroup))]
    internal partial class GameUiSystem : SystemBase {
        private const string GAME_SCENE_NAME = "Game";
        private const string MAIN_MENU_SCENE_NAME = "MainMenu";

        private GameUiView uiView;
        private bool isBound;
        private bool isMenuOpen;
        private bool lastIsGameOver;

        protected override void OnCreate() {
            RequireForUpdate<GameState>();
        }

        protected override void OnStartRunning() {
            isMenuOpen = false;
            lastIsGameOver = false;
        }

        protected override void OnUpdate() {
            TryBind();

            if (!isBound || !uiView)
                return;

            GameState gameState = SystemAPI.GetSingleton<GameState>();
            if (gameState.isGameOver == lastIsGameOver)
                return;

            lastIsGameOver = gameState.isGameOver;
            if (gameState.isGameOver)
                OpenMenu(false);
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
            OpenMenu(true);

        private void OnResumeClicked() =>
            CloseMenu();

        private void OnRestartClicked() =>
            SceneManager.LoadScene(GAME_SCENE_NAME);

        private void OnMainMenuClicked() =>
            SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);

        private void OpenMenu(bool canResume) {
            isMenuOpen = true;

            RefRW<GameState> gameState = SystemAPI.GetSingletonRW<GameState>();
            gameState.ValueRW.isPaused = canResume;

            uiView.SetMenuVisible(true);
            uiView.SetOpenMenuButtonVisible(false);
            uiView.SetResumeButtonVisible(canResume);
        }

        private void CloseMenu() {
            if (!isMenuOpen) return;
            isMenuOpen = false;

            RefRW<GameState> gameState = SystemAPI.GetSingletonRW<GameState>();
            gameState.ValueRW.isPaused = false;

            uiView.SetMenuVisible(false);
            uiView.SetOpenMenuButtonVisible(true);
            uiView.SetResumeButtonVisible(true);
        }
    }
}