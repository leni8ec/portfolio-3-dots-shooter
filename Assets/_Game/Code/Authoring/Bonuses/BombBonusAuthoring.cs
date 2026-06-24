using Game.Ecs._Refactor.Components.Bonuses;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Bonuses {
    public class BombBonusAuthoring : MonoBehaviour {
        public byte DamageAmount;

        private class Baker : Baker<BombBonusAuthoring> {
            public override void Bake(BombBonusAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bomb { DamageAmount = authoring.DamageAmount });
            }
        }

    }
}