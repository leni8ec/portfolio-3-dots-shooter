using Game.Ecs._Refactor.Components.Bonuses;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Bonuses {
    public class WipeBonusAuthoring : MonoBehaviour {
        public byte DamageAmount;

        private class Baker : Baker<WipeBonusAuthoring> {
            public override void Bake(WipeBonusAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Wipe { DamageAmount = authoring.DamageAmount });
            }
        }

    }
}