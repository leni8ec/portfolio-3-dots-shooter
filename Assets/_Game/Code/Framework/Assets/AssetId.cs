using System;
using System.Diagnostics.CodeAnalysis;
using Unity.Burst;
using Unity.Collections;

namespace Game.Framework.Assets {
    /// <summary>
    /// Contains fixed string with 32 bytes
    /// </summary>
    /// <remarks>
    /// Holds 30 ASCII symbols
    /// </remarks>
    [BurstCompile]
    [SuppressMessage("ReSharper", "StructCanBeMadeReadOnly")]
    public struct AssetId : IEquatable<AssetId> {

        public FixedString32Bytes Value { get; }

        public AssetId(FixedString32Bytes value) {
            Value = value;
        }

        public AssetId(string value) {
            Value = (FixedString32Bytes) value;
        }

        public bool Equals(AssetId other) => Value.Equals(other.Value);
        public override bool Equals(object obj) => obj is AssetId other && Equals(other);

        public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
        public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right);

        public override int GetHashCode() => (int) GetDeterministicHash();

        /// <summary>
        /// Computes a deterministic 32-bit FNV-1a hash from the FixedString32Bytes value.
        /// </summary>
        /// <remarks>
        /// This method provides zero-allocation (no GC) and is fully compatible with Unity Burst Compiler.
        /// </remarks>
        [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
        private uint GetDeterministicHash() {
            uint hash = 2166136261;

            var byteList = Value.AsFixedList();
            int length = byteList.Length;

            for (int i = 0; i < length; i++) {
                hash ^= byteList[i];
                hash *= 16777619;
            }

            return hash;
        }
    }
}