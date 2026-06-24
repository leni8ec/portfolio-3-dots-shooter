using Game.Configs.Ammos;
using Game.Configs.Units;
using Game.Ecs._Refactor.Components.Ammos;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Game.Ecs._Refactor.Components.Stats;
using Game.Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Units {
    public sealed class UnitAuthoring : MonoBehaviour {
        public UnitAsset Unit;
        public AmmoAsset Ammo;
        [Space]
        public int Health = 1;
        public float MoveSpeed = 2.5f;
        public float ShootInterval = 1.5f;

        private sealed class Baker : Baker<UnitAuthoring> {
            public override void Bake(UnitAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Unit { UnitId = authoring.Unit });
                AddComponent(entity, new Health { value = authoring.Health });
                AddComponent(entity, new MoveSpeed { value = authoring.MoveSpeed });
                AddComponent(entity, new AmmoEquipment { AmmoId = authoring.Ammo });
                AddComponent(entity, new ShootTimer {
                    Value = authoring.ShootInterval,
                    Interval = authoring.ShootInterval
                });
            }
        }
    }
}