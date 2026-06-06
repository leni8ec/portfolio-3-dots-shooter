using Game.Ecs.Components;
using Game.Ecs.Groups;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.Input {
    [UpdateBefore(typeof(GameplaySystemGroup))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class PlayerInputAdapterSystem : SystemBase {
        private GameInput input;
        private Camera camera;

        protected override void OnCreate() {
            input = new GameInput();
            input.Enable();

            RequireForUpdate<PlayerTag>();
        }

        protected override void OnDestroy() {
            input.Disable();
            input.Dispose();
        }

        protected override void OnUpdate() {
            Vector2 move = input.Player.Move.ReadValue<Vector2>();
            Vector2 cursorScreenPosition = input.Player.Aim.ReadValue<Vector2>();

            if (!camera) camera = Camera.main;
            if (!camera)
                return;

            foreach (var (playerInput, transform) in
                     SystemAPI.Query<RefRW<PlayerInputData>, RefRO<LocalTransform>>()
                         .WithAll<PlayerTag>()) {
                // move
                playerInput.ValueRW.move = new float2(move.x, move.y);

                // direction
                Ray ray = camera.ScreenPointToRay(cursorScreenPosition);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                if (!groundPlane.Raycast(ray, out float enter))
                    continue;

                Vector3 hitPoint = ray.GetPoint(enter);

                float3 playerPosition = transform.ValueRO.Position;
                float3 aimDirection = new float3(
                    hitPoint.x - playerPosition.x,
                    0f,
                    hitPoint.z - playerPosition.z
                );

                if (math.lengthsq(aimDirection) > 0.001f) {
                    playerInput.ValueRW.aimDirection = math.normalize(aimDirection);
                }
            }
        }
    }
}