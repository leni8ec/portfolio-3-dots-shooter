using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.MainMenu {
    internal sealed class MainMenuUiView : MonoBehaviour {
        public static MainMenuUiView Instance { get; private set; }

        public Button PlayButton { get; private set; }
        public Button ExitButton { get; private set; }

        private void Awake() {
            Instance = this;

            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            PlayButton = root.Q<Button>("play-button");
            ExitButton = root.Q<Button>("exit-button");
        }

        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }
    }
}