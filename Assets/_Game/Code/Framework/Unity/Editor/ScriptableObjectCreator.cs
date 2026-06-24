using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Game.Framework.Unity.Attributes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Framework.Unity.Editor {

    public static class ScriptableObjectCreator {
        // Shortcut: Alt + Shift + C
        [MenuItem("Assets/Create/ScriptableObject &#c", false, -1000)]
        public static void Open() {
            var provider = ScriptableObject.CreateInstance<ScriptableObjectCreatorWindow>();

            var pos = Event.current != null
                ? GUIUtility.GUIToScreenPoint(Event.current.mousePosition)
                : EditorGUIUtility.GetMainWindowPosition().center - new Vector2(150f, 200f);

            SearchWindow.Open(new SearchWindowContext(pos), provider);
        }
    }

    public class ScriptableObjectCreatorWindow : ScriptableObject, ISearchWindowProvider {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
            var tree = new List<SearchTreeEntry> {
                new SearchTreeGroupEntry(new GUIContent("Create ScriptableObject"), 0)
            };

            var types = TypeCache.GetTypesWithAttribute<CreateScriptableObjectAssetAttribute>()
                .Where(t => t.IsSubclassOf(typeof(ScriptableObject)) && !t.IsAbstract)
                .OrderBy(t => t.Name);

            foreach (var type in types) {
                var attribute = (CreateScriptableObjectAssetAttribute) Attribute.GetCustomAttribute(type, typeof(CreateScriptableObjectAssetAttribute));
                var menuPath = attribute?.MenuPath;
                var friendlyName = AddSpacesBeforeUppercaseLetters(type.Name);

                if (!string.IsNullOrEmpty(menuPath)) {
                    var parts = menuPath.Split('/');
                    for (var i = 0; i < parts.Length; i++) {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(parts[i]), i + 1));
                    }
                    tree.Add(new SearchTreeEntry(new GUIContent(friendlyName)) { userData = type, level = parts.Length + 1 });
                } else {
                    tree.Add(new SearchTreeEntry(new GUIContent(friendlyName)) { userData = type, level = 1 });
                }
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            if (searchTreeEntry.userData is Type type) {
                CreateAsset(type);
                return true;
            }
            return false;
        }

        private void CreateAsset(Type type) {
            var path = "Assets";
            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
                path = AssetDatabase.GetAssetPath(obj);
                if (!Directory.Exists(path)) path = Path.GetDirectoryName(path);
                break;
            }

            var cleanName = GetCleanName(type.Name);
            var asset = CreateInstance(type);
            var assetPath = AssetDatabase.GenerateUniqueAssetPath($"{path}/{cleanName}.asset");

            ProjectWindowUtil.CreateAsset(asset, assetPath);
        }

        private static string GetCleanName(string originalName) {
            var result = originalName;
            if (result.EndsWith("Asset")) result = result[..^5];
            return result;
        }

        /// <summary>Adds spaces between words before uppercase letters</summary>
        public static string AddSpacesBeforeUppercaseLetters(string input) =>
            string.IsNullOrWhiteSpace(input) ? input : Regex.Replace(input, @"(?<!^)(?=[A-Z])", " ");

        /// <summary>Adds a space before the last uppercase letter</summary>
        public static string AddSpaceBeforeLastUppercaseLetter(string input) =>
            string.IsNullOrWhiteSpace(input) ? input : Regex.Replace(input, @"(?<!^)(?=[A-Z][^A-Z]*$)", " ");

    }
}