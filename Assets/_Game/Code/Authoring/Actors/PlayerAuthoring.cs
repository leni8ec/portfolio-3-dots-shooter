using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Values;
using Game.Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Authoring.Actors {
    /// <summary>
    /// Add with <see cref="ActorBaseAuthoring"/>
    /// </summary>
    public sealed class PlayerAuthoring : MonoBehaviour {

        public AmmoIdentity extraAmmo;

        private sealed class Baker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<PlayerTag>(entity);
                AddComponent(entity, new PlayerInputData {
                    move = float2.zero,
                    aimDirection = new float3(0f, 0f, 1f)
                });
                // extra shoot request
                AddComponent(entity, new ExtraShootRequest { Ammo = authoring.extraAmmo });
                SetComponentEnabled<ExtraShootRequest>(entity, false);
            }
        }
    }
}