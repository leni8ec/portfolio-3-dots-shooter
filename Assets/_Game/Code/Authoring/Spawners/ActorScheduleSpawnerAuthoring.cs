using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class ActorScheduleSpawnerAuthoring : MonoBehaviour {
        public float interval;
        public Actor actor;
        public ArenaLocation location;

        public sealed class Baker : Baker<ActorScheduleSpawnerAuthoring> {
            public override void Bake(ActorScheduleSpawnerAuthoring authoring) {
                // schedule
                var schedule = GetEntity(TransformUsageFlags.None);
                AddComponent(schedule, new LocationSpawnSchedule {
                    interval = authoring.interval,
                    actor = authoring.actor,
                    location = authoring.location
                });

                // timer
                AddComponent(schedule, new Timer {
                    value = authoring.interval,
                });
                AddComponent<TimerElapsed>(schedule);
                SetComponentEnabled<TimerElapsed>(schedule, false);

            }
        }
    }
}