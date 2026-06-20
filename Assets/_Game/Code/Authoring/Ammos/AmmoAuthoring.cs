using Game.Ecs._Refactor.Components.Units;
using Game.Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Ammos {
    public sealed class AmmoAuthoring : MonoBehaviour {
        public int damage;
        public float speed;

        private sealed class Baker : Baker<AmmoAuthoring> {
            public override void Bake(AmmoAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Ammo {
                    damage = authoring.damage,
                    speed = authoring.speed,
                });
            }
        }
    }
}