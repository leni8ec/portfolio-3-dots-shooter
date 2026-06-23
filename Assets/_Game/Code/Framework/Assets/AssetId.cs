using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.Collections;

// todo: AssetId
//  - rename to `Identity` or `Token` or `IdentityToken`
//  - use `byte` as `id` for release builds
//  - make generic to use compile time specifications (ex: AssetId<Unit>, AssetId<Ammo>), like `IdentityAsset` authoring
//  - make `value` as private with public property?
//  - use combined `ushort ` value with a scope? (scope: value >> 8)
namespace Game.Framework.Assets {
    /// <summary>
    /// Contains fixed string with 32 bytes
    /// </summary>
    /// <remarks>
    /// Holds 29 ASCII symbols (32 - 3 for internal usages)
    /// </remarks>
    [SuppressMessage("ReSharper", "StructCanBeMadeReadOnly")]
    public struct AssetId : IEquatable<AssetId> {

        public FixedString32Bytes Value;

        public AssetId(FixedString32Bytes value) =>
            Value = value;

        public AssetId(string value) =>
            Value = (FixedString32Bytes) value;

        public static AssetId Combine(AssetId left, AssetId right) =>
            left.Combine(right);

        public readonly FixedString32Bytes ToFixedString() => Value;

        public override string ToString() => Value.ToString();

        [SuppressMessage("ReSharper", "Unity.BurstLoadingManagedType")]
        public override bool Equals(object obj) => obj is AssetId other && Equals(other);

        public bool Equals(AssetId other) => Value.Equals(other.Value);

        public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
        public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right);

        public override int GetHashCode() => (int) GetDeterministicHash(Value);

        /// <summary>
        /// Computes a deterministic 32-bit FNV-1a hash from the FixedString32Bytes value.
        /// </summary>
        /// <remarks>
        /// This method provides zero-allocation (no GC) and is fully compatible with Unity Burst Compiler.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
        private static uint GetDeterministicHash(FixedString32Bytes value) {
            uint hash = 2166136261;

            var byteList = value.AsFixedList();
            int length = byteList.Length;

            for (int i = 0; i < length; i++) {
                hash ^= byteList[i];
                hash *= 16777619;
            }

            return hash;
        }
    }
}