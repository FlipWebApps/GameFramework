//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework.EditorExtras.Editor;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel.Conditions;
using GameFramework.GameStructure.GameItems.Editor.Conditions;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.Components;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor.AbstractClasses
{
    public abstract class EnableBasedUponGameItemEditor<T> : UnityEditor.Editor where T : GameItem, new()
    {
        SerializedProperty _contextProperty;
        SerializedProperty _enableModeProperty;
        SerializedProperty _conditionMetGameObjectProperty;
        SerializedProperty _conditionNotMetGameObjectProperty;
        SerializedProperty _conditionsProperty;

        EnableBasedUponGameItem<T> _gameItemEditor;

        // types and associated names for all conditions
        Type[] _conditionTypes;
        string[] _conditionTypeNames;

        protected ConditionEditor[] subEditors;         // Array of Editors nested within this Editor.


        public void OnEnable()
        {
            _gameItemEditor = (EnableBasedUponGameItem<T>) target;

            _contextProperty = serializedObject.FindProperty("_context");
            _enableModeProperty = serializedObject.FindProperty("EnableMode");
            _conditionMetGameObjectProperty = serializedObject.FindProperty("ConditionMetGameObject");
            _conditionNotMetGameObjectProperty = serializedObject.FindProperty("ConditionNotMetGameObject");
            _conditionsProperty = serializedObject.FindProperty("ConditionReferences");

            // If new editors are required create them.
            CheckAndCreateSubEditors();

            FindConditions();
        }

        protected void OnDisable()
        {
            CleanupSubEditors();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // If new editors are required create them.
            CheckAndCreateSubEditors();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_contextProperty);
            EditorGUILayout.PropertyField(_enableModeProperty);
            if (_enableModeProperty.enumValueIndex == 0)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_conditionMetGameObjectProperty);
                EditorGUILayout.PropertyField(_conditionNotMetGameObjectProperty);
                EditorGUI.indentLevel--;
            }
            //EditorGUILayout.PropertyField(_conditionsProperty, true);

            // Display all the sub editors - use for rather than foreach loop in case of deletion.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Conditions", "The different conditions that this component should react to."), EditorStyles.boldLabel);
            for (var i = 0; i < subEditors.Length; i++)
            {
                subEditors[i].OnInspectorGUI();
            }

            //            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus More", "Add to list"), EditorStyles.miniButton))
            if (GUILayout.Button(new GUIContent("Add", "Add a new condition to the list"), EditorStyles.miniButton))
            {
                var menu = new GenericMenu();
                for (var i = 0; i < _conditionTypes.Length; i++)
                {
                    menu.AddItem(new GUIContent(_conditionTypeNames[i]), false, AddConditionMenuHandler,
                        _conditionTypes[i]);
                }
                menu.ShowAsContext();
            }

            serializedObject.ApplyModifiedProperties();
        }


        void AddConditionMenuHandler(object target)
        {
            var type = (Type)target;

            _conditionsProperty.arraySize++;
            var newElement = _conditionsProperty.GetArrayElementAtIndex(_conditionsProperty.arraySize - 1);
            var propName = newElement.FindPropertyRelative("_className");
            propName.stringValue = type.Name;

            serializedObject.ApplyModifiedProperties();
        }


        #region SubEditors

        /// <summary>
        /// In case of changes create sub editors
        /// </summary>
        protected void CheckAndCreateSubEditors()
        {
            // If there are the correct number of subEditors then do nothing.
            if (subEditors != null && subEditors.Length == _gameItemEditor.ConditionReferences.Length)
                return;

            // Otherwise get rid of the editors.
            CleanupSubEditors();

            // Create an array of the subEditor type that is the right length for the targets.
            subEditors = new ConditionEditor[_gameItemEditor.ConditionReferences.Length];

            // Populate the array and setup each Editor.
            for (var i = 0; i < subEditors.Length; i++)
            {
                subEditors[i] = CreateEditor(_gameItemEditor.ConditionReferences[i].Condition) as ConditionEditor;
                subEditors[i].ParentCollectionProperty = _conditionsProperty;
                subEditors[i].DataProperty = _conditionsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_data"); ;
                //subEditors[i].ParentTarget = serializedObject;
                subEditors[i].index = i;
            }
        }

        /// <summary>
        /// Destroy all subeditors
        /// </summary>
        void CleanupSubEditors()
        {
            if (subEditors == null) return;
            for (var i = 0; i < subEditors.Length; i++)
            {
                DestroyImmediate(subEditors[i]);
            }
            // Null the array so it's GCed.
            subEditors = null;
        }

        #endregion SubEditors


        /// <summary>
        /// This method will use reflection for finding all conditions and add them to the list dynamically.
        /// </summary>
        void FindConditions()
        {
            var conditionType = typeof(Condition);

            // Go through all the types in the Assembly and find non abstract subclasses
            var conditionSubTypeList = new List<Type>();
            var allTypes = conditionType.Assembly.GetTypes();
            foreach (var type in allTypes)
            {
                if (type.IsSubclassOf(conditionType) && !type.IsAbstract)
                {
                    conditionSubTypeList.Add(type);
                }
            }
            _conditionTypes = conditionSubTypeList.ToArray();

            // Get the names of all the types and add those that can process instances of the GameItem this editor represents.
            var gameItemInstance = CreateInstance<T>();
            var conditionTypeNameList = new List<string>();
            foreach (var type in _conditionTypes)
            {
                var conditionInstance = (Condition)CreateInstance(type.Name);
                if (conditionInstance.CanProcessGameItem(gameItemInstance))
                    conditionTypeNameList.Add(EditorHelper.PrettyPrintCamelCase(type.Name));
            } 
            _conditionTypeNames = conditionTypeNameList.ToArray();
        }

    }
}
