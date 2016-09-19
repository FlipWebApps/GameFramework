using FlipWebApps.GameFramework.Scripts.EditorExtras.Editor;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Preferences;
using UnityEditor;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Editor
{
    [CustomEditor(typeof(GameItemExtension))]
    public class GameItemExtensionInspector : UnityEditor.Editor
    {
        GameItemExtension _gameItemExtension;
        SerializedProperty _giNameProperty;
        SerializedProperty _giDescriptionProperty;
        SerializedProperty _giValueToUnlockProperty;
        void OnEnable()
        {
            _gameItemExtension = (GameItemExtension)target;
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _giNameProperty = serializedObject.FindProperty("_name");
            _giDescriptionProperty = serializedObject.FindProperty("_description");
            _giValueToUnlockProperty = serializedObject.FindProperty("_valueToUnlock");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            serializedObject.Update();

            DrawGameItemExtension();
            serializedObject.ApplyModifiedProperties();
        }

        public void DrawGameItemExtension()
        {
            EditorGUILayout.LabelField("Game Item Extension", EditorStyles.boldLabel);
            // Game Item setup
            EditorGUILayout.BeginVertical("Box");
                EditorGUI.indentLevel += 1;
                    EditorGUILayout.PropertyField(_giNameProperty, new GUIContent("Name"));
                    EditorGUILayout.PropertyField(_giDescriptionProperty, new GUIContent("Description"));
                    EditorGUILayout.PropertyField(_giValueToUnlockProperty, new GUIContent("Value to Unlock"));
                EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }
    }
}
