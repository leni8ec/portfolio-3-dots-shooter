using Game.Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Authoring {
    public sealed class PlayerAuthoring : MonoBehaviour {

        public int health = 10;
        public float moveSpeed = 6f;
        public float shootInterval = 0.3f;

        private sealed class Baker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<PlayerTag>(entity);
                AddComponent(entity, new Health { value = authoring.health });
                AddComponent(entity, new MoveSpeed { value = authoring.moveSpeed });
                AddComponent(entity, new ShootTimer {
                    value = 0f,
                    interval = authoring.shootInterval
                });
                AddComponent(entity, new PlayerInputData {
                    move = float2.zero,
                    aimDirection = new float3(0f, 0f, 1f)
                });
            }
        }
    }
}