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
using FlipWebApps.GameFramework.Scripts.EditorExtras.Editor;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Editor
{
    [CustomEditor(typeof(GameItem))]
    public class GameItemEditor : UnityEditor.Editor
    {
        //GameItem _gameItem;
        SerializedProperty _giNameProperty;
        SerializedProperty _giDescriptionProperty;
        SerializedProperty _giValueToUnlockProperty;
        SerializedProperty _giLocalisablePrefabsProperty;
        SerializedProperty _giLocalisableSpritesProperty;

        Rect _prefabDropRect;
        Rect _spriteDropRect;

        protected virtual void OnEnable()
        {
            //_gameItem = (GameItem)target;
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _giNameProperty = serializedObject.FindProperty("_localisableName");
            _giDescriptionProperty = serializedObject.FindProperty("_localisableDescription");
            _giValueToUnlockProperty = serializedObject.FindProperty("_valueToUnlock");
            _giLocalisablePrefabsProperty = serializedObject.FindProperty("_localisablePrefabs");
            _giLocalisableSpritesProperty = serializedObject.FindProperty("_localisableSprites");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawGUI();

            // process drag and drop
            CheckDragAndDrop(_spriteDropRect,
                draggedObject => draggedObject is Sprite,
                draggedObject => AddNewSprite(draggedObject as Sprite));

            CheckDragAndDrop(_prefabDropRect,
                draggedObject => draggedObject is GameObject && PrefabUtility.GetPrefabType(draggedObject) != PrefabType.None,
                draggedObject => AddNewPrefab(draggedObject as GameObject));

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawGUI()
        {
            EditorGUILayout.LabelField("Game Item", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use these settings to provide additional customisation for GameItems.\n\nThere are also Extensions for specific GameItems such as Level and Player. In addition you can create your own derived classes to hold custom properties and / or code", MessageType.Info);
            DrawProperties();
        }

        protected void DrawProperties()
        {
            DrawBasicProperties();
            DrawPrefabs();
            DrawSprites();
        }


        protected void DrawBasicProperties()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Basic Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_giNameProperty, new GUIContent("Name"));
            EditorGUILayout.PropertyField(_giDescriptionProperty, new GUIContent("Description"));
            EditorGUILayout.PropertyField(_giValueToUnlockProperty);
            EditorGUILayout.EndVertical();
        }

        protected void DrawPrefabs()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(new GUIContent("Prefabs", "Here you can add prefabs for use with standard features such as selection screens or for your own custom needs"), EditorStyles.boldLabel);

            _prefabDropRect = DrawDropRect("Drag a Prefab here to create a new entry");

            if (_giLocalisablePrefabsProperty.arraySize > 0)
            {
                for (var i = 0; i < _giLocalisablePrefabsProperty.arraySize; i++)
                {
                    var arrayProperty = _giLocalisablePrefabsProperty.GetArrayElementAtIndex(i);
                    var nameProperty = arrayProperty.FindPropertyRelative("Name");

                    var deleted = false;
                    // indent
                    EditorGUILayout.BeginHorizontal(GuiStyles.BoxLightStyle);
                    GUILayout.Space(15f);
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    arrayProperty.isExpanded = EditorGUILayout.Foldout(arrayProperty.isExpanded, string.IsNullOrEmpty(nameProperty.stringValue) ? "<missing name>" : nameProperty.stringValue);
                    if (GUILayout.Button("X", GuiStyles.BorderlessButtonStyle, GUILayout.Width(12), GUILayout.Height(12)) &&
                        EditorUtility.DisplayDialog("Remove Entry?", "Are you sure you want to remove this entry?", "Yes",
                            "No"))
                    {
                        _giLocalisablePrefabsProperty.DeleteArrayElementAtIndex(i);
                        //                        _poolManager.Pools.RemoveAt(i);
                        deleted = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!deleted && arrayProperty.isExpanded)
                    {
                        var typeProperty = arrayProperty.FindPropertyRelative("LocalisablePrefabEntryType");
                        var defaultPrefabProperty = arrayProperty.FindPropertyRelative("LocalisablePrefab");

                        EditorGUILayout.PropertyField(typeProperty, new GUIContent("Type", typeProperty.tooltip));
                        if (typeProperty.enumValueIndex == (int)GameItem.LocalisablePrefabEntry.LocalisablePrefabEntryTypeEnum.SelectionMenu)
                        {
                            nameProperty.stringValue = "Selection Menu";
                        }
                        if (typeProperty.enumValueIndex == (int)GameItem.LocalisablePrefabEntry.LocalisablePrefabEntryTypeEnum.InGame)
                        {
                            nameProperty.stringValue = "In Game";
                        }
                        if (typeProperty.enumValueIndex == (int)GameItem.LocalisablePrefabEntry.LocalisablePrefabEntryTypeEnum.Custom)
                        {
                            EditorGUILayout.PropertyField(nameProperty, new GUIContent("Name", nameProperty.tooltip));
                        }
                        EditorGUILayout.PropertyField(defaultPrefabProperty, new GUIContent("Prefab", defaultPrefabProperty.tooltip));
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(2f);
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Add Prefab Entry"), GUILayout.ExpandWidth(false)))
            {
                AddNewPrefab();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);
            EditorGUILayout.EndVertical();
        }

        void AddNewPrefab(GameObject prefab = null)
        {
            _giLocalisablePrefabsProperty.arraySize++;
            var newElement =
                _giLocalisablePrefabsProperty.GetArrayElementAtIndex(_giLocalisablePrefabsProperty.arraySize - 1);
            newElement.isExpanded = true;
            var propType = newElement.FindPropertyRelative("LocalisablePrefabEntryType");
            propType.enumValueIndex = 0;
            var propName = newElement.FindPropertyRelative("Name");
            propName.stringValue = null;
            var propPrefab = newElement.FindPropertyRelative("LocalisablePrefab._default");
            propPrefab.objectReferenceValue = prefab;
            var propLocalisedPrefabs = newElement.FindPropertyRelative("LocalisablePrefab._localisedItems");
            propLocalisedPrefabs.arraySize = 0;
        }

        protected void DrawSprites()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(new GUIContent("Sprites", "Here you can add prefabs for use with standard features such as selection screens or for your own custom needs"), EditorStyles.boldLabel);

            _spriteDropRect = DrawDropRect("Drag a Sprite here to create a new entry");

            if (_giLocalisableSpritesProperty.arraySize > 0)
            {
                for (var i = 0; i < _giLocalisableSpritesProperty.arraySize; i++)
                {
                    var arrayProperty = _giLocalisableSpritesProperty.GetArrayElementAtIndex(i);
                    var nameProperty = arrayProperty.FindPropertyRelative("Name");

                    var deleted = false;
                    // indent
                    EditorGUILayout.BeginHorizontal(GuiStyles.BoxLightStyle);
                    GUILayout.Space(15f);
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    arrayProperty.isExpanded = EditorGUILayout.Foldout(arrayProperty.isExpanded, string.IsNullOrEmpty(nameProperty.stringValue) ? "<missing name>" : nameProperty.stringValue);
                    if (GUILayout.Button("X", GuiStyles.BorderlessButtonStyle, GUILayout.Width(12), GUILayout.Height(12)) &&
                        EditorUtility.DisplayDialog("Remove Entry?", "Are you sure you want to remove this entry?", "Yes",
                            "No"))
                    {
                        _giLocalisableSpritesProperty.DeleteArrayElementAtIndex(i);
                        deleted = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!deleted && arrayProperty.isExpanded)
                    {
                        var typeProperty = arrayProperty.FindPropertyRelative("LocalisableSpriteEntryType");
                        var defaultSpriteProperty = arrayProperty.FindPropertyRelative("LocalisableSprite");

                        EditorGUILayout.PropertyField(typeProperty, new GUIContent("Type", typeProperty.tooltip));
                        if (typeProperty.enumValueIndex == (int)GameItem.LocalisableSpriteEntry.LocalisableSpriteEntryTypeEnum.SelectionMenu)
                        {
                            nameProperty.stringValue = "Selection Menu";
                        }
                        if (typeProperty.enumValueIndex == (int)GameItem.LocalisableSpriteEntry.LocalisableSpriteEntryTypeEnum.InGame)
                        {
                            nameProperty.stringValue = "In Game";
                        }
                        if (typeProperty.enumValueIndex == (int)GameItem.LocalisableSpriteEntry.LocalisableSpriteEntryTypeEnum.Custom)
                        {
                            EditorGUILayout.PropertyField(nameProperty, new GUIContent("Name", nameProperty.tooltip));
                        }

                        EditorGUILayout.PropertyField(defaultSpriteProperty, new GUIContent("Sprite", defaultSpriteProperty.tooltip));
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(2f);
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Add Sprite Entry"), GUILayout.ExpandWidth(false)))
            {
                AddNewSprite();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);
            EditorGUILayout.EndVertical();
        }

        void AddNewSprite(Sprite sprite = null)
        {
            _giLocalisableSpritesProperty.arraySize++;
            var newElement =
                _giLocalisableSpritesProperty.GetArrayElementAtIndex(_giLocalisableSpritesProperty.arraySize - 1);
            newElement.isExpanded = true;
            var propType = newElement.FindPropertyRelative("LocalisableSpriteEntryType");
            propType.enumValueIndex = (int)GameItem.LocalisableSpriteEntry.LocalisableSpriteEntryTypeEnum.Custom;
            var propName = newElement.FindPropertyRelative("Name");
            propName.stringValue = null;
            var propSprite = newElement.FindPropertyRelative("LocalisableSprite._default");
            propSprite.objectReferenceValue = sprite;
            var propLocalisedSprite = newElement.FindPropertyRelative("LocalisableSprite._localisedItems");
            propLocalisedSprite.arraySize = 0;
        }

        #region Drag and Drop
        Rect DrawDropRect(string title)
        {
            GUILayout.Space(2f);
            var dropRect = GUILayoutUtility.GetRect(0f, 30f, GUILayout.ExpandWidth(true));
            dropRect.x += 2;
            dropRect.width -= 4;
            GUI.Box(dropRect, title, GuiStyles.DropAreaStyle);
            GUILayout.Space(2f);
            return dropRect;
        }

        void CheckDragAndDrop(Rect dropArea, Func<UnityEngine.Object, bool> dragValidTest, Action<UnityEngine.Object> processValidDrop)
        {
            var currentEvent = Event.current;

            if (!dropArea.Contains(currentEvent.mousePosition))
                return;

            switch (currentEvent.type)
            {
                // is dragging
                case EventType.DragUpdated:
                    // check that at least one of the dragged items is valie
                    var dragIsValid = false;
                    foreach (var draggedObject in DragAndDrop.objectReferences)
                    {
                        if (dragValidTest(draggedObject))
                        {
                            dragIsValid = true;
                            break;
                        }

                    }

                    // changing the visual mode of the cursor and hence whether a drop can be performed based on the IsDragValid function.
                    DragAndDrop.visualMode = dragIsValid ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
                    
                    // Consume the event so it isn't used by anything else.
                    currentEvent.Use();
                    break;

                // was dragging and has now dropped
                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();

                    foreach (var draggedObject in DragAndDrop.objectReferences)
                    {
                        if (dragValidTest(draggedObject))
                            processValidDrop(draggedObject);
                    }

                    // Consume the event so it isn't used by anything else.
                    currentEvent.Use();
                    break;
            }
        }

        #endregion Drag and Drop
    }
}
