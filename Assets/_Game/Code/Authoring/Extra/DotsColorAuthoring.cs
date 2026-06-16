using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Game.Authoring.Extra {
    public class DotsColorAuthoring : MonoBehaviour {
        private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
        public Color cubeColor = Color.white;

        private void OnValidate() {
            var render = GetComponent<Renderer>();
            if (render != null) {
                var props = new MaterialPropertyBlock();
                props.SetColor(_baseColor, cubeColor);
                render.SetPropertyBlock(props);
            }
        }

        class Baker : Baker<DotsColorAuthoring> {
            public override void Bake(DotsColorAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new URPMaterialPropertyBaseColor { Value = (Vector4) authoring.cubeColor });
            }
        }
    }
}