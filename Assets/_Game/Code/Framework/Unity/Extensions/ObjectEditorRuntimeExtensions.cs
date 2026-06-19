using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Game.Framework.Unity.Extensions {
    /// <summary>
    /// Runtime-safe Unity.Object editor extensions that act as a no-op in build.
    /// </summary>
    public static class ObjectEditorRuntimeExtensions {
        /// <summary>
        /// Marks the object as dirty to be saved by Unity.
        /// </summary>
        public static void MarkDirty(this Object obj) {
#if UNITY_EDITOR
            if (!obj)
                throw new ArgumentNullException(nameof(obj));

            EditorUtility.SetDirty(obj);
#endif
        }

        /// <summary>
        /// Marks the object as dirty and forces an immediate save to disk.
        /// </summary>
        public static void MarkDirtyAndSave(this Object obj) {
#if UNITY_EDITOR
            if (!obj)
                throw new ArgumentNullException(nameof(obj));

            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssetIfDirty(obj);
#endif
        }
    }
}