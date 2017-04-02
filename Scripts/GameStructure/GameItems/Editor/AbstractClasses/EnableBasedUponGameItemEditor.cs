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
using GameFramework.EditorExtras.Editor;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor.AbstractClasses
{
    public abstract class EnableBasedUponGameItemEditor
    {
        public const float RemoveButtonWidth = 30f;
    }

    public abstract class EnableBasedUponGameItemEditor<T> : UnityEditor.Editor where T : GameItem, new()
    {
        SerializedProperty _contextProperty;
        SerializedProperty _enableModeProperty;
        SerializedProperty _conditionMetGameObjectProperty;
        SerializedProperty _conditionNotMetGameObjectProperty;
        SerializedProperty _conditionsProperty;
        SerializedProperty _gameObjectToGameObjectAnimationProperty;

        EnableBasedUponGameItem<T> _gameItemEditor;

        readonly string[] _boolOptions = { "False", "True" };

        readonly string[] _conditionNames = Enum.GetNames(typeof(EnableBasedUponGameItem<T>.ConditionTypes));
        readonly int[] _conditionValues = (int[])Enum.GetValues(typeof(EnableBasedUponGameItem<T>.ConditionTypes));
        readonly string[] _conditionTooltips = {
            "Whether this GameItem can be unlocked by coins",
            "Whether this GameItem can be unlocked by completion",
            "Whether this GameItem can be unlocked by payment",
            "Compare the GameItems coins with a specified value",
            "Whether the current player can unlock this GameItem (combine if needed with Unlocked condition)",
            "Compare the GameItems score with a specified value",
            "Whether this GameItem is selected",
            "Whether this GameItem is unlocked",
            "A custom Condition that you should add a reference to"
        };

        public void OnEnable()
        {
            _gameItemEditor = (EnableBasedUponGameItem<T>) target;

            _contextProperty = serializedObject.FindProperty("_context");
            _enableModeProperty = serializedObject.FindProperty("EnableMode");
            _conditionMetGameObjectProperty = serializedObject.FindProperty("ConditionMetGameObject");
            _conditionNotMetGameObjectProperty = serializedObject.FindProperty("ConditionNotMetGameObject");
            _conditionsProperty = serializedObject.FindProperty("ConditionReferences");
            _gameObjectToGameObjectAnimationProperty = serializedObject.FindProperty("GameObjectToGameObjectAnimation");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

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

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Animation", "Animate gameobject changes."), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_gameObjectToGameObjectAnimationProperty);

            // Display all items - use a for loop rather than a foreach loop in case of deletion.
            //EditorGUILayout.PropertyField(_conditionsProperty, true);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Conditions", "The different conditions that this component should react to."), EditorStyles.boldLabel);
            for (var i = 0; i < _gameItemEditor.ConditionReferences.Length; i++)
            {
                // draw built in editor 
                EditorGUILayout.BeginHorizontal(GUI.skin.box);

                var conditionReference = _conditionsProperty.GetArrayElementAtIndex(i);
                var conditionType = (EnableBasedUponGameItem<T>.ConditionTypes)_gameItemEditor.ConditionReferences[i].Identifier;
                EditorGUILayout.PrefixLabel(new GUIContent(EditorHelper.PrettyPrintCamelCase(_conditionNames[(int)conditionType]), _conditionTooltips[(int)conditionType]));
                switch (conditionType) {
                    case EnableBasedUponGameItem<T>.ConditionTypes.CanUnlockWithCoins:
                    case EnableBasedUponGameItem<T>.ConditionTypes.CanUnlockWithCompletion:
                    case EnableBasedUponGameItem<T>.ConditionTypes.CanUnlockWithPayment:
                    case EnableBasedUponGameItem<T>.ConditionTypes.PlayerHasCoinsToUnlock:
                    case EnableBasedUponGameItem<T>.ConditionTypes.Selected:
                    case EnableBasedUponGameItem<T>.ConditionTypes.Unlocked:
                        var conditionBoolProperty = conditionReference.FindPropertyRelative("_boolValue");
                        conditionBoolProperty.boolValue = EditorGUILayout.Popup(conditionBoolProperty.boolValue ? 1 : 0, _boolOptions, GUILayout.ExpandWidth(true)) == 1;
                        break;
                    case EnableBasedUponGameItem<T>.ConditionTypes.Coins:
                    case EnableBasedUponGameItem<T>.ConditionTypes.Score:
                        var comparisonProperty = conditionReference.FindPropertyRelative("_comparison");
                        var conditionIntProperty = conditionReference.FindPropertyRelative("_intValue");
                        EditorGUILayout.PropertyField(comparisonProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                        EditorGUILayout.PropertyField(conditionIntProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                        break;
                    case EnableBasedUponGameItem<T>.ConditionTypes.Custom:
                        var scriptableObjectProperty = conditionReference.FindPropertyRelative("_scriptableObject");
                        EditorGUILayout.PropertyField(scriptableObjectProperty, GUIContent.none, GUILayout.ExpandWidth(true));
                        break;
                    default:
                        Debug.LogError("Unknown built in type : " + _gameItemEditor.ConditionReferences[i].Identifier);
                        break;
                }
                if (GUILayout.Button("-", GUILayout.Width(EnableBasedUponGameItemEditor.RemoveButtonWidth)))
                {
                    _conditionsProperty.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            //            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus More", "Add to list"), EditorStyles.miniButton))
            if (GUILayout.Button(new GUIContent("Add", "Add a new condition to the list"), EditorStyles.miniButton))
            {
                var menu = new GenericMenu();
                for (var i = 0; i < _conditionNames.Length; i++)
                {
                    menu.AddItem(new GUIContent(_conditionNames[i]), false, AddCondition,
                        _conditionValues[i]);
                }
                menu.ShowAsContext();
            }

            serializedObject.ApplyModifiedProperties();
        }


        void AddCondition(object conditionValue)
        {
            var conditionType = (EnableBasedUponGameItem<T>.ConditionTypes)conditionValue;

            _conditionsProperty.arraySize++;
            var newElement = _conditionsProperty.GetArrayElementAtIndex(_conditionsProperty.arraySize - 1);
            var propName = newElement.FindPropertyRelative("_identifier");
            propName.intValue = (int)conditionType;
            var propUseScriptableObject = newElement.FindPropertyRelative("_useScriptableObject");
            if (conditionType == EnableBasedUponGameItem<T>.ConditionTypes.Custom)
            {
                propUseScriptableObject.boolValue = true;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
