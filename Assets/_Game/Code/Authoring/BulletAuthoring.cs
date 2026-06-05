using Game.Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Authoring {
    public sealed class BulletAuthoring : MonoBehaviour {
        private sealed class Baker : Baker<BulletAuthoring> {
            public override void Bake(BulletAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BulletData {
                    owner = BulletOwner.Player,
                    direction = new float3(0f, 0f, 1f),
                    speed = 0f,
                    damage = 0
                });
            }
        }
    }
}