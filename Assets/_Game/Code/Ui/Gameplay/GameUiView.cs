using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Gameplay {
    internal sealed class GameUiView : MonoBehaviour {
        public static GameUiView Instance { get; private set; }

        public Button PauseButton { get; private set; }
        public Button ResumeButton { get; private set; }
        public Button RestartButton { get; private set; }
        public Button MainMenuButton { get; private set; }

        private VisualElement gameMenu;

        private void Awake() {
            Instance = this;

            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            PauseButton = root.Q<Button>("pause-button");
            ResumeButton = root.Q<Button>("resume-button");
            RestartButton = root.Q<Button>("restart-button");
            MainMenuButton = root.Q<Button>("main-menu-button");

            gameMenu = root.Q<VisualElement>("game-menu");

            SetMenuVisible(false);
        }

        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }

        public void SetMenuVisible(bool isVisible) =>
            gameMenu.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;

        public void SetOpenMenuButtonVisible(bool isVisible) =>
            PauseButton.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;

        public void SetResumeButtonVisible(bool isVisible) =>
            ResumeButton.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}