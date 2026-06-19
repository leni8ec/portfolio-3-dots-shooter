using UnityEngine;

namespace Game.Framework.Unity.Configs {

    /// <summary>
    /// Base class for all unity configs (as ScriptableObject).
    /// </summary>
    /// <remarks>
    /// Implement <see cref="IMappedScriptableConfig"/> interface to use mapping.
    /// <br/>
    /// <br/> It will be mapped on each enable/validate call (in editor and play mode)
    /// </remarks>
    public abstract class ScriptableConfig : ScriptableObject {

        /// <summary>
        /// Perform mapping unity types to app types
        /// </summary>
        private void MapInternal() {
            if (this is IMappedScriptableConfig config) config.Map();
        }

        /// <summary>
        /// Perform mapping when a scriptable object is created
        /// </summary>
        private void OnEnable() {
            MapInternal();
        }


#if UNITY_EDITOR
        /// <summary>
        /// Perform mapping when a scriptable object is edited
        /// </summary>
        private void OnValidate() {
            MapInternal();
        }

#endif

    }
}