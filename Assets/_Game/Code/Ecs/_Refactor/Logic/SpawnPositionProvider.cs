using Game.Ecs._Refactor.Values;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Ecs._Refactor.Logic {
    internal static class SpawnPositionProvider {

        public static float3 Get(ArenaLocation location, float2 arenaMin, float2 arenaMax, float spawnOffset, ref Random random) {
            switch (location) {
                case ArenaLocation.OutsideArena:
                    return OutsideArena(arenaMin, arenaMax, spawnOffset, ref random);
                default:
                    Debug.LogError("Incorrect arena location");
                    return float3.zero;
            }
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