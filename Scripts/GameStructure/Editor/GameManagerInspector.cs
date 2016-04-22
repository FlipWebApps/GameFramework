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

using UnityEngine;
using System.Collections;
using UnityEditor;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.EditorExtras.Editor;

namespace FlipWebApps.GameFramework.Scripts.GameStructure
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : Editor
    {
        GameManager _gameManager;
        bool showLinks = false;
        bool showAdvanced = false;
        bool showPlayerAdvanced = false;

        SerializedProperty _gameNameProperty;
        SerializedProperty _playWebUrlProperty;
        SerializedProperty _playMarketUrlProperty;
        SerializedProperty _iOSWebUrlProperty;
        SerializedProperty _debugLevelProperty;
        SerializedProperty _referencePhysicalScreenHeightInInchesProperty;
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
            showLinks = EditorGUILayout.Foldout(showLinks, "Links");
            if (showLinks)
            {
                EditorGUILayout.PropertyField(_playWebUrlProperty);
                EditorGUILayout.PropertyField(_playMarketUrlProperty);
                EditorGUILayout.PropertyField(_iOSWebUrlProperty);
            }
            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced");
            if (showAdvanced)
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
            showPlayerAdvanced = EditorGUILayout.Foldout(showPlayerAdvanced, "Advanced");
            if (showPlayerAdvanced)
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
            EditorList.Show(_supportedLanguagesProperty, EditorListOption.ListLabel | EditorListOption.Buttons | EditorListOption.AlwaysShowAddButton, addButtonText: "Add Language", addButtonToolTip: "Add Language");
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();
        }
    }
}