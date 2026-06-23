using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Ecs._Refactor.Utils {
    /// <summary>
    /// ScopedMap[scope][key]
    /// </summary>
    public struct ScopedNativeParallelHashMap<TKey, TValue> : INativeDisposable
        where TKey : unmanaged, IEquatable<TKey>
        where TValue : unmanaged {

        [SerializeField]
        private NativeParallelHashMap<ScopedKey, TValue> map;

        public ScopedNativeParallelHashMap(int capacity, AllocatorManager.AllocatorHandle allocator) : this() {
            map = new NativeParallelHashMap<ScopedKey, TValue>(capacity, allocator);
        }

        public void Dispose() => map.Dispose();
        public JobHandle Dispose(JobHandle inputDeps) => map.Dispose(inputDeps);

        public bool IsCreated => map.IsCreated;
        public int Count() => map.Count();
        public void Clear() => map.Clear();

        public bool ContainsKey(TKey key, TKey scope) =>
            map.ContainsKey(new ScopedKey(scope, key));

        public TValue Get(TKey key, TKey scope) =>
            map[new ScopedKey(scope, key)];

        public bool TryGet(TKey key, TKey scope, out TValue value) =>
            map.TryGetValue(new ScopedKey(scope, key), out value);

        // todo: find ways to optimize for arrays addition
        public bool TryAdd(TKey key, TValue value, TKey scope) =>
            map.TryAdd(new ScopedKey(scope, key), value);

        public void Add(TKey key, TValue value, TKey scope) {
            if (!map.TryAdd(new ScopedKey(scope, key), value))
                Debug.LogError($"Key ({key.ToString()}) already exists in scope ({scope.ToString()})");
        }


        public readonly struct ScopedKey : IEquatable<ScopedKey> {
            private readonly TKey scope;
            private readonly TKey key;

            public ScopedKey(TKey scope, TKey key) {
                this.scope = scope;
                this.key = key;
            }

            public override string ToString()
                => $"Scope: {scope}, Key: {key}";

            public bool Equals(ScopedKey other)
                => scope.Equals(other.scope) && key.Equals(other.key);

            // rough-like solution for deterministic hash
            public override int GetHashCode() {
                unchecked {
                    return (scope.GetHashCode() * 397) ^ key.GetHashCode();
                }
            }
        }

    }
}