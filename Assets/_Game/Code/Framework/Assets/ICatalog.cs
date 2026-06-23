using UnityEngine;

namespace Game.Framework.Assets {
    // todo: not used (remove?)
    public interface ICatalog<in TKey, out TValue>
        where TValue : Object {

        TValue Get(TKey key);

    }
}