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
using System.Text;
using GameFramework.EditorExtras.Editor;
using GameFramework.Localisation.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Localisation.Editor
{
    [CustomEditor(typeof(LocalisationData))]
    public class LocalisationDataEditor : UnityEditor.Editor
    {
        //GameItem _gameItem;
        SerializedProperty _entriesProperty;
        SerializedProperty _languagesProperty;

        int _currentTab;

        protected virtual void OnEnable()
        {
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _entriesProperty = serializedObject.FindProperty("_localisationEntries");
            _languagesProperty = serializedObject.FindProperty("_languages");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawGUI()
        {
            _currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Entries", "Languages" });
            switch (_currentTab)
            {
                case 0:
                    EditorGUILayout.LabelField("Entries", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox("Use these settings to provide additional customisation for GameItems.\n\nThere are also Extensions for specific GameItems such as Level and Player. In addition you can create your own derived classes to hold custom properties and / or code", MessageType.Info);
                    DrawEntries();
                    break;
                case 1:
                    DrawLanguages();
                    break;
            }
        }

        protected void DrawEntries() { 
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_entriesProperty, true);
            EditorGUILayout.EndVertical();
        }

        protected void DrawLanguages()
        {
            EditorGUILayout.LabelField("Languages", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("The languages that are supported within this file.", MessageType.Info);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_languagesProperty, true);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(new GUIContent("Prefabs", "Here you can add prefabs for use with standard features such as selection screens or for your own custom needs"), EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
        }
    }
}
