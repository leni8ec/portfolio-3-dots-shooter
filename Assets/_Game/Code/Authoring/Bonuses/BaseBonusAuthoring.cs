using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Bonuses {
    public class BaseBonusAuthoring : MonoBehaviour {
        public bool ForPlayerOnly;

        private class Baker : Baker<BaseBonusAuthoring> {
            public override void Bake(BaseBonusAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bonus { ForPlayerOnly = authoring.ForPlayerOnly });
                AddComponent<Active>(entity);
                SetComponentEnabled<Active>(entity, false);
            }
        }

    }
}