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

using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.GenericGameItems.Components;
using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.Editor
{
    [CustomEditor(typeof(GenericGameItemManager))]
    public class GenericGameItemManagerInspector : UnityEditor.Editor
    {
        GenericGameItemManager _genericGameItemManager;

        SerializedProperty _autoCreateItemsProperty;
        SerializedProperty _numberOfItemsProperty;
        SerializedProperty _loadFromResources;
        SerializedProperty _coinsToUnlockProperty;
        SerializedProperty _unlockModeProperty;

        void OnEnable()
        {
            _genericGameItemManager = (GenericGameItemManager)target;

            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _autoCreateItemsProperty = serializedObject.FindProperty("AutoCreateItems");
            _numberOfItemsProperty = serializedObject.FindProperty("NumberOfItems");
            _loadFromResources = serializedObject.FindProperty("LoadFromResources");
            _unlockModeProperty = serializedObject.FindProperty("UnlockMode");
            _coinsToUnlockProperty = serializedObject.FindProperty("CoinsToUnlock");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            serializedObject.Update();

            DrawGameStructure();

            serializedObject.ApplyModifiedProperties();


        }


        void DrawGameStructure()
        {
            // Standalone GenericGameItem setup
            EditorGUILayout.PropertyField(_autoCreateItemsProperty);
            if (_genericGameItemManager.AutoCreateItems)
            {
                EditorGUILayout.PropertyField(_numberOfItemsProperty, new GUIContent("Count"));
                EditorGUILayout.PropertyField(_loadFromResources, new GUIContent("Load From Resources"));
                EditorGUILayout.PropertyField(_unlockModeProperty, new GUIContent("Unlock Mode"));
                if (_genericGameItemManager.UnlockMode == GameItem.UnlockModeType.Coins)
                    EditorGUILayout.PropertyField(_coinsToUnlockProperty);
            }
        }
    }
}