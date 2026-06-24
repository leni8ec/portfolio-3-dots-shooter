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
        public ControlType ControlType;
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
                    ControlType = authoring.ControlType,
                    UnitId = authoring.Unit,
                    FactionId = authoring.Faction,
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