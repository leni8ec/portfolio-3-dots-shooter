using Game.Ecs._Refactor.Components.Enemies;
using Game.Ecs.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Actors {
    /// <summary>
    /// Add with <see cref="ActorBaseAuthoring"/>
    /// </summary>
    public sealed class EnemyAuthoring : MonoBehaviour {

        private sealed class Baker : Baker<EnemyAuthoring> {
            public override void Bake(EnemyAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<EnemyTag>(entity);
                AddComponent<EnemyTarget>(entity);
                SetComponentEnabled<EnemyTarget>(entity, false);
            }
        }
    }
}