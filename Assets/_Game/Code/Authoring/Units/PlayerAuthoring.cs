using Game.Configs.Ammos;
using Game.Ecs._Refactor.Components;
using Game.Ecs.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Authoring.Units {
    /// <summary>
    /// Add with <see cref="UnitBaseAuthoring"/>
    /// </summary>
    public sealed class PlayerAuthoring : MonoBehaviour {

        public AmmoAsset extraAmmo;

        private sealed class Baker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<PlayerTag>(entity);
                AddComponent(entity, new PlayerInput {
                    Move = float2.zero,
                    AimDirection = new float3(0f, 0f, 1f)
                });
                // extra shoot request
                AddComponent(entity, new ExtraShootRequest { AmmoId = authoring.extraAmmo.AsAssetId() });
                SetComponentEnabled<ExtraShootRequest>(entity, false);
            }
        }
    }
}