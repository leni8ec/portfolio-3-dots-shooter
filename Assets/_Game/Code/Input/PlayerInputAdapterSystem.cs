using Game.Ecs._Refactor.Components.Ammos;
using Game.Ecs._Refactor.Components.Controls;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.Input {
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerInputAdapterSystem : SystemBase {
        private GameInput input;
        private GameInput.TopDownActions inputActions;
        private Camera camera;

        protected override void OnCreate() {
            input = new GameInput();
            input.Enable();
            inputActions = input.TopDown;

            RequireForUpdate(SystemAPI.QueryBuilder()
                .WithAll<PlayerControlTag, PlayerInput>().Build());
        }

        protected override void OnDestroy() {
            input.Disable();
            input.Dispose();
        }

        protected override void OnUpdate() {
            if (!camera) camera = Camera.main;
            if (!camera) return;

            var move = inputActions.Move.ReadValue<Vector2>();
            var cursorScreenPosition = inputActions.Aim.ReadValue<Vector2>();
            var wasShootPressedThisFrame = inputActions.ExtraShoot.WasPressedThisFrame();

            var ecb = wasShootPressedThisFrame
                ? SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                    .CreateCommandBuffer(World.Unmanaged)
                : default;


            foreach (var (playerInput, transform, playerEntity) in SystemAPI
                         .Query<RefRW<PlayerInput>, RefRO<LocalTransform>>()
                         .WithAll<PlayerControlTag>().WithEntityAccess()) {

                { // === move ===
                    playerInput.ValueRW.Move = new float2(move.x, move.y);
                }

                { // === direction ===
                    var ray = camera.ScreenPointToRay(cursorScreenPosition);
                    var groundPlane = new Plane(Vector3.up, Vector3.zero);
                    if (groundPlane.Raycast(ray, out var enter)) {
                        var hitPoint = ray.GetPoint(enter);

                        var playerPosition = transform.ValueRO.Position;
                        var aimDirection = new float3(
                            hitPoint.x - playerPosition.x,
                            0f,
                            hitPoint.z - playerPosition.z
                        );

                        if (math.lengthsq(aimDirection) > 0.001f) {
                            playerInput.ValueRW.AimDirection = math.normalize(aimDirection);
                        }
                    }
                }

                { // === shoot ===
                    if (wasShootPressedThisFrame && SystemAPI.HasComponent<ExtraShootRequest>(playerEntity)) {
                        ecb.SetComponentEnabled<ExtraShootRequest>(playerEntity, true);
                    }
                }
            }
        }
    }
}