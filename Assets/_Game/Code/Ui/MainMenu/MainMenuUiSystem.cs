using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Ui.MainMenu {
    internal partial class MainMenuUiSystem : SystemBase {
        private MainMenuUiView uiView;
        private bool isBound;

        protected override void OnUpdate() {
            if (isBound)
                return;

            uiView = MainMenuUiView.Instance;
            if (uiView == null)
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
            SceneManager.LoadScene("Game");

        private void OnExitClicked() =>
            Application.Quit();
    }
}