using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Extensions {
    public static class MathFloat3Extensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 WithX(this in float3 value, float x) =>
            new(x, value.y, value.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 WithY(this float3 value, float y) =>
            new(value.x, y, value.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 WithZ(this float3 value, float z) =>
            new(value.x, value.y, z);

    }
}