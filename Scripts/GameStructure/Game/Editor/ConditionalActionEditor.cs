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

using GameFramework.EditorExtras.Editor;
using GameFramework.GameStructure.Game.Components;
using GameFramework.GameStructure.Game.Editor.GameActions;
using GameFramework.GameStructure.Game.Editor.GameConditions.Common;
using GameFramework.Helper;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Game.Editor
{
    [CustomEditor(typeof(ConditionalAction))]
    public class ConditionalActionEditor : UnityEditor.Editor
    {
        SerializedProperty _conditionReferencesProperty;
        SerializedProperty _actionReferencesConditionMetProperty;
        SerializedProperty _actionReferencesConditionMetCallbackProperty;
        SerializedProperty _actionReferencesConditionNotMetProperty;
        SerializedProperty _actionReferencesConditionNotMetCallbackProperty;

        ConditionalAction conditionalAction;

        List<ClassDetailsAttribute> _gameConditionClassDetails;
        List<ClassDetailsAttribute> _gameActionClassDetails;
        Rect _mainHelpRect;

        GameConditionEditor[] conditionEditors;
        GameActionEditor[] actionEditorsMet;
        GameActionEditor[] actionEditorsNotMet;

        protected virtual void OnEnable()
        {
            conditionalAction = (ConditionalAction)target;

            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _conditionReferencesProperty = serializedObject.FindProperty("_conditionReferences");
            _actionReferencesConditionMetProperty = serializedObject.FindProperty("_actionReferencesConditionMet");
            _actionReferencesConditionMetCallbackProperty = serializedObject.FindProperty("_actionReferencesConditionMetCallback");
            _actionReferencesConditionNotMetProperty = serializedObject.FindProperty("_actionReferencesConditionNotMet");
            _actionReferencesConditionNotMetCallbackProperty = serializedObject.FindProperty("_actionReferencesConditionNotMetCallback");

            // setup actions types
            _gameActionClassDetails = GameActionEditorHelper.FindTypesClassDetails();
            _gameConditionClassDetails = GameConditionEditorHelper.FindTypesClassDetails();
        }

        protected void OnDisable()
        {
            EditorHelper.CleanupSubEditors(actionEditorsMet);
            EditorHelper.CleanupSubEditors(actionEditorsNotMet);
            EditorHelper.CleanupSubEditors(conditionEditors);
            actionEditorsMet = null;
            actionEditorsNotMet = null;
            conditionEditors = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGUI();
            serializedObject.ApplyModifiedProperties();
        }


        protected void DrawGUI()
        {
            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.ConditionalActionEditor", "With this component you can configure actions that should be taken based upon different conditions that might occur.\nMore conditions and actions will come over time - if there is something you are missing then let us know. Alternatively easily create your own custom actions or add a callback .", _mainHelpRect);

            GameConditionEditorHelper.DrawConditions(serializedObject, _conditionReferencesProperty, conditionalAction.ConditionReferences,
                ref conditionEditors, _gameConditionClassDetails, "Conditions", "Conditions that should be tested.");

            GameActionEditorHelper.DrawActions(serializedObject, _actionReferencesConditionMetProperty, conditionalAction.ActionReferencesConditionMet,
                ref actionEditorsMet, _gameActionClassDetails, _actionReferencesConditionMetCallbackProperty, heading: "Condition Met", tooltip: "Actions that will be run when the condition is met");

            GameActionEditorHelper.DrawActions(serializedObject, _actionReferencesConditionNotMetProperty, conditionalAction.ActionReferencesConditionNotMet,
                ref actionEditorsNotMet, _gameActionClassDetails, _actionReferencesConditionNotMetCallbackProperty, heading: "Condition Not Met", tooltip: "Actions that will be run when the condition is not met");
        }
    }
}
