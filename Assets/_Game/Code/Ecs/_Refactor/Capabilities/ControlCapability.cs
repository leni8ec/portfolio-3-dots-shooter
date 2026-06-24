using Game.Ecs._Refactor.Components.Controls;
using Game.Ecs._Refactor.Values;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

// todo: subject naming considerations
//  - Capability: Grant/Revoke, Enable/Disable, Add/Remove
namespace Game.Ecs._Refactor.Capabilities {
    /// <summary>
    /// Grant player/bot control capability fot entities (ex. for units)
    /// </summary>
    internal static class ControlCapability {

        public static void Grant(ControlType controlType, Entity entity, EntityCommandBuffer ecb) {
            switch (controlType) {
                case ControlType.Player:
                    AddPlayerControlComponents(entity, ecb);
                    break;
                case ControlType.Bot:
                    AddBotControlComponents(entity, ecb);
                    break;
                default:
                    Debug.LogError($"Unknown control type: {controlType}");
                    break;
            }
        }

        private static void AddPlayerControlComponents(Entity entity, EntityCommandBuffer ecb) {
            ecb.AddComponent<PlayerControlTag>(entity);
            ecb.AddComponent(entity, new PlayerInput {
                Move = float2.zero,
                AimDirection = new float3(0f, 0f, 1f)
            });
        }

        private static void AddBotControlComponents(Entity entity, EntityCommandBuffer ecb) {
            ecb.AddComponent<BotControlTag>(entity);
            ecb.AddComponent<BotTarget>(entity);
            ecb.SetComponentEnabled<BotTarget>(entity, false);
        }
    }
}