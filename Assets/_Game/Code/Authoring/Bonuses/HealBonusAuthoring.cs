using Game.Ecs._Refactor.Components.Bonuses;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Bonuses {
    public class HealBonusAuthoring : MonoBehaviour {
        public byte HealAmount;

        private class Baker : Baker<HealBonusAuthoring> {
            public override void Bake(HealBonusAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Heal { Amount = authoring.HealAmount });
            }
        }

    }
}