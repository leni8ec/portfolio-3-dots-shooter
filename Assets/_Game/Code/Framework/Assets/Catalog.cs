using System;
using System.Collections.Generic;
using Game.Framework.Unity.Configs;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Framework.Assets {
    // todo: not used (remove?)
    public abstract class Catalog<TKey, TValue> : ScriptableConfig, IMappedScriptableConfig, ICatalog<TKey, TValue>
        where TValue : Object {

        [TableList(AlwaysExpanded = true)] [PropertySpace]
        [SerializeField] private List<Entry> assets;

        private readonly Dictionary<TKey, TValue> map = new();

        public void Map() {
            foreach (var entry in assets)
                map[entry.Key] = entry.Asset;
        }

        public TValue Get(TKey asset) =>
            map.TryGetValue(asset, out var prefab)
                ? prefab
                : throw new NullReferenceException($"Prefab is not found for '{asset}'");


        [Serializable]
        public struct Entry {
            public TKey Key;
            public TValue Asset;
        }
    }

}