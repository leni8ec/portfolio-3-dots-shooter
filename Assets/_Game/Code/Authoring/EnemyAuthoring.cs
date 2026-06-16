using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Enemies;
using Game.Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring {
    public sealed class EnemyAuthoring : MonoBehaviour {

        public int health = 1;
        public float moveSpeed = 2.5f;
        public float shootInterval = 1.5f;

        private sealed class Baker : Baker<EnemyAuthoring> {
            public override void Bake(EnemyAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<EnemyTag>(entity);
                AddComponent(entity, new Health { value = authoring.health });
                AddComponent(entity, new MoveSpeed { value = authoring.moveSpeed });
                AddComponent(entity, new ShootTimer {
                    value = authoring.shootInterval,
                    interval = authoring.shootInterval
                });

                AddComponent<EnemyTarget>(entity);
                SetComponentEnabled<EnemyTarget>(entity, false);
            }
        }
    }
}