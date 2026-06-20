using System;

namespace Game.Framework.Unity.Attributes {
    /// <summary>
    /// Adds this ScriptableObject to the custom creation menu.
    /// <br/> Leave empty for root menu, or set a path with slashes to create folders (e.g. "Actors/Weapons/").
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScriptableObjectAssetAttribute : Attribute {
        public string MenuPath { get; }

        public ScriptableObjectAssetAttribute(string menuPath = "") {
            MenuPath = menuPath;
        }
    }
}