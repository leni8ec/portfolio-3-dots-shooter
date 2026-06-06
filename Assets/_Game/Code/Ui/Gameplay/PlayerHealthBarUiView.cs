using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Gameplay {
    internal sealed class PlayerHealthBarUiView : MonoBehaviour {
        public static PlayerHealthBarUiView Instance { get; private set; }

        [field: SerializeField] public Vector3 WorldOffset { get; private set; } = new(0f, 1.5f, 0f);
        [field: SerializeField] public bool LookAtCamera { get; private set; } = true;

        private Label healthLabel;

        private void Awake() {
            Instance = this;

            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            healthLabel = root.Q<Label>("health-label");
        }

        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
        }

        public void SetHealth(int health) {
            healthLabel.text = health.ToString();
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }

        public void FaceCamera(Camera cam) {
            if (!LookAtCamera || !cam)
                return;

            transform.rotation = cam.transform.rotation;
        }
    }
}