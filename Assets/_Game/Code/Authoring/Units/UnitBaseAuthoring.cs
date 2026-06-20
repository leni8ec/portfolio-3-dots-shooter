using Game.Ecs._Refactor.Components.Units;
using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Units {
    public sealed class UnitBaseAuthoring : MonoBehaviour {

        public UnitIdentity Unit;
        public AmmoIdentity ammo;
        [Space]
        public int health = 1;
        public float moveSpeed = 2.5f;
        public float shootInterval = 1.5f;

        private sealed class Baker : Baker<UnitBaseAuthoring> {
            public override void Bake(UnitBaseAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Unit {
                    identity = authoring.Unit,
                    faction = authoring.Unit.GetFaction(),
                });
                AddComponent(entity, new Health { value = authoring.health });
                AddComponent(entity, new MoveSpeed { value = authoring.moveSpeed });
                AddComponent(entity, new AmmoEquipment { value = authoring.ammo });
                AddComponent(entity, new ShootTimer {
                    value = authoring.shootInterval,
                    interval = authoring.shootInterval
                });
            }
        }
    }
}