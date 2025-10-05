using UnityEngine.UIElements;

namespace Calluna.SceneManagement.Editor
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SceneChange))]
    public class SceneChangeDrawer : PropertyDrawer
    {
        private const string _sceneBacking = "<Scene>k__BackingField";
        private const string modeBacking = "<Mode>k__BackingField";

        private static string[] _sceneNames;
        private static double _lastRefresh;
        private const string _noneOption = "(None)";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            RefreshSceneNames();
            return base.CreatePropertyGUI(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Wrap in property scope for prefab override handling, etc.
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Find the serialized backing fields
            var sceneProp = property.FindPropertyRelative(_sceneBacking);
            var modeProp = property.FindPropertyRelative(modeBacking);

            if (sceneProp == null || modeProp == null)
            {
                EditorGUI.HelpBox(position,
                    "Could not find backing fields for Scene/Mode. Make sure the members use [field: SerializeField].",
                    MessageType.Error);
                EditorGUI.EndProperty();
                return;
            }

            // Layout: two lines (Scene popup, Mode field)
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var vPad = EditorGUIUtility.standardVerticalSpacing;

            var headerRect = new Rect(position.x, position.y, position.width, lineHeight);
            var sceneRect = new Rect(position.x, headerRect.yMax + vPad, position.width, lineHeight);
            var modeRect = new Rect(position.x, sceneRect.yMax + vPad, position.width, lineHeight);

            // Ensure we have a fresh list (refresh occasionally to reflect Build Settings changes)

            // Current index
            var names = _sceneNames ?? Array.Empty<string>();
            int currentIndex = Mathf.Max(0, Array.IndexOf(names, sceneProp.stringValue));
            if (currentIndex == -1) currentIndex = 0; // fallback to None

            // Draw header
            EditorGUI.LabelField(headerRect, GetHeader(sceneProp.stringValue, (SceneChangeType)modeProp.intValue),
                EditorStyles.boldLabel);

            // Draw popup
            int newIndex = EditorGUI.Popup(sceneRect, "Scene", currentIndex, names);
            string chosen = names.Length > 0 ? names[Mathf.Clamp(newIndex, 0, names.Length - 1)] : _noneOption;
            sceneProp.stringValue = chosen == _noneOption ? string.Empty : chosen;

            // Draw Mode (enum) as usual
            EditorGUI.PropertyField(modeRect, modeProp, new GUIContent("Mode"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // two lines + spacing
            var lh = EditorGUIUtility.singleLineHeight;
            var v = EditorGUIUtility.standardVerticalSpacing;
            return lh * 3 + v * 2;
        }

        private static void RefreshSceneNames()
        {
            // Throttle refresh a bit so we don't rebuild every repaint
            if (_sceneNames != null && EditorApplication.timeSinceStartup - _lastRefresh < 1.0f) return;

            var scenes = EditorBuildSettings.scenes;
            var names = scenes
                .Where(s => s.enabled)
                .Select(s => System.IO.Path.GetFileNameWithoutExtension(s.path))
                .Distinct()
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToList();

            names.Insert(0, _noneOption);
            _sceneNames = names.ToArray();
            _lastRefresh = EditorApplication.timeSinceStartup;
        }

        private string GetHeader(string sceneName, SceneChangeType sceneChangeType)
        {
            if (string.IsNullOrEmpty(sceneName) || sceneChangeType <= 0)
            {
                return "<color=#FF0000>Missing scene change update</color>";
            }

            switch (sceneChangeType)
            {
                case SceneChangeType.Load:
                    return $"Load single scene '{sceneName}'";
                case SceneChangeType.LoadAdditive:
                    return $"Load scene '{sceneName}' additive";
                case SceneChangeType.Unload:
                    return $"Unload scene '{sceneName}'";
            }

            return string.Empty;
        }
    }
}