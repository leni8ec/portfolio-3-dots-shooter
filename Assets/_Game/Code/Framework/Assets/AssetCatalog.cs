using System;
using System.Collections.Generic;
using Game.Framework.Unity.Configs;
using TriInspector;
using UnityEngine;

namespace Game.Framework.Assets {
    public abstract class AssetCatalog<T> : ScriptableConfig, IMappedScriptableConfig, IAssetCatalog<T> where T : Asset {
        [TableList]
        [SerializeField] private List<Entry> assets;

        private readonly Dictionary<T, GameObject> map = new();

        public void Map() {
            foreach (var entry in assets)
                map[entry.Asset] = entry.Prefab;
        }

        public GameObject Get(T asset) =>
            map.TryGetValue(asset, out var prefab)
                ? prefab
                : throw new NullReferenceException($"Prefab is not found for '{asset}'");


        [Serializable]
        public struct Entry {
            public T Asset;
            public GameObject Prefab;
        }
    }

}