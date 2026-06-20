using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Values;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class UnitScheduleSpawnerAuthoring : MonoBehaviour {
        public float interval;
        public UnitIdentity Unit;
        public ArenaLocation location;

        public sealed class Baker : Baker<UnitScheduleSpawnerAuthoring> {
            public override void Bake(UnitScheduleSpawnerAuthoring authoring) {
                // schedule
                var schedule = GetEntity(TransformUsageFlags.None);
                AddComponent(schedule, new UnitSpawnSchedule {
                    interval = authoring.interval,
                    Unit = authoring.Unit,
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