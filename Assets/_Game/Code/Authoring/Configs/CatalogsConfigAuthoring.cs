using Game.Configs.Game;
using Game.Ecs._Refactor.Components;
using Game.Framework.Assets;
using Unity.Entities;
using UnityEngine;

namespace Game.Authoring.Configs {
    public sealed class CatalogsConfigAuthoring : MonoBehaviour {
        public ActorCatalogsAsset Actors;

        private sealed class Baker : Baker<CatalogsConfigAuthoring> {
            public override void Bake(CatalogsConfigAuthoring authoring) {
                var prefabs = authoring.Actors;

                DependsOn(prefabs);

                var bufferEntity = GetEntity(TransformUsageFlags.None);
                var buffer = AddBuffer<ActorPrefabElement>(bufferEntity);

                for (var i = 0; i < prefabs.Catalogs.Count; i++) {
                    var catalog = prefabs.Catalogs[i];
                    DependsOn(catalog);
                    AddCatalogToBuffer(catalog, buffer);
                }
            }

            private void AddCatalogToBuffer(AssetCatalog<GameObject> catalog, DynamicBuffer<ActorPrefabElement> buffer) {
                var scopeId = catalog.Scope;
                // Debug.Log($"catalog: {catalog.name} (scope: {scopeId})");
                for (var i = 0; i < catalog.Count; i++) {
                    var (assetId, prefab) = catalog[i];
                    // Debug.Log($"assetId: {assetId}, scopeId: {scopeId}, prefab: {prefab}");

                    buffer.Add(new ActorPrefabElement {
                        AssetId = assetId,
                        Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic),
                        Scope = scopeId
                    });
                }
            }

        }
    }
}