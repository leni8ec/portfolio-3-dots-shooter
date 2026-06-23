using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Game.Framework.Assets {
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    public static class AssetIdExtensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssetId Combine(this AssetId self, AssetId other) {
            const byte separator = (byte) '_';
            const int textBudget = 28; // 29 bytes minus separator and left part
            const int halfBudget = 14; // Half of 29-byte text limit

            var left = self.Value;
            var right = other.Value;

            if (left.Length + right.Length > textBudget) {
                right.Length = math.min(right.Length, textBudget - math.min(left.Length, halfBudget));
                left.Length = math.min(left.Length, textBudget - right.Length);
            }

            FixedString32Bytes result = default;
            result.Append(left);
            result.AppendRawByte(separator);
            result.Append(right);

            return new AssetId(result);
        }

    }
}