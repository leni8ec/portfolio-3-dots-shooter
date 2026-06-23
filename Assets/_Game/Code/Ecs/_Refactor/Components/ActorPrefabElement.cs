using Game.Framework.Assets;
using Unity.Entities;

namespace Game.Ecs._Refactor.Components {
    public struct ActorPrefabElement : IBufferElementData {
        public Identity Identity;
        public Entity Prefab;
        public Identity Scope;

        public void Deconstruct(out Identity identity, out Entity prefab, out Identity scope) {
            identity = Identity;
            prefab = Prefab;
            scope = Scope;
        }
    }
}