using Game.Configs.Ammos;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs._Refactor.Components.Identities.Traits;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Ammos {
    public sealed class AmmoAuthoring : MonoBehaviour {
        public AmmoAsset ammo;
        public int damage;
        public float speed;

        private sealed class Baker : Baker<AmmoAuthoring> {
            public override void Bake(AmmoAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Ammo {
                    ammoId = authoring.ammo.AsIdentity(),
                    damage = authoring.damage,
                    speed = authoring.speed,
                });
            }
        }
    }
}