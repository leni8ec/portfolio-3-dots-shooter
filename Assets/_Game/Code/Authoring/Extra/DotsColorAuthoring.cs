using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Authoring.Extra {
    public class DotsColorAuthoring : MonoBehaviour {
        private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
        [FormerlySerializedAs("cubeColor")]
        public Color Color = Color.white;

        private void OnValidate() {
            var render = GetComponent<Renderer>();
            if (render == null)
                return;

            var props = new MaterialPropertyBlock();
            props.SetColor(_baseColor, Color);
            render.SetPropertyBlock(props);
        }

        class Baker : Baker<DotsColorAuthoring> {
            public override void Bake(DotsColorAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new URPMaterialPropertyBaseColor { Value = (Vector4) authoring.Color });
            }
        }
    }
}