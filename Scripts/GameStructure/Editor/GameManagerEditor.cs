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

namespace GameFramework.GameStructure.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        GameManager _gameManager;
        bool _showLinks;
        bool _showAdvanced;
        bool _showPrefs;
        bool _showPlayerAdvanced;

        ReorderableList _supportedLanguagesList;

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

        SerializedProperty _supportedLanguagesProperty;

        SerializedProperty _defaultLivesProperty;
        SerializedProperty _playerCountProperty;

        SerializedProperty _autoCreateWorldsProperty;
        SerializedProperty _numberOfAutoCreatedWorldsProperty;
        SerializedProperty _loadWorldDatafromResources;
        SerializedProperty _coinsToUnlockWorldsProperty;
        SerializedProperty _worldUnlockModeProperty;
        SerializedProperty _worldLevelNumbersProperty;

        SerializedProperty _autoCreateLevelsProperty;
        SerializedProperty _numberOfAutoCreatedLevelsProperty;
        SerializedProperty _loadLevelDatafromResources;
        SerializedProperty _coinsToUnlockLevelsProperty;
        SerializedProperty _levelUnlockModeProperty;

        SerializedProperty _autoCreateCharactersProperty;
        SerializedProperty _numberOfAutoCreatedCharactersProperty;
        SerializedProperty _loadCharacterDatafromResources;
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

            _supportedLanguagesProperty = serializedObject.FindProperty("SupportedLanguages");

            _playerCountProperty = serializedObject.FindProperty("PlayerCount");
            _defaultLivesProperty = serializedObject.FindProperty("DefaultLives");

            _autoCreateWorldsProperty = serializedObject.FindProperty("AutoCreateWorlds");
            _numberOfAutoCreatedWorldsProperty = serializedObject.FindProperty("NumberOfAutoCreatedWorlds");
            _loadWorldDatafromResources = serializedObject.FindProperty("LoadWorldDatafromResources");
            _worldUnlockModeProperty = serializedObject.FindProperty("WorldUnlockMode");
            _coinsToUnlockWorldsProperty = serializedObject.FindProperty("CoinsToUnlockWorlds");
            _worldLevelNumbersProperty = serializedObject.FindProperty("WorldLevelNumbers");

            _autoCreateLevelsProperty = serializedObject.FindProperty("AutoCreateLevels");
            _numberOfAutoCreatedLevelsProperty = serializedObject.FindProperty("NumberOfAutoCreatedLevels");
            _loadLevelDatafromResources = serializedObject.FindProperty("LoadLevelDatafromResources");
            _levelUnlockModeProperty = serializedObject.FindProperty("LevelUnlockMode");
            _coinsToUnlockLevelsProperty = serializedObject.FindProperty("CoinsToUnlockLevels");

            _autoCreateCharactersProperty = serializedObject.FindProperty("AutoCreateCharacters");
            _numberOfAutoCreatedCharactersProperty = serializedObject.FindProperty("NumberOfAutoCreatedCharacters");
            _loadCharacterDatafromResources = serializedObject.FindProperty("LoadCharacterDatafromResources");
            _characterUnlockModeProperty = serializedObject.FindProperty("CharacterUnlockMode");
            _coinsToUnlockCharactersProperty = serializedObject.FindProperty("CoinsToUnlockCharacters");

            _supportedLanguagesList = new ReorderableList(serializedObject, _supportedLanguagesProperty, true, true, true, true);
            _supportedLanguagesList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Supported Languages");
            };
            _supportedLanguagesList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = _supportedLanguagesList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element, GUIContent.none);
                };
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            serializedObject.Update();

            DrawGameDetails();
            DrawGameStructure();
            DrawLocalisation();

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
            EditorGUILayout.PropertyField(_defaultLivesProperty);
            EditorGUI.indentLevel += 1;
            _showPlayerAdvanced = EditorGUILayout.Foldout(_showPlayerAdvanced, "Advanced");
            if (_showPlayerAdvanced)
            {
                EditorGUILayout.PropertyField(_playerCountProperty);
            }
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();

            // Worlds setup
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_autoCreateWorldsProperty);
            if (_gameManager.AutoCreateWorlds)
            {
                EditorGUILayout.PropertyField(_numberOfAutoCreatedWorldsProperty, new GUIContent("Count"));
                EditorGUILayout.PropertyField(_loadWorldDatafromResources, new GUIContent("Load From Resources"));
                EditorGUILayout.PropertyField(_worldUnlockModeProperty, new GUIContent("Unlock Mode"));
                if (_gameManager.WorldUnlockMode == GameItem.UnlockModeType.Coins)
                    EditorGUILayout.PropertyField(_coinsToUnlockWorldsProperty);

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(_autoCreateLevelsProperty);
                if (_gameManager.AutoCreateLevels)
                {
                    EditorGUILayout.PropertyField(_loadLevelDatafromResources, new GUIContent("Load From Resources"));
                    EditorGUILayout.PropertyField(_levelUnlockModeProperty, new GUIContent("Unlock Mode"));
                    if (_gameManager.LevelUnlockMode == GameItem.UnlockModeType.Coins)
                        EditorGUILayout.PropertyField(_coinsToUnlockLevelsProperty);

                    // level number ranges
                    EditorGUILayout.LabelField("Level Number Ranges", EditorStyles.boldLabel);
                    for (var i = 0; i < _worldLevelNumbersProperty.arraySize; i++)
                    {
                        EditorGUILayout.PropertyField(_worldLevelNumbersProperty.GetArrayElementAtIndex(i), new GUIContent("World " + i), true);
                    }
                    bool overlap = false;
                    for (var i = 0; i < _gameManager.WorldLevelNumbers.Length - 1; i++)
                    {
                        for (var j = i+1; j < _gameManager.WorldLevelNumbers.Length; j++)
                        {
                            if (_gameManager.WorldLevelNumbers[i].Overlaps(_gameManager.WorldLevelNumbers[j]))
                                overlap = true;
                        }
                    }
                    if (overlap) EditorGUILayout.HelpBox("Level ranges should not overlap!", MessageType.Error);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            // Standalone level setup
            if (!_gameManager.AutoCreateWorlds)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(_autoCreateLevelsProperty);
                if (_gameManager.AutoCreateLevels)
                {
                    EditorGUILayout.PropertyField(_numberOfAutoCreatedLevelsProperty, new GUIContent("Count"));
                    EditorGUILayout.PropertyField(_loadLevelDatafromResources, new GUIContent("Load From Resources"));
                    EditorGUILayout.PropertyField(_levelUnlockModeProperty, new GUIContent("Unlock Mode"));
                    if (_gameManager.LevelUnlockMode == GameItem.UnlockModeType.Coins)
                        EditorGUILayout.PropertyField(_coinsToUnlockLevelsProperty);
                }
                EditorGUILayout.EndVertical();
            }

            // Standalone character setup
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.PropertyField(_autoCreateCharactersProperty);
            if (_gameManager.AutoCreateCharacters)
            {
                EditorGUILayout.PropertyField(_numberOfAutoCreatedCharactersProperty, new GUIContent("Count"));
                EditorGUILayout.PropertyField(_loadCharacterDatafromResources, new GUIContent("Load From Resources"));
                EditorGUILayout.PropertyField(_characterUnlockModeProperty, new GUIContent("Unlock Mode"));
                if (_gameManager.CharacterUnlockMode == GameItem.UnlockModeType.Coins)
                    EditorGUILayout.PropertyField(_coinsToUnlockCharactersProperty);
            }
            EditorGUILayout.EndVertical();
        }

        void DrawLocalisation()
        {
            EditorGUILayout.LabelField("Localisation", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel += 1;
            //EditorList.Show(_supportedLanguagesProperty, EditorListOption.ListLabel | EditorListOption.Buttons | EditorListOption.AlwaysShowAddButton, addButtonText: "Add Language", addButtonToolTip: "Add Language");
            _supportedLanguagesList.DoLayoutList();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }
    }
}