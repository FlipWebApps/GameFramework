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

using UnityEditor;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Editor.AbstractClasses
{
    public abstract class GameItemButtonEditor : UnityEditor.Editor
    {
        SerializedProperty _contextProperty;
        SerializedProperty _numberProperty;
        SerializedProperty _legacyDisplayModeProperty;
        SerializedProperty _selectionModeProperty;
        SerializedProperty _clickUnlockedSceneToLoadProperty;
        SerializedProperty _coinColorCanUnlockProperty;
        SerializedProperty _coinColorCantUnlockProperty;
        SerializedProperty _clickToUnlockProperty;
        SerializedProperty _clickToBuyProperty;

        SerializedProperty _showBuyWindowProperty;
        SerializedProperty _buyTitleTextProperty;
        SerializedProperty _buyText1Property;
        SerializedProperty _buyText2Property;
        SerializedProperty _buyDialogSpriteTypeProperty;
        SerializedProperty _buyDialogSpriteProperty;
        SerializedProperty _buyContentPrefabProperty;
        SerializedProperty _buyContentAnimatorControllerProperty;
        SerializedProperty _buyContentShowsButtonsProperty;

        SerializedProperty _buyOrUnlockTitleTextProperty;
        SerializedProperty _buyOrUnlockText1Property;
        SerializedProperty _buyOrUnlockText2Property;
        SerializedProperty _buyOrUnlockDialogSpriteTypeProperty;
        SerializedProperty _buyOrUnlockDialogSpriteProperty;
        SerializedProperty _buyOrUnlockContentPrefabProperty;
        SerializedProperty _buyOrUnlockContentAnimatorControllerProperty;
        SerializedProperty _buyOrUnlockContentShowsButtonsProperty;

        SerializedProperty _buyCantUnlockTitleTextProperty;
        SerializedProperty _buyCantUnlockText1Property;
        SerializedProperty _buyCantUnlockText2Property;
        SerializedProperty _buyCantUnlockDialogSpriteTypeProperty;
        SerializedProperty _buyCantUnlockDialogSpriteProperty;
        SerializedProperty _buyCantUnlockContentPrefabProperty;
        SerializedProperty _buyCantUnlockContentAnimatorControllerProperty;
        SerializedProperty _buyCantUnlockContentShowsButtonsProperty;

        SerializedProperty _showConfirmWindowProperty;
        SerializedProperty _confirmTitleTextProperty;
        SerializedProperty _confirmText1Property;
        SerializedProperty _confirmText2Property;
        SerializedProperty _confirmDialogSpriteTypeProperty;
        SerializedProperty _confirmDialogSpriteProperty;
        SerializedProperty _confirmContentPrefabProperty;
        SerializedProperty _confirmContentAnimatorControllerProperty;
        SerializedProperty _confirmContentShowsButtonsProperty;

        SerializedProperty _showUnlockWindowProperty;
        SerializedProperty _unlockTitleTextProperty;
        SerializedProperty _unlockText1Property;
        SerializedProperty _unlockText2Property;
        SerializedProperty _unlockContentPrefabProperty;
        SerializedProperty _unlockContentAnimatorControllerProperty;
        SerializedProperty _unlockContentShowsButtonsProperty;

        SerializedProperty _showNotEnoughCoinsWindowProperty;
        SerializedProperty _notEnoughCoinsTitleTextProperty;
        SerializedProperty _notEnoughCoinsText1Property;
        SerializedProperty _notEnoughCoinsText2Property;
        SerializedProperty _notEnoughCoinsDialogSpriteTypeProperty;
        SerializedProperty _notEnoughCoinsDialogSpriteProperty;
        SerializedProperty _notEnoughCoinsContentPrefabProperty;
        SerializedProperty _notEnoughCoinsContentAnimatorControllerProperty;
        SerializedProperty _notEnoughCoinsContentShowsButtonsProperty;

        public void OnEnable()
        {
            _contextProperty = serializedObject.FindProperty("_context");
            _numberProperty = serializedObject.FindProperty("Number");
            _legacyDisplayModeProperty = serializedObject.FindProperty("LegacyDisplayMode");
            _selectionModeProperty = serializedObject.FindProperty("SelectionMode");
            _clickUnlockedSceneToLoadProperty = serializedObject.FindProperty("ClickUnlockedSceneToLoad");
            _coinColorCanUnlockProperty = serializedObject.FindProperty("CoinColorCanUnlock");
            _coinColorCantUnlockProperty = serializedObject.FindProperty("CoinColorCantUnlock");
            _clickToUnlockProperty = serializedObject.FindProperty("_clickToUnlock");
            _clickToBuyProperty = serializedObject.FindProperty("_clickToBuy");

            _buyOrUnlockTitleTextProperty = serializedObject.FindProperty("BuyOrUnlockTitleText");
            _buyOrUnlockText1Property = serializedObject.FindProperty("BuyOrUnlockText1");
            _buyOrUnlockText2Property = serializedObject.FindProperty("BuyOrUnlockText2");
            _buyOrUnlockDialogSpriteTypeProperty = serializedObject.FindProperty("BuyOrUnlockDialogSpriteType");
            _buyOrUnlockDialogSpriteProperty = serializedObject.FindProperty("BuyOrUnlockDialogSprite");
            _buyOrUnlockContentPrefabProperty = serializedObject.FindProperty("BuyOrUnlockContentPrefab");
            _buyOrUnlockContentAnimatorControllerProperty = serializedObject.FindProperty("BuyOrUnlockContentAnimatorController");
            _buyOrUnlockContentShowsButtonsProperty = serializedObject.FindProperty("BuyOrUnlockContentShowsButtons");

            _buyCantUnlockTitleTextProperty = serializedObject.FindProperty("BuyCantUnlockTitleText");
            _buyCantUnlockText1Property = serializedObject.FindProperty("BuyCantUnlockText1");
            _buyCantUnlockText2Property = serializedObject.FindProperty("BuyCantUnlockText2");
            _buyCantUnlockDialogSpriteTypeProperty = serializedObject.FindProperty("BuyCantUnlockDialogSpriteType");
            _buyCantUnlockDialogSpriteProperty = serializedObject.FindProperty("BuyCantUnlockDialogSprite");
            _buyCantUnlockContentPrefabProperty = serializedObject.FindProperty("BuyCantUnlockContentPrefab");
            _buyCantUnlockContentAnimatorControllerProperty = serializedObject.FindProperty("BuyCantUnlockContentAnimatorController");
            _buyCantUnlockContentShowsButtonsProperty = serializedObject.FindProperty("BuyCantUnlockContentShowsButtons");

            _showBuyWindowProperty = serializedObject.FindProperty("ShowBuyWindow");
            _buyTitleTextProperty = serializedObject.FindProperty("BuyTitleText");
            _buyText1Property = serializedObject.FindProperty("BuyText1");
            _buyText2Property = serializedObject.FindProperty("BuyText2");
            _buyDialogSpriteTypeProperty = serializedObject.FindProperty("BuyDialogSpriteType");
            _buyDialogSpriteProperty = serializedObject.FindProperty("BuyDialogSprite");
            _buyContentPrefabProperty = serializedObject.FindProperty("BuyContentPrefab");
            _buyContentAnimatorControllerProperty = serializedObject.FindProperty("BuyContentAnimatorController");
            _buyContentShowsButtonsProperty = serializedObject.FindProperty("BuyContentShowsButtons");

            _showConfirmWindowProperty = serializedObject.FindProperty("ShowConfirmWindow");
            _confirmTitleTextProperty = serializedObject.FindProperty("ConfirmTitleText");
            _confirmText1Property = serializedObject.FindProperty("ConfirmText1");
            _confirmText2Property = serializedObject.FindProperty("ConfirmText2");
            _confirmDialogSpriteTypeProperty = serializedObject.FindProperty("ConfirmDialogSpriteType");
            _confirmDialogSpriteProperty = serializedObject.FindProperty("ConfirmDialogSprite");
            _confirmContentPrefabProperty = serializedObject.FindProperty("ConfirmContentPrefab");
            _confirmContentAnimatorControllerProperty = serializedObject.FindProperty("ConfirmContentAnimatorController");
            _confirmContentShowsButtonsProperty = serializedObject.FindProperty("ConfirmContentShowsButtons");

            _showUnlockWindowProperty = serializedObject.FindProperty("ShowUnlockWindow");
            _unlockTitleTextProperty = serializedObject.FindProperty("UnlockTitleText");
            _unlockText1Property = serializedObject.FindProperty("UnlockText1");
            _unlockText2Property = serializedObject.FindProperty("UnlockText2");
            _unlockContentPrefabProperty = serializedObject.FindProperty("UnlockContentPrefab");
            _unlockContentAnimatorControllerProperty = serializedObject.FindProperty("UnlockContentAnimatorController");
            _unlockContentShowsButtonsProperty = serializedObject.FindProperty("UnlockContentShowsButtons");

            _showNotEnoughCoinsWindowProperty = serializedObject.FindProperty("ShowNotEnoughCoinsWindow");
            _notEnoughCoinsTitleTextProperty = serializedObject.FindProperty("NotEnoughCoinsTitleText");
            _notEnoughCoinsText1Property = serializedObject.FindProperty("NotEnoughCoinsText1");
            _notEnoughCoinsText2Property = serializedObject.FindProperty("NotEnoughCoinsText2");
            _notEnoughCoinsDialogSpriteTypeProperty = serializedObject.FindProperty("NotEnoughCoinsDialogSpriteType");
            _notEnoughCoinsDialogSpriteProperty = serializedObject.FindProperty("NotEnoughCoinsDialogSprite");
            _notEnoughCoinsContentPrefabProperty = serializedObject.FindProperty("NotEnoughCoinsContentPrefab");
            _notEnoughCoinsContentAnimatorControllerProperty = serializedObject.FindProperty("NotEnoughCoinsContentAnimatorController");
            _notEnoughCoinsContentShowsButtonsProperty = serializedObject.FindProperty("NotEnoughCoinsContentShowsButtons");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_contextProperty);
            EditorGUILayout.PropertyField(_numberProperty, new GUIContent("Number (Deprecated)"));
            EditorGUILayout.PropertyField(_legacyDisplayModeProperty);
            EditorGUILayout.PropertyField(_selectionModeProperty);
            if (_selectionModeProperty.enumValueIndex == 0)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_clickUnlockedSceneToLoadProperty, new GUIContent("Unlocked Scene"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Unlocking", EditorStyles.boldLabel);
            if (_legacyDisplayModeProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_coinColorCanUnlockProperty);
                EditorGUILayout.PropertyField(_coinColorCantUnlockProperty);
            }

            EditorGUILayout.PropertyField(_clickToBuyProperty);
            EditorGUILayout.PropertyField(_clickToUnlockProperty);
            if (_clickToBuyProperty.boolValue)
            {
                EditorGUI.indentLevel++;

                if (_clickToUnlockProperty.boolValue)
                {
                    EditorGUI.indentLevel++;
                    _buyOrUnlockTitleTextProperty.isExpanded =
                        EditorGUILayout.Foldout(_buyOrUnlockTitleTextProperty.isExpanded, "Buy Or Unlock Window Configuration");
                    if (_buyOrUnlockTitleTextProperty.isExpanded)
                    {

                        EditorGUILayout.PropertyField(_buyOrUnlockTitleTextProperty,
                            new GUIContent("Title", _buyOrUnlockTitleTextProperty.tooltip));
                        EditorGUILayout.PropertyField(_buyOrUnlockText1Property,
                            new GUIContent("Text 1", _buyOrUnlockText1Property.tooltip));
                        EditorGUILayout.PropertyField(_buyOrUnlockText2Property,
                            new GUIContent("Text 2", _buyOrUnlockText2Property.tooltip));
                        EditorGUILayout.PropertyField(_buyOrUnlockDialogSpriteTypeProperty,
                            new GUIContent("Image", _buyOrUnlockDialogSpriteTypeProperty.tooltip));
                        if (_buyOrUnlockDialogSpriteTypeProperty.enumValueIndex == 2)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(_buyOrUnlockDialogSpriteProperty, GUIContent.none);
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel++;
                        _buyOrUnlockContentPrefabProperty.isExpanded = EditorGUILayout.Foldout(_buyOrUnlockContentPrefabProperty.isExpanded, "Advanced");
                        if (_buyOrUnlockContentPrefabProperty.isExpanded)
                        {
                            EditorGUILayout.PropertyField(_buyOrUnlockContentPrefabProperty,
                                new GUIContent("Content Prefab", _buyOrUnlockContentPrefabProperty.tooltip));
                            EditorGUILayout.PropertyField(_buyOrUnlockContentAnimatorControllerProperty,
                                new GUIContent("Content Animation",
                                    _buyOrUnlockContentAnimatorControllerProperty.tooltip));
                            EditorGUILayout.PropertyField(_buyOrUnlockContentShowsButtonsProperty,
                                new GUIContent("Content Shows Buttons", _buyOrUnlockContentShowsButtonsProperty.tooltip));
                        }
                        EditorGUI.indentLevel--;
                    }

                    _buyCantUnlockTitleTextProperty.isExpanded =
                        EditorGUILayout.Foldout(_buyCantUnlockTitleTextProperty.isExpanded, "Buy Can't Unlock Window Configuration");
                    if (_buyCantUnlockTitleTextProperty.isExpanded)
                    {

                        EditorGUILayout.PropertyField(_buyCantUnlockTitleTextProperty,
                            new GUIContent("Title", _buyCantUnlockTitleTextProperty.tooltip));
                        EditorGUILayout.PropertyField(_buyCantUnlockText1Property,
                            new GUIContent("Text 1", _buyCantUnlockText1Property.tooltip));
                        EditorGUILayout.PropertyField(_buyCantUnlockText2Property,
                            new GUIContent("Text 2", _buyCantUnlockText2Property.tooltip));
                        EditorGUILayout.PropertyField(_buyCantUnlockDialogSpriteTypeProperty,
                            new GUIContent("Image", _buyCantUnlockDialogSpriteTypeProperty.tooltip));
                        if (_buyCantUnlockDialogSpriteTypeProperty.enumValueIndex == 2)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(_buyCantUnlockDialogSpriteProperty, GUIContent.none);
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel++;
                        _buyCantUnlockContentPrefabProperty.isExpanded = EditorGUILayout.Foldout(_buyCantUnlockContentPrefabProperty.isExpanded, "Advanced");
                        if (_buyCantUnlockContentPrefabProperty.isExpanded)
                        {
                            EditorGUILayout.PropertyField(_buyCantUnlockContentPrefabProperty,
                                new GUIContent("Content Prefab", _buyCantUnlockContentPrefabProperty.tooltip));
                            EditorGUILayout.PropertyField(_buyCantUnlockContentAnimatorControllerProperty,
                                new GUIContent("Content Animation", _buyCantUnlockContentAnimatorControllerProperty.tooltip));
                            EditorGUILayout.PropertyField(_buyCantUnlockContentShowsButtonsProperty,
                                new GUIContent("Content Shows Buttons", _buyCantUnlockContentShowsButtonsProperty.tooltip));
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }

                if (!_clickToUnlockProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(_showBuyWindowProperty);
                    if (_showBuyWindowProperty.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        _showBuyWindowProperty.isExpanded =
                            EditorGUILayout.Foldout(_showBuyWindowProperty.isExpanded, "Configuration");
                        if (_showBuyWindowProperty.isExpanded)
                        {

                            EditorGUILayout.PropertyField(_buyTitleTextProperty,
                                new GUIContent("Title", _buyTitleTextProperty.tooltip));
                            EditorGUILayout.PropertyField(_buyText1Property,
                                new GUIContent("Text 1", _buyText1Property.tooltip));
                            EditorGUILayout.PropertyField(_buyText2Property,
                                new GUIContent("Text 2", _buyText2Property.tooltip));
                            EditorGUILayout.PropertyField(_buyDialogSpriteTypeProperty,
                                new GUIContent("Image", _buyDialogSpriteTypeProperty.tooltip));
                            if (_buyDialogSpriteTypeProperty.enumValueIndex == 2)
                            {
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(_buyDialogSpriteProperty, GUIContent.none);
                                EditorGUI.indentLevel--;
                            }
                            EditorGUI.indentLevel++;
                            _buyContentPrefabProperty.isExpanded =
                                EditorGUILayout.Foldout(_buyContentPrefabProperty.isExpanded, "Advanced");
                            if (_buyContentPrefabProperty.isExpanded)
                            {
                                EditorGUILayout.PropertyField(_buyContentPrefabProperty,
                                    new GUIContent("Content Prefab", _buyContentPrefabProperty.tooltip));
                                EditorGUILayout.PropertyField(_buyContentAnimatorControllerProperty,
                                    new GUIContent("Content Animation", _buyContentAnimatorControllerProperty.tooltip));
                                EditorGUILayout.PropertyField(_buyContentShowsButtonsProperty,
                                    new GUIContent("Content Shows Buttons", _buyContentShowsButtonsProperty.tooltip));
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }

            if (_clickToUnlockProperty.boolValue)
            {
                EditorGUI.indentLevel++;

                if (!_clickToBuyProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(_showConfirmWindowProperty);
                    if (_showConfirmWindowProperty.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        _showConfirmWindowProperty.isExpanded =
                            EditorGUILayout.Foldout(_showConfirmWindowProperty.isExpanded, "Configuration");
                        if (_showConfirmWindowProperty.isExpanded)
                        {

                            EditorGUILayout.PropertyField(_confirmTitleTextProperty,
                                new GUIContent("Title", _confirmTitleTextProperty.tooltip));
                            EditorGUILayout.PropertyField(_confirmText1Property,
                                new GUIContent("Text 1", _confirmText1Property.tooltip));
                            EditorGUILayout.PropertyField(_confirmText2Property,
                                new GUIContent("Text 2", _confirmText2Property.tooltip));
                            EditorGUILayout.PropertyField(_confirmDialogSpriteTypeProperty,
                                new GUIContent("Image", _confirmDialogSpriteTypeProperty.tooltip));
                            if (_confirmDialogSpriteTypeProperty.enumValueIndex == 2)
                            {
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(_confirmDialogSpriteProperty, GUIContent.none);
                                EditorGUI.indentLevel--;
                            }
                            EditorGUI.indentLevel++;
                            _confirmContentPrefabProperty.isExpanded =
                                EditorGUILayout.Foldout(_confirmContentPrefabProperty.isExpanded, "Advanced");
                            if (_confirmContentPrefabProperty.isExpanded)
                            {
                                EditorGUILayout.PropertyField(_confirmContentPrefabProperty,
                                    new GUIContent("Content Prefab", _confirmContentPrefabProperty.tooltip));
                                EditorGUILayout.PropertyField(_confirmContentAnimatorControllerProperty,
                                    new GUIContent("Content Animation",
                                        _confirmContentAnimatorControllerProperty.tooltip));
                                EditorGUILayout.PropertyField(_confirmContentShowsButtonsProperty,
                                    new GUIContent("Content Shows Buttons", _confirmContentShowsButtonsProperty.tooltip));
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                }

                EditorGUILayout.PropertyField(_showUnlockWindowProperty);
                if (_showUnlockWindowProperty.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.indentLevel++;
                    _showUnlockWindowProperty.isExpanded = EditorGUILayout.Foldout(
                        _showUnlockWindowProperty.isExpanded, "Configuration");
                    if (_showUnlockWindowProperty.isExpanded)
                    {
                        EditorGUILayout.PropertyField(_unlockTitleTextProperty,
                            new GUIContent("Title", _unlockTitleTextProperty.tooltip));
                        EditorGUILayout.PropertyField(_unlockText1Property,
                            new GUIContent("Text 1", _unlockText1Property.tooltip));
                        EditorGUILayout.PropertyField(_unlockText2Property,
                            new GUIContent("Text 2", _unlockText2Property.tooltip));
                        EditorGUI.indentLevel++;
                        _unlockContentPrefabProperty.isExpanded = EditorGUILayout.Foldout(_unlockContentPrefabProperty.isExpanded, "Advanced");
                        if (_unlockContentPrefabProperty.isExpanded)
                        {
                            EditorGUILayout.PropertyField(_unlockContentPrefabProperty,
                                new GUIContent("Content Prefab", _unlockContentPrefabProperty.tooltip));
                            EditorGUILayout.PropertyField(_unlockContentAnimatorControllerProperty,
                                new GUIContent("Content Animation", _unlockContentAnimatorControllerProperty.tooltip));
                            EditorGUILayout.PropertyField(_unlockContentShowsButtonsProperty,
                                new GUIContent("Content Shows Buttons", _unlockContentShowsButtonsProperty.tooltip));
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUI.indentLevel--;
                }

                if (!_clickToBuyProperty.boolValue)
                {

                    EditorGUILayout.PropertyField(_showNotEnoughCoinsWindowProperty);
                    if (_showNotEnoughCoinsWindowProperty.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        _showNotEnoughCoinsWindowProperty.isExpanded = EditorGUILayout.Foldout(
                            _showNotEnoughCoinsWindowProperty.isExpanded, "Configuration");
                        if (_showNotEnoughCoinsWindowProperty.isExpanded)
                        {
                            EditorGUILayout.PropertyField(_notEnoughCoinsTitleTextProperty,
                                new GUIContent("Title", _notEnoughCoinsTitleTextProperty.tooltip));
                            EditorGUILayout.PropertyField(_notEnoughCoinsText1Property,
                                new GUIContent("Text 1", _notEnoughCoinsText1Property.tooltip));
                            EditorGUILayout.PropertyField(_notEnoughCoinsText2Property,
                                new GUIContent("Text 1", _notEnoughCoinsText2Property.tooltip));
                            EditorGUILayout.PropertyField(_notEnoughCoinsDialogSpriteTypeProperty,
                                new GUIContent("Image", _notEnoughCoinsDialogSpriteTypeProperty.tooltip));
                            if (_notEnoughCoinsDialogSpriteTypeProperty.enumValueIndex == 2)
                            {
                                EditorGUI.indentLevel++;
                                EditorGUILayout.PropertyField(_notEnoughCoinsDialogSpriteProperty, GUIContent.none);
                                EditorGUI.indentLevel--;
                            }
                            _notEnoughCoinsContentPrefabProperty.isExpanded =
                                EditorGUILayout.Foldout(_notEnoughCoinsContentPrefabProperty.isExpanded, "Advanced");
                            if (_notEnoughCoinsContentPrefabProperty.isExpanded)
                            {
                                EditorGUILayout.PropertyField(_notEnoughCoinsContentPrefabProperty,
                                    new GUIContent("Content Prefab", _notEnoughCoinsContentPrefabProperty.tooltip));
                                EditorGUILayout.PropertyField(_notEnoughCoinsContentAnimatorControllerProperty,
                                    new GUIContent("Content Animation",
                                        _notEnoughCoinsContentAnimatorControllerProperty.tooltip));
                                EditorGUILayout.PropertyField(_notEnoughCoinsContentShowsButtonsProperty,
                                    new GUIContent("Content Shows Buttons",
                                        _notEnoughCoinsContentShowsButtonsProperty.tooltip));
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                }

                EditorGUI.indentLevel++;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
