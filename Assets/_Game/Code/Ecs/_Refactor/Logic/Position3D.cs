using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Logic {
    internal static class Position3D {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 From2D(float2 position2D) =>
            new(position2D.x, 0, position2D.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 MoveTowards2D(float3 fromPosition, float3 targetPosition, float speed, float deltaTime) {
            var direction = targetPosition - fromPosition;
            direction.y = 0f;

            var distanceSq = math.lengthsq(direction);
            if (distanceSq < 0.001f)
                return fromPosition;

            direction *= math.rsqrt(distanceSq); // perf analog of 'direction = math.normalize(direction)'
            return fromPosition + direction * speed * deltaTime;
        }

    }
}