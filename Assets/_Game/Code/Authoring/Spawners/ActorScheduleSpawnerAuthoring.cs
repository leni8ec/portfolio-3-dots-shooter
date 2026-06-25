using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Values;
using Game.Framework.Assets;
using TriInspector;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class ActorScheduleSpawnerAuthoring : MonoBehaviour {
        public IdentityAsset Actor;
        [PropertySpace]
        public float Interval;
        public ArenaLocation Location;

        public sealed class Baker : Baker<ActorScheduleSpawnerAuthoring> {
            public override void Bake(ActorScheduleSpawnerAuthoring authoring) {
                // schedule
                var schedule = GetEntity(TransformUsageFlags.None);
                AddComponent(schedule, new ActorSpawnSchedule {
                    ActorId = authoring.Actor,
                    Interval = authoring.Interval,
                    Location = authoring.Location
                });

                // timer
                AddComponent(schedule, new Timer {
                    Value = authoring.Interval,
                });
                AddComponent<TimerElapsed>(schedule);
                SetComponentEnabled<TimerElapsed>(schedule, false);
            }
        }

    }
}