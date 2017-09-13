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
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Preferences;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
#pragma warning disable 618

namespace GameFramework.GameStructure.Game.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        GameManager _gameManager;
        bool _showLinks;
        bool _showAdvanced;
        bool _showPrefs;
        bool _showPlayerAdvanced;
        Rect _mainHelpRect;

        ReorderableList _numberedLevelReferencesList;

        SerializedProperty _gameNameProperty;
        SerializedProperty _playWebUrlProperty;
        SerializedProperty _playMarketUrlProperty;
        SerializedProperty _iOSWebUrlProperty;
        SerializedProperty _debugLevelProperty;
        SerializedProperty _referencePhysicalScreenHeightInInchesProperty;

        SerializedProperty _useSecurePreferences;
        SerializedProperty _preferencesPassPhrase;
        SerializedProperty _autoConvertUnsecurePrefs;

        SerializedProperty _displayChangeCheckDelayProperty;
        SerializedProperty _identifierBaseProperty;

        SerializedProperty _variablesProperty;

        SerializedProperty _playerSetupModeProperty;
        SerializedProperty _playerCountProperty;

        SerializedProperty _worldSetupModeProperty;
        SerializedProperty _autoCreateWorldsProperty;
        SerializedProperty _numberOfAutoCreatedWorldsProperty;
        SerializedProperty _coinsToUnlockWorldsProperty;
        SerializedProperty _worldUnlockModeProperty;
        SerializedProperty _worldLevelNumbersProperty;

        SerializedProperty _levelSetupModeProperty;
        SerializedProperty _autoCreateLevelsProperty;
        SerializedProperty _numberOfAutoCreatedLevelsProperty;
        SerializedProperty _coinsToUnlockLevelsProperty;
        SerializedProperty _levelUnlockModeProperty;
        SerializedProperty _levelMasterProperty;
        SerializedProperty _numberedLevelReferencesProperty;

        SerializedProperty _characterSetupModeProperty;
        SerializedProperty _autoCreateCharactersProperty;
        SerializedProperty _numberOfAutoCreatedCharactersProperty;
        SerializedProperty _coinsToUnlockCharactersProperty;
        SerializedProperty _characterUnlockModeProperty;

        void OnEnable()
        {
            _gameManager = (GameManager)target;
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _gameNameProperty = serializedObject.FindProperty("GameName");
            _playWebUrlProperty = serializedObject.FindProperty("PlayWebUrl");
            _playMarketUrlProperty = serializedObject.FindProperty("PlayMarketUrl");
            _iOSWebUrlProperty = serializedObject.FindProperty("iOSWebUrl");
            _debugLevelProperty = serializedObject.FindProperty("DebugLevel");
            _identifierBaseProperty = serializedObject.FindProperty("IdentifierBase");

            _useSecurePreferences = serializedObject.FindProperty("SecurePreferences");
            _preferencesPassPhrase = serializedObject.FindProperty("PreferencesPassPhrase");
            _autoConvertUnsecurePrefs = serializedObject.FindProperty("AutoConvertUnsecurePrefs");

            _referencePhysicalScreenHeightInInchesProperty = serializedObject.FindProperty("ReferencePhysicalScreenHeightInInches");
            _displayChangeCheckDelayProperty = serializedObject.FindProperty("DisplayChangeCheckDelay");

            _variablesProperty = serializedObject.FindProperty("Variables");

            _playerSetupModeProperty = serializedObject.FindProperty("PlayerSetupMode");
            _playerCountProperty = serializedObject.FindProperty("PlayerCount");

            _worldSetupModeProperty = serializedObject.FindProperty("WorldSetupMode");
            _autoCreateWorldsProperty = serializedObject.FindProperty("AutoCreateWorlds");
            _numberOfAutoCreatedWorldsProperty = serializedObject.FindProperty("NumberOfAutoCreatedWorlds");
            _worldUnlockModeProperty = serializedObject.FindProperty("WorldUnlockMode");
            _coinsToUnlockWorldsProperty = serializedObject.FindProperty("CoinsToUnlockWorlds");
            _worldLevelNumbersProperty = serializedObject.FindProperty("WorldLevelNumbers");

            _levelSetupModeProperty = serializedObject.FindProperty("LevelSetupMode");
            _autoCreateLevelsProperty = serializedObject.FindProperty("AutoCreateLevels");
            _numberOfAutoCreatedLevelsProperty = serializedObject.FindProperty("NumberOfAutoCreatedLevels");
            _levelUnlockModeProperty = serializedObject.FindProperty("LevelUnlockMode");
            _coinsToUnlockLevelsProperty = serializedObject.FindProperty("CoinsToUnlockLevels");
            _levelMasterProperty = serializedObject.FindProperty("LevelMaster");
            _numberedLevelReferencesProperty = serializedObject.FindProperty("NumberedLevelReferences");

            _characterSetupModeProperty = serializedObject.FindProperty("CharacterSetupMode");
            _autoCreateCharactersProperty = serializedObject.FindProperty("AutoCreateCharacters");
            _numberOfAutoCreatedCharactersProperty = serializedObject.FindProperty("NumberOfAutoCreatedCharacters");
            _characterUnlockModeProperty = serializedObject.FindProperty("CharacterUnlockMode");
            _coinsToUnlockCharactersProperty = serializedObject.FindProperty("CoinsToUnlockCharacters");

            _numberedLevelReferencesList = new ReorderableList(serializedObject, _numberedLevelReferencesProperty, true, true, true, true);
            _numberedLevelReferencesList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Level Overrides (Between 1 and " + _numberOfAutoCreatedLevelsProperty.intValue + ")");
            };
            _numberedLevelReferencesList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = _numberedLevelReferencesList.serializedProperty.GetArrayElementAtIndex(index);
                    var oldIndent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Number"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 65, rect.y, rect.width - 65, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("GameItemReference"), GUIContent.none);
                    EditorGUI.indentLevel = oldIndent;
                };
            _numberedLevelReferencesList.onAddCallback = (ReorderableList l) => {
                var index = l.serializedProperty.arraySize;
                l.serializedProperty.arraySize++;
                l.index = index;
                var element = l.serializedProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative("Number").intValue = 1;
                element.FindPropertyRelative("GameItemReference").objectReferenceValue = null;
            };
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            serializedObject.Update();

            _mainHelpRect = EditorHelper.ShowHideableHelpBox("GameFramework.GameStructure.GameManagerEditorWindow", "In GameManager you can configure and access various global settings for your game.\n\nIn addition to this runtime configuration, additional static configuration options such as custom counters are now also available by adding a GameConfiguration file to a /Resources/ folder within your project (right click the folder -> Create -> Game Framework -> Game Configuration)\n\nNote that as of v4.5 Default Lives is moved to GameConfiguration.", _mainHelpRect);

            DrawGameDetails();
            DrawGameStructure();
            DrawVariables();

            // do this check here at the end of layout to avoid any layout issues
            if (Event.current.type == EventType.Repaint)
            {
                _worldLevelNumbersProperty.arraySize = _gameManager.NumberOfAutoCreatedWorlds;
            }

            serializedObject.ApplyModifiedProperties();


        }

        void DrawGameDetails()
        {
            EditorGUILayout.LabelField("Game Details", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_gameNameProperty);
            EditorGUI.indentLevel += 1;
            _showLinks = EditorGUILayout.Foldout(_showLinks, "Links");
            if (_showLinks)
            {
                EditorGUILayout.PropertyField(_playWebUrlProperty);
                EditorGUILayout.PropertyField(_playMarketUrlProperty);
                EditorGUILayout.PropertyField(_iOSWebUrlProperty);
            }
            _showPrefs = EditorGUILayout.Foldout(_showPrefs, "Player Preferences");
            if (_showPrefs)
            {
                if (!PreferencesFactory.SupportsSecurePrefs)
                {
                    EditorGUILayout.HelpBox("Unity preferences are not encrypted allowing users to modify them and cheat!\n\nSee the integrations window (Window Menu | Game Framework) for assets that let you use encrypted preferences instead (e.g. Prefs Editor).", MessageType.Warning);
                    GUI.enabled = false;
                }
                else if (!_gameManager.SecurePreferences)
                {
                    EditorGUILayout.HelpBox("Unity preferences are not encrypted!\n\nOn certain platforms Unity stores preferences as plain text and it may be possible for others to manually modify them. Depending on your game needs, consider enabling secure preferences below.", MessageType.Warning);
                }
                EditorGUILayout.PropertyField(_useSecurePreferences, new GUIContent("Secure Prefs"));
                if (_gameManager.SecurePreferences)
                {
                    EditorGUILayout.PropertyField(_preferencesPassPhrase, new GUIContent("Pass Phrase"));
                    EditorGUILayout.PropertyField(_autoConvertUnsecurePrefs, new GUIContent("Convert Unsecure Prefs"));
                }
                GUI.enabled = true;
            }
            _showAdvanced = EditorGUILayout.Foldout(_showAdvanced, "Advanced");
            if (_showAdvanced)
            {
                EditorGUILayout.PropertyField(_identifierBaseProperty);
                EditorGUILayout.PropertyField(_debugLevelProperty);
                EditorGUILayout.PropertyField(_referencePhysicalScreenHeightInInchesProperty);
                EditorGUILayout.PropertyField(_displayChangeCheckDelayProperty);
            }

            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }

        void DrawGameStructure()
        {
            EditorGUILayout.LabelField("Game Structure", EditorStyles.boldLabel);

            // Player setup
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_playerSetupModeProperty, new GUIContent("Player Setup"));
            if (_playerSetupModeProperty.enumValueIndex == 1 || _playerSetupModeProperty.enumValueIndex == 2)
            {
                EditorGUI.indentLevel += 1;
                _showPlayerAdvanced = EditorGUILayout.Foldout(_showPlayerAdvanced, "Advanced");
                if (_showPlayerAdvanced)
                {
                    EditorGUILayout.PropertyField(_playerCountProperty);
                }
                EditorGUI.indentLevel -= 1;
            }
            else if (_playerSetupModeProperty.enumValueIndex == 3)
                EditorGUILayout.HelpBox("Specified mode is not currently implemented for Players. Let us know if you need this functionality...", MessageType.Info);
            else if (_playerSetupModeProperty.enumValueIndex == 4)
                EditorGUILayout.HelpBox("MasterWithOverrides mode is not currently implemented for Players. Let us know if you need this functionality...", MessageType.Info);
            EditorGUILayout.EndVertical();

            // Worlds setup
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_worldSetupModeProperty, new GUIContent("World Setup"));
            if (_worldSetupModeProperty.enumValueIndex != 0) {
                // correct deprecated values - update setup mode and always set autocreate to false
                _autoCreateWorldsProperty.boolValue = false;

                if (_worldSetupModeProperty.enumValueIndex == 1)
                {
                    EditorGUILayout.PropertyField(_numberOfAutoCreatedWorldsProperty, new GUIContent("Count"));
                    EditorGUILayout.PropertyField(_worldUnlockModeProperty, new GUIContent("Unlock Mode"));
                    if (_gameManager.WorldUnlockMode == GameItem.UnlockModeType.Coins)
                        EditorGUILayout.PropertyField(_coinsToUnlockWorldsProperty);
                }
                else if (_worldSetupModeProperty.enumValueIndex == 2)
                {
                    EditorGUILayout.PropertyField(_numberOfAutoCreatedWorldsProperty, new GUIContent("Count"));
                }
                else if (_worldSetupModeProperty.enumValueIndex == 3)
                    EditorGUILayout.HelpBox("Specified mode is not currently implemented for Worlds. Let us know if you need this functionality...", MessageType.Info);
                else if (_worldSetupModeProperty.enumValueIndex == 4)
                    EditorGUILayout.HelpBox("MasterWithOverrides mode is not currently implemented for Worlds. Let us know if you need this functionality...", MessageType.Info);

                // per world level setup
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(_levelSetupModeProperty, new GUIContent("Level Setup"));
                if (_levelSetupModeProperty.enumValueIndex != 0)
                {
                    // correct deprecated values - update setup mode and always set autocreate to false
                    //_autoCreateLevelsProperty.boolValue = false;

                    if (_levelSetupModeProperty.enumValueIndex == 1)
                    {
                        EditorGUILayout.PropertyField(_numberOfAutoCreatedLevelsProperty, new GUIContent("Count"));
                        EditorGUILayout.PropertyField(_levelUnlockModeProperty, new GUIContent("Unlock Mode"));
                        if (_levelUnlockModeProperty.enumValueIndex == 2)
                            EditorGUILayout.PropertyField(_coinsToUnlockLevelsProperty);
                    }
                    else if (_levelSetupModeProperty.enumValueIndex == 2)
                    {
                        EditorGUILayout.PropertyField(_numberOfAutoCreatedLevelsProperty, new GUIContent("Count"));
                    }
                    else if (_levelSetupModeProperty.enumValueIndex == 3)
                        EditorGUILayout.HelpBox("Specified mode is not currently implemented for World Levels. Let us know if you need this functionality...", MessageType.Info);
                    else if (_levelSetupModeProperty.enumValueIndex == 4)
                        EditorGUILayout.HelpBox("MasterWithOverrides mode is not currently implemented for World Levels. Let us know if you need this functionality...", MessageType.Info);

                    // level number ranges
                    if (_levelSetupModeProperty.enumValueIndex == 1 || _levelSetupModeProperty.enumValueIndex == 2)
                    {
                        EditorGUILayout.LabelField("Level Number Ranges", EditorStyles.boldLabel);
                        for (var i = 0; i < _worldLevelNumbersProperty.arraySize; i++)
                        {
                            EditorGUILayout.PropertyField(_worldLevelNumbersProperty.GetArrayElementAtIndex(i),
                                new GUIContent("World " + i), true);
                        }
                        bool overlap = false;
                        for (var i = 0; i < _gameManager.WorldLevelNumbers.Length - 1; i++)
                        {
                            for (var j = i + 1; j < _gameManager.WorldLevelNumbers.Length; j++)
                            {
                                if (_gameManager.WorldLevelNumbers[i].Overlaps(_gameManager.WorldLevelNumbers[j]))
                                    overlap = true;
                            }
                        }
                        if (overlap) EditorGUILayout.HelpBox("Level ranges should not overlap!", MessageType.Error);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            // Standalone level setup
            if (_worldSetupModeProperty.enumValueIndex == 0)
            {
                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.PropertyField(_levelSetupModeProperty, new GUIContent("Level Setup"));
                if (_levelSetupModeProperty.enumValueIndex != 0)
                {
                    // correct deprecated values - update setup mode and always set autocreate to false
                    _autoCreateLevelsProperty.boolValue = false;

                    if (_levelSetupModeProperty.enumValueIndex == 1)
                    {
                        EditorGUILayout.PropertyField(_numberOfAutoCreatedLevelsProperty, new GUIContent("Count"));
                        EditorGUILayout.PropertyField(_levelUnlockModeProperty, new GUIContent("Unlock Mode"));
                        if (_levelUnlockModeProperty.enumValueIndex == 2)
                            EditorGUILayout.PropertyField(_coinsToUnlockLevelsProperty);
                    }
                    else if (_levelSetupModeProperty.enumValueIndex == 2)
                    {
                        EditorGUILayout.PropertyField(_numberOfAutoCreatedLevelsProperty, new GUIContent("Count"));
                    }
                    else if (_levelSetupModeProperty.enumValueIndex == 3)
                        EditorGUILayout.HelpBox("Specified mode is not currently implemented for Levels. Let us know if you need this functionality...", MessageType.Info);
                    else if (_levelSetupModeProperty.enumValueIndex == 4)
                    {
                        EditorGUILayout.PropertyField(_numberOfAutoCreatedLevelsProperty, new GUIContent("Count"));
                        EditorGUILayout.PropertyField(_levelMasterProperty, new GUIContent("Master"));
                        EditorGUI.indentLevel += 1;
                        _numberedLevelReferencesList.DoLayoutList();
                        EditorGUI.indentLevel -= 1;
                    }
                }
                EditorGUILayout.EndVertical();
            }

            // Standalone character setup
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_characterSetupModeProperty, new GUIContent("Character Setup"));
            if (_characterSetupModeProperty.enumValueIndex != 0)
            {
                // correct deprecated values - update setup mode and always set autocreate to false
                _autoCreateCharactersProperty.boolValue = false;

                if (_characterSetupModeProperty.enumValueIndex == 1)
                {
                    EditorGUILayout.PropertyField(_numberOfAutoCreatedCharactersProperty, new GUIContent("Count"));
                    EditorGUILayout.PropertyField(_characterUnlockModeProperty, new GUIContent("Unlock Mode"));
                    if (_characterUnlockModeProperty.enumValueIndex == 2)
                        EditorGUILayout.PropertyField(_coinsToUnlockCharactersProperty);
                }
                else if (_characterSetupModeProperty.enumValueIndex == 2)
                {
                    EditorGUILayout.PropertyField(_numberOfAutoCreatedCharactersProperty, new GUIContent("Count"));
                }
                else if (_characterSetupModeProperty.enumValueIndex == 3)
                    EditorGUILayout.HelpBox("Specified mode is not currently implemented for Characters. Let us know if you need this functionality...", MessageType.Info);
                else if (_characterSetupModeProperty.enumValueIndex == 4)
                    EditorGUILayout.HelpBox("MasterWithOverrides mode is not currently implemented for Characters. Let us know if you need this functionality...", MessageType.Info);
            }
            EditorGUILayout.EndVertical();
        }


        void DrawVariables()
        {
            EditorGUILayout.LabelField("Global Variables", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
#if !PREFS_EDITOR
            if (_gameManager.Variables.BoolVariables.Length > 0 || _gameManager.Variables.Vector2Variables.Length > 0 || _gameManager.Variables.Vector3Variables.Length > 0)
                EditorGUILayout.HelpBox("Note: Persisting of runtime changes to Bool, Vector2 and Vector3 variables is only supported with the PlayerPrefs integration. For more details see: Main Menu | Window | Game Framework | Integrations Window", MessageType.Info);
#endif
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_variablesProperty, true);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}