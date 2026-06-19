namespace Game.Framework.Unity.Configs {
    /// <summary>
    /// Interface for configs that require mapping.
    /// </summary>
    /// <remarks>
    /// Mapping occurs at `OnEnable` from ScriptableObject
    /// </remarks>
    public interface IMappedScriptableConfig {
        /// <summary>
        /// Perform mapping process
        /// </summary>
        void Map();
    }
}