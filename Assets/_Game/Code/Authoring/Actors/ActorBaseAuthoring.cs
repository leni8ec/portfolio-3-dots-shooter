using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Actors {
    public sealed class ActorBaseAuthoring : MonoBehaviour {

        public ActorIdentity actor;
        public AmmoIdentity ammo;
        [Space]
        public int health = 1;
        public float moveSpeed = 2.5f;
        public float shootInterval = 1.5f;

        private sealed class Baker : Baker<ActorBaseAuthoring> {
            public override void Bake(ActorBaseAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Actor {
                    identity = authoring.actor,
                    role = authoring.actor.GetRole(),
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