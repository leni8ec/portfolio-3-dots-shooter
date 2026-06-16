using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Game.Ecs._Refactor.Logic {
    internal static class Arena2D {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInside(float3 position, float2 arenaMin, float2 arenaMax) {
            return position.x >= arenaMin.x &&
                   position.x <= arenaMax.x &&
                   position.z >= arenaMin.y &&
                   position.z <= arenaMax.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInside(float2 position, float2 arenaMin, float2 arenaMax) {
            return position.x >= arenaMin.x &&
                   position.x <= arenaMax.x &&
                   position.y >= arenaMin.y &&
                   position.y <= arenaMax.y;
        }

    }
}