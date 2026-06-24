using Game.Configs.Ammos;
using Game.Ecs._Refactor.Components.Ammos;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Ammos {
    public sealed class ExtraAmmoAuthoring : MonoBehaviour {

        public AmmoAsset ExtraAmmo;

        private sealed class Baker : Baker<ExtraAmmoAuthoring> {
            public override void Bake(ExtraAmmoAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // extra ammo with shoot request
                AddComponent(entity, new ExtraAmmoEquipment { AmmoId = authoring.ExtraAmmo.AsIdentity() });
                AddComponent<ExtraShootRequest>(entity);
                SetComponentEnabled<ExtraShootRequest>(entity, false);
            }
        }
    }
}