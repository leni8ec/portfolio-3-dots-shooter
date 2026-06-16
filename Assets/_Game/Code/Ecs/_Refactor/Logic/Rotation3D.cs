using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Logic {
    // todo: use extensions ?
    internal static class Rotation3D {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion LookRotation2D(float3 fromPosition, float3 toPosition) {
            var direction = toPosition - fromPosition;
            direction.y = 0f;

            return FromDirection(direction);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion FromDirection(float3 direction) {
            direction = math.normalize(direction);
            return quaternion.LookRotation(direction, math.up());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetDirection2D(quaternion rotation) {
            return math.mul(rotation, math.forward());
        }

    }
}