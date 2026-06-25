using Game.Ecs._Refactor.Values;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

// todo: naming..
namespace Game.Ecs._Refactor.Logic {
    internal static class LocationPositionProvider {

        public static float3 Get(ArenaLocation location, float2 arenaMin, float2 arenaMax, float spawnOffset, ref Random random, float3 relatedPosition = default) {
            switch (location) {
                case ArenaLocation.Related:
                    return relatedPosition;
                case ArenaLocation.InsideArena:
                    return InsideArena(arenaMin, arenaMax, spawnOffset, ref random);
                case ArenaLocation.OutsideArena:
                    return OutsideArena(arenaMin, arenaMax, spawnOffset, ref random);
                default:
                    Debug.LogError("Incorrect arena location");
                    return float3.zero;
            }
        }

        /// <returns> Position inside arena, shrunk by the spawn offset</returns>
        private static float3 InsideArena(float2 arenaMin, float2 arenaMax, float spawnOffset, ref Random random) {
            return new float3(
                random.NextFloat(arenaMin.x + spawnOffset, arenaMax.x - spawnOffset),
                0,
                random.NextFloat(arenaMin.y + spawnOffset, arenaMax.y - spawnOffset));
        }

        private static float3 OutsideArena(float2 arenaMin, float2 arenaMax, float spawnOffset, ref Random random) {
            var pos = default(float3);
            var side = random.NextInt(0, 4);
            switch (side) {
                case 0:
                    pos.x = random.NextFloat(arenaMin.x, arenaMax.x);
                    pos.z = arenaMax.y + spawnOffset;
                    break;

                case 1:
                    pos.x = random.NextFloat(arenaMin.x, arenaMax.x);
                    pos.z = arenaMin.y - spawnOffset;
                    break;

                case 2:
                    pos.x = arenaMin.x - spawnOffset;
                    pos.z = random.NextFloat(arenaMin.y, arenaMax.y);
                    break;

                default:
                    pos.x = arenaMax.x + spawnOffset;
                    pos.z = random.NextFloat(arenaMin.y, arenaMax.y);
                    break;
            }

            return pos;
        }

    }
}