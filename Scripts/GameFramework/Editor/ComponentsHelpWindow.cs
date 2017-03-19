//----------------------------------------------
// Flip Web Apps: Prefs Editor
// Copyright © 2016-2017 Flip Web Apps / Mark Hewitt
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
using GameFramework.Audio.Components;
using GameFramework.EditorExtras.Editor;
using System.Collections.Generic;
using GameFramework.GameFramework;

namespace GameFramework.Localisation.Editor
{
    /// <summary>
    /// Editor window for component help and quick access.
    /// </summary>
    public class ComponentsHelpWindow : EditorWindow
    {
        Vector2 _scrollPosition = Vector2.zero;

        string _searchString;

        [System.NonSerialized]
        List<string> _categories = new List<string>();
        int _selectedCategory;

        [System.NonSerialized]
        ComponentHelp[] _componentHelps =
        {
            new ComponentHelp("SetAnimatorStartValues", "Set a list of start values on an Animator.", typeof(Animation.Components.SetAnimatorStartValues), "Animation"),
            new ComponentHelp("SetBoolOnce", "Set an animation bool one time only for the entire lifetime of your game, optionally only after another RunOnceGameObject derived component (including this one) has been run.", typeof(Animation.Components.SetBoolOnce), "Animation"),
            new ComponentHelp("SetTriggerOnce", "Set an animation trigger one time only for the entire lifetime of your game, optionally only after another RunOnceGameObject derived component (including this one) has been run.", typeof(Animation.Components.SetTriggerOnce), "Animation"),
            new ComponentHelp("CopyBackgroundVolume", "Copies the global effect volume to an attached Audio Source.", typeof(CopyBackgroundVolume), "Audio", "GameStructure"),
            new ComponentHelp("CopyGlobalEffectVolume", "Copies the global effect volume to an attached Audio Source.", typeof(CopyGlobalEffectVolume), "Audio", "GameStructure"),
            new ComponentHelp("StartStopBackgroundMusic", "Automatically start or stop the global background music.", typeof(StartStopBackgroundMusic), "Audio"),
            new ComponentHelp("EnableIfBillingEnabled", "Enabled or a disabled a gameobject based upon whether billing is enabled through the editor services window", typeof(Billing.Components.EnableIfBillingEnabled), "Billing"),
#if UNITY_PURCHASING
            new ComponentHelp("PaymentManager", "Provides code for setting up and callind in app billing. This derives from IStoreListener to enable it to receive messages from Unity Purchasing.", typeof(Billing.Components.PaymentManager), "Billing"),
#endif
            new ComponentHelp("DummyGameLoop", "A simple dummy game loop with win and lose buttons that will allow you to test the structure and interfaces in your game.", typeof(Debugging.Components.DummyGameLoop), "Debugging", "GameStructure"),
            new ComponentHelp("LogToDisk", "Used for storing logging information to a file in debug build mode.", typeof(Debugging.Components.LogToDisk), "Debugging"),
            new ComponentHelp("LogToScreen", "Displays the current log onto the screen for debugging purposes", typeof(Debugging.Components.LogToScreen), "Debugging"),
            new ComponentHelp("GradientBackground", "Creates a background for the camera, which is a simple gradient blend between two colors.", typeof(Display.Other.Components.GradientBackground), "Display"),
            new ComponentHelp("GradientBackgroundSplitMiddle", "Creates a background for the camera, which is a simple gradient blend between three colors.", typeof(Display.Other.Components.GradientBackgroundSplitMiddle), "Display"),
            new ComponentHelp("LevelOfDetail", "Simple level of detail component that enables different gameobjects based upon the distance from the camera to help improve performance.", typeof(Display.Other.Components.LevelOfDetail), "Display"),
            new ComponentHelp("ScrollMaterialComponent", "Used for scrolling a material within the gameobjects renderer", typeof(Display.Other.Components.ScrollMaterialComponent), "Display"),
            new ComponentHelp("SetCubeUvs", "Set the UV co-ordinates of a cube", typeof(Display.Other.Components.SetCubeUvs), "Display"),
            new ComponentHelp("SetQuadUvs", "Set the UV co-ordinates of a quad", typeof(Display.Other.Components.SetQuadUvs), "Display"),
            new ComponentHelp("SetSortingLayer", "Set the attached Renderer's sorting layer name and sorting order", typeof(Display.Other.Components.SetSortingLayer), "Display"),
            new ComponentHelp("AutoDestructWhenParticleSystemFinished", "Destroy or disable the specified gameobject once the particle system is finished playing.", typeof(Display.Particles.Components.AutoDestructWhenParticleSystemFinished), "Display"),
            new ComponentHelp("ParticlePlayer", "Provides a callback for creating a new particle system that you can hook up to event handlers.", typeof(Display.Particles.Components.ParticlePlayer), "Display"),
            new ComponentHelp("AlignScreenBounds", "Align this game object with the screen bounds. Useful when catering for dynamic screen sizes.", typeof(Display.Placement.Components.AlignScreenBounds), "Display"),
            new ComponentHelp("CenterInCamera", "Center this gameobject within the main camera.", typeof(Display.Placement.Components.CenterInCamera), "Display"),
            new ComponentHelp("FixedMovement", "Move this gameobject at a given rate.", typeof(Display.Placement.Components.FixedMovement), "Display"),
            new ComponentHelp("FixedRotation", "Rotate this gameobject at a given rate.", typeof(Display.Placement.Components.FixedRotation), "Display"),
            new ComponentHelp("LookAtTransform", "Rotate so that the gameobject looks towards the specified transform", typeof(Display.Placement.Components.LookAtTransform), "Display"),
            new ComponentHelp("MoveWithTransform", "Maintain a fixed distance from the specified transform", typeof(Display.Placement.Components.MoveWithTransform), "Display"),
            new ComponentHelp("ObjectDestroyer", "Automatically destroys gameobjects that collide with this one.", typeof(Display.Placement.Components.ObjectDestroyer), "Display"),
            new ComponentHelp("EnableIfFacebookSDK", "Enables one of two gameobjects based upon whether the facebook SDK is installed", typeof(Facebook.Components.EnableIfFacebookSDK), "Facebook"),
            new ComponentHelp("EnableIfPermissionsGranted", "Enables one of two gameobjects based upon whether the specified facebook permissions are granted", typeof(Facebook.Components.EnableIfPermissionsGranted), "Facebook", "Social"),
            new ComponentHelp("EnableIfUserDataLoaded", "Enables one of two gameobjects based upon whether the users data is loaded", typeof(Facebook.Components.EnableIfUserDataLoaded), "Facebook", "Social"),
#if FACEBOOK_SDK
            new ComponentHelp("FacebookConnection", "Support for login / logout functionality.", typeof(Facebook.Components.FacebookConnection), "Facebook", "Social"),
            new ComponentHelp("FacebookManager", "Functionality to handle logging into facebook and interactions such as posting updates, inviting friends etc.", typeof(Facebook.Components.FacebookManager), "Facebook", "Social"),
#endif
            new ComponentHelp("OnButtonClickAppRequest", "Automatically hooks a button on click event to show a Facebook AppRequest dialog", typeof(Facebook.Components.OnButtonClickAppRequest), "Facebook", "UI", "Social"),
            new ComponentHelp("OnButtonClickShareLink", "Automatically hooks a button on click event to show a Facebook ShareLink dialog", typeof(Facebook.Components.OnButtonClickShareLink), "Facebook", "UI", "Social"),
            new ComponentHelp("EnableIfPrizeAvailable", "Shows an enabled or a disabled gameobject based upon whether there is a free prize available", typeof(FreePrize.Components.EnableIfPrizeAvailable), "FreePrize"),
            new ComponentHelp("FreePrizeManager", "Manager class for handling the Free Prize status and configuration. Add this to a persistant gameobject in your scene if you are using Free Prize functionality", typeof(FreePrize.Components.FreePrizeManager), "FreePrize"),
            new ComponentHelp("OnButtonClickShowFreePrizeDialog", "Show the free prize dialog when the button is clicked.", typeof(FreePrize.Components.OnButtonClickShowFreePrizeDialog), "FreePrize", "UI"),
            new ComponentHelp("TimeToFreePrizeDisplay", "Shows the amount of time until the free prize is available", typeof(FreePrize.Components.TimeToFreePrizeDisplay), "FreePrize", "UI"),
            new ComponentHelp("AutoDestructWhenNoChildren", "Automatically destroys the GameObject when there are no children left.", typeof(GameObjects.Components.AutoDestructWhenNoChildren), "Display"),
            new ComponentHelp("EnableBasedUponFlag", "Enable one of two gameobjects based upon a given flag (preferences key)", typeof(GameObjects.Components.EnableBasedUponFlag), "Display"),
            new ComponentHelp("EnableComponentOnce", "Enables a component one time only. This can be useful for e.g. showing an animation the first time accesses a level.", typeof(GameObjects.Components.EnableComponentOnce), "Display"),
            new ComponentHelp("EnableGameObjectOnce", "Enables a gameobject one time only. This can be useful for e.g. showing information on using a game when a user first starts.", typeof(GameObjects.Components.EnableGameObjectOnce), "Display"),
            
            //TODO GAMESTRUCTURE:

            new ComponentHelp("GenericCallbackFunctions", "A collection of generic callback functions that can be used from UI input,  animators or otherwise that didn't really fit anywhere else!", typeof(Helper.Components.GenericCallbackFunctions), "Helper"),
            new ComponentHelp("OnEscapeLoadScene", "Loads the specified scene when the escape key or android back button is pressed", typeof(Input.Components.OnEscapeLoadScene), "Input", "GameStructure"),
            new ComponentHelp("OnEscapeQuit", "Quit the application when the escape key or android back button is pressed", typeof(Input.Components.OnEscapeQuit), "Input", "GameStructure"),
            new ComponentHelp("OnMouseClickOrTapLoadScene", "Loads the given scene when a mouse button is pressed or the screen is tapped", typeof(Input.Components.OnMouseClickOrTapLoadScene), "Input", "GameStructure"),
            new ComponentHelp("EnableIfLanguage", "Enabled or a disabled a gameobject based upon whether the specified language matches that currently used by Game Framework", typeof(Localisation.Components.EnableIfLanguage), "Localisation"),
            new ComponentHelp("EnableIfMultipleLanguages", "Enabled or a disabled a gameobject based upon whether multiple languages are in use.", typeof(Localisation.Components.EnableIfMultipleLanguages), "Localisation"),
            new ComponentHelp("LocaliseImage", "Localises an image field based upon the given Key", typeof(Localisation.Components.LocaliseImage), "Localisation", "UI"),
            new ComponentHelp("LocaliseSpriteRenderer", "Localises an sprite renderer based upon the given Key", typeof(Localisation.Components.LocaliseSpriteRenderer), "Localisation"),
            new ComponentHelp("LocaliseText", "Localises a Text field based upon the given Key", typeof(Localisation.Components.LocaliseText), "Localisation"),
            new ComponentHelp("OnButtonClickSetLanguage", "Sets localisation to use the specified language when a button is clicked", typeof(Localisation.Components.OnButtonClickSetLanguage), "Localisation", "UI"),
            new ComponentHelp("EnableBasedUponModulusOfGamesPlayed", "Shows one of two gameobjects based upon the modulus of the number of games played", typeof(Social.Components.EnableBasedUponModulusOfGamesPlayed), "Social", "GameStructure"),
            new ComponentHelp("EnableBasedUponModulusOfLevelsPlayed", "Shows one of two gameobjects based upon the modulus of the number of levels played", typeof(Social.Components.EnableBasedUponModulusOfLevelsPlayed), "Social", "GameStructure"),
            new ComponentHelp("EnableBasedUponNumberOfGamesPlayed", "Shows one of two gameobjects based upon the number of games played", typeof(Social.Components.EnableBasedUponNumberOfGamesPlayed), "Social", "GameStructure"),
            new ComponentHelp("EnableBasedUponNumberOfLevelsPlayed", "Shows one of two gameobjects based upon the number of levels played", typeof(Social.Components.EnableBasedUponNumberOfLevelsPlayed), "Social", "GameStructure"),
            new ComponentHelp("ShowGameFeedback", "Callback functions for showing the game feedback dialog", typeof(Social.Components.ShowGameFeedback), "Social"),

            //TODO UI:
            new ComponentHelp("OnButtonClickExit", "Quit the game when an attached button is clicked.", typeof(UI.Buttons.Components.OnButtonClickExit), "UI"),
            new ComponentHelp("OnButtonClickLoadScene", "Load the specified scene when an attached button is clicked.", typeof(UI.Buttons.Components.OnButtonClickLoadScene), "UI"),
            new ComponentHelp("OnButtonClickLoadUrl", "When an attached button is clicked then load the specificed Url", typeof(UI.Buttons.Components.OnButtonClickLoadUrl), "UI"),
            new ComponentHelp("SyncStateImageColors", "Syncronises UI Image colors agains changing button states. This can be used where you want button state changes to be reflected across multiple images such as when you have more complex buttons composed of multiple or child images.", typeof(UI.Buttons.Components.SyncStateImageColors), "UI"),
            new ComponentHelp("SyncStateTextColors", "Syncronises UI text, shadows and outline colors agains changing button states. This can be used where you want button state changes to be reflected across multiple Text components such as when you have more complex buttons composed of multiple or child Text components.", typeof(UI.Buttons.Components.SyncStateTextColors), "UI"),
            new ComponentHelp("DialogCallbackShowButtons", "Call back that will show the specified dialog buttons. This might typically be triggered from an animation to only show buttons dialog after an animation is shown. This can be used to stop the user clicking and exiting a dialog before we have shown what we want to show.", typeof(UI.Dialogs.Components.DialogCallbackShowButtons), "UI", "Dialog"),
            new ComponentHelp("DialogInstance", "Represents an instance of a dialog. Allows for animation and managing feedback state.", typeof(UI.Dialogs.Components.DialogInstance), "UI", "Dialog"),
            new ComponentHelp("DialogManager", "Provides dialog creation, display and management functionality.", typeof(UI.Dialogs.Components.DialogManager), "UI", "Dialog"),
            new ComponentHelp("GameOver", "Provides the basis for a game over dialog.", typeof(UI.Dialogs.Components.GameOver), "UI", "Dialog", "GameStructure"),
            new ComponentHelp("OnButtonClickPauseLevel", "Pauses a level when an attached button is clicked.", typeof(UI.Dialogs.Components.OnButtonClickPauseLevel), "UI", "Dialog", "GameStructure"),
            new ComponentHelp("GradientText", "Provides a gradient effect for UI elements", typeof(UI.Other.Components.GradientText), "UI"),
            new ComponentHelp("ScrollRectEnsureVisible", "Provides support for scrolling a scrollrect to ensure that a specified item is displayed.", typeof(UI.Other.Components.ScrollRectEnsureVisible), "UI"),
            new ComponentHelp("TimeRemaining", "Provides a up / down counter based upon either a specific time target or that specified by the current Level", typeof(UI.Other.Components.TimeRemaining), "UI", "GameStructure"),
            new ComponentHelp("DistanceWeight", "Used for holding a list of weight values for different distances.", typeof(Weighting.Components.DistanceWeight), "Weighting"),
        };

