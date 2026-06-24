using Game.Configs.Factions;
using Game.Configs.Units;
using Game.Ecs._Refactor.Components;
using Game.Ecs._Refactor.Components.Common;
using Game.Ecs._Refactor.Values;
using TriInspector;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Spawners {
    public class UnitScheduleSpawnerAuthoring : MonoBehaviour {
        public UnitAsset Unit;
        public FactionAsset Faction;
        [PropertySpace]
        public float Interval;
        public ArenaLocation Location;

        public sealed class Baker : Baker<UnitScheduleSpawnerAuthoring> {
            public override void Bake(UnitScheduleSpawnerAuthoring authoring) {
                // schedule
                var schedule = GetEntity(TransformUsageFlags.None);
                AddComponent(schedule, new UnitSpawnSchedule {
                    UnitId = authoring.Unit,
                    FactionId = authoring.Faction,
                    interval = authoring.Interval,
                    location = authoring.Location
                });

                // timer
                AddComponent(schedule, new Timer {
                    value = authoring.Interval,
                });
                AddComponent<TimerElapsed>(schedule);
                SetComponentEnabled<TimerElapsed>(schedule, false);
            }
        }
    }
}