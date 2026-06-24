using Game.Ecs._Refactor.Components.Bonuses;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Components.Identities.Actors;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Bonuses {
    public class BaseBonusAuthoring : MonoBehaviour {

        private class Baker : Baker<BaseBonusAuthoring> {
            public override void Bake(BaseBonusAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Bonus>(entity);
                AddComponent<Active>(entity);
                SetComponentEnabled<Active>(entity, false);
            }
        }

    }
}