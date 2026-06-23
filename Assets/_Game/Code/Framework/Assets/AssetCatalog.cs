using System;
using System.Collections.Generic;
using Game.Framework.Unity.Configs;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Framework.Assets {
    ///<remarks> Duck-typed foreach (read-only) </remarks>
    public abstract class AssetCatalog<TAsset> : ScriptableConfig where TAsset : Object {

        public abstract IdentityAsset Scope { get; }

        public struct CatalogEntry {
            public Identity Identity;
            public TAsset Prefab;

            public void Deconstruct(out Identity identity, out TAsset prefab) {
                identity = Identity;
                prefab = Prefab;
            }
        }

        public Dictionary<Identity, TAsset> ToDictionary() {
            var dictionary = new Dictionary<Identity, TAsset>();
            for (var i = 0; i < this.Count; i++) {
                var entry = this[i];
                if (!dictionary.TryAdd(entry.Identity, entry.Prefab))
                    Debug.LogError($"Duplicate asset id {entry.Identity.ToFixedString()}");
            }
            return dictionary;
        }

        public abstract int Count { get; }
        public abstract CatalogEntry this[int index] { get; }

        public Enumerator GetEnumerator() => new(this);

        public struct Enumerator {
            private readonly AssetCatalog<TAsset> catalog;
            private int index;

            public Enumerator(AssetCatalog<TAsset> catalog) {
                this.catalog = catalog;
                index = -1;
            }

            public bool MoveNext() => ++index < catalog.Count;

            public CatalogEntry Current => catalog[index];
        }
    }

    public class AssetCatalog<TIdentityAsset, TAsset, TScopeAsset> : AssetCatalog<TAsset>
        where TIdentityAsset : IdentityAsset
        where TAsset : Object
        where TScopeAsset : IdentityAsset {

        [PropertySpace]
        [SerializeField] private TScopeAsset scope;
        public override IdentityAsset Scope => scope;

        [TableList(AlwaysExpanded = true)] [PropertySpace]
        [SerializeField] private List<ListEntry> entries = new();

        public override int Count => entries.Count;

        public override CatalogEntry this[int index] => (CatalogEntry) entries[index];

        [Serializable]
        internal struct ListEntry {
            public TIdentityAsset IdentityAsset;
            public TAsset Prefab;

            public static explicit operator CatalogEntry(ListEntry listEntry) =>
                new() {
                    Identity = listEntry.IdentityAsset.AsIdentity(),
                    Prefab = listEntry.Prefab
                };
        }
    }
}