        // Add menu item for showing the window
        [MenuItem("Window/Game Framework/Components Help", priority = 1)]
        public static void ShowWindow() 
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow<ComponentsHelpWindow>("Components Help", true);
        }


        void OnEnable()
        {
            _categories = new List<string>();
            _categories.Add("All Categories");
            foreach (var component in _componentHelps)
            {
                foreach (var category in component.Categories)
                    if (!_categories.Contains(category))
                        _categories.Add(category);
            }
            _categories.Sort();
        }

        /// <summary>
        /// Draw the GUI
        /// </summary>
        void OnGUI()
        {
            DrawToolbar();
            GUILayout.Space(5);
            DrawEntries();
        }


        /// <summary>
        /// Draws the toolbar.
        /// </summary>
        void DrawToolbar()
        {
            EditorGUILayout.HelpBox("Note: This list is currently missing Game Structure components - these will be added soon. Use the Inspector Add Component button for now.", MessageType.Info);
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

            _searchString = EditorGUILayout.TextField(_searchString, GuiStyles.ToolbarSearchField, GUILayout.ExpandWidth(true));
            if (GUILayout.Button(GUIContent.none, GuiStyles.ToolbarSearchFieldCancel, GUILayout.ExpandWidth(true)))
            {

                _searchString = "";
                GUIUtility.keyboardControl = 0;
            }
            //EditorGUILayout.LabelField("Category:", EditorStyles.label, GUILayout.MaxWidth(EditorStyles.label.CalcSize(new GUIContent("Category:")).x));
            _selectedCategory = EditorGUILayout.Popup(_selectedCategory, _categories.ToArray(), EditorStyles.toolbarPopup, GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            if (EditorHelper.ButtonTrimmed("Documentation", EditorStyles.toolbarButton))
            {
                GameFrameworkHelper.ShowDocumentation();
            }
            if (EditorHelper.ButtonTrimmed("API Docs.", EditorStyles.toolbarButton))
            {
                GameFrameworkHelper.ShowAPIDocumentation();
            }

            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Draw the entries
        /// </summary>
        void DrawEntries()
        {
            List<ComponentHelp> filtered = new List<ComponentHelp>();
            foreach (var c in _componentHelps)
            {
                if (_selectedCategory == 0 || System.Array.IndexOf(c.Categories, _categories[_selectedCategory]) != -1)
                {
                    if (_searchString == null || _searchString.Length == 0 || c.Name.IndexOf(_searchString, System.StringComparison.InvariantCultureIgnoreCase) != -1)
                        filtered.Add(c);
                }
            }
            filtered.Sort((x, y) => x.Name.CompareTo(y.Name));

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var c in filtered)
                ShowComponent(c);
            EditorGUILayout.EndScrollView();
        }


        void ShowComponent(ComponentHelp componentHelp)
        {
            EditorGUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            GUILayout.Label(componentHelp.Name, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUI.enabled = Selection.activeGameObject != null;
            if (EditorHelper.LinkButton("Add Component", true))
                Selection.activeGameObject.AddComponent(componentHelp.Type);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(componentHelp.Description, MessageType.None);
            GUILayout.EndHorizontal();

            GUILayout.Label("Categories: " + string.Join(", ", componentHelp.Categories));

            EditorGUILayout.EndVertical();
        }

        public class ComponentHelp
        {
            public string Name;
            public string[] Categories;
            public string Description;
            public string ClassName;
            public System.Type Type;

            public ComponentHelp(string name, string description, System.Type type, params string[] categories)
            {
                Name = name;
                Description = description;
                Type = type;
                Categories = categories;
            }
        }

    }
}
