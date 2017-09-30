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

        [System.NonSerialized] readonly ComponentHelp[] _componentHelps =
        {
            // Advertising
            new ComponentHelp("AdMobManager", "Manager class for setting up and accessing AdMob functionality. Add this with GameScope for automatic setup of AdMob functionality", typeof(Advertising.AdMob.Components.AdMobManager), "Advertising"),
            new ComponentHelp("ShowHideAdvert (AdMob)", "Automatically show or hide an AdMob advert.", typeof(Advertising.AdMob.Components.ShowHideAdvert), "Advertising"),
            new ComponentHelp("OnButtonClickWatchAdvertForCoins (Unity Ads)", "Shows a 'watch advert for coins dialog' on button click.", typeof(Advertising.UnityAds.Components.OnButtonClickWatchAdvertForCoins), "Advertising"),
            new ComponentHelp("ShowAdvert (Unity Ads)", "Automatically show a Unity Ads advert. ", typeof(Advertising.UnityAds.Components.ShowAdvert), "Advertising"),

            // Animation
            new ComponentHelp("SetAnimatorStartValues", "Set a list of start values on an Animator.", typeof(Animation.Components.SetAnimatorStartValues), "Animation"),
            new ComponentHelp("SetBoolOnce", "Set an animation bool one time only for the entire lifetime of your game, optionally only after another RunOnceGameObject derived component (including this one) has been run.", typeof(Animation.Components.SetBoolOnce), "Animation"),
            new ComponentHelp("SetTriggerOnce", "Set an animation trigger one time only for the entire lifetime of your game, optionally only after another RunOnceGameObject derived component (including this one) has been run.", typeof(Animation.Components.SetTriggerOnce), "Animation"),

            // Audio
            new ComponentHelp("CopyBackgroundVolume", "Copies the global effect volume to an attached Audio Source.", typeof(Audio.Components.CopyBackgroundVolume), "Audio", "GameStructure"),
            new ComponentHelp("CopyGlobalEffectVolume", "Copies the global effect volume to an attached Audio Source.", typeof(Audio.Components.CopyGlobalEffectVolume), "Audio", "GameStructure"),
            new ComponentHelp("StartStopBackgroundMusic", "Automatically start or stop the global background music.", typeof(Audio.Components.StartStopBackgroundMusic), "Audio"),

            //Billing
            new ComponentHelp("EnableIfBillingEnabled", "Enabled or a disabled a gameobject based upon whether billing is enabled through the editor services window", typeof(Billing.Components.EnableIfBillingEnabled), "Billing"),
#if UNITY_PURCHASING
            new ComponentHelp("PaymentManager", "Provides code for setting up and callind in app billing. This derives from IStoreListener to enable it to receive messages from Unity Purchasing.", typeof(Billing.Components.PaymentManager), "Billing"),
#endif

            // Debugging
            new ComponentHelp("DummyGameLoop", "A simple dummy game loop with win and lose buttons that will allow you to test the structure and interfaces in your game.", typeof(Debugging.Components.DummyGameLoop), "Debugging", "GameStructure"),
            new ComponentHelp("LogToDisk", "Used for storing logging information to a file in debug build mode.", typeof(Debugging.Components.LogToDisk), "Debugging"),
            new ComponentHelp("LogToScreen", "Displays the current log onto the screen for debugging purposes", typeof(Debugging.Components.LogToScreen), "Debugging"),

            // Display
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

            // Facebook
            new ComponentHelp("EnableIfFacebookSDK", "Enables one of two gameobjects based upon whether the facebook SDK is installed", typeof(Facebook.Components.EnableIfFacebookSDK), "Facebook"),
            new ComponentHelp("EnableIfPermissionsGranted", "Enables one of two gameobjects based upon whether the specified facebook permissions are granted", typeof(Facebook.Components.EnableIfPermissionsGranted), "Facebook", "Social"),
            new ComponentHelp("EnableIfUserDataLoaded", "Enables one of two gameobjects based upon whether the users data is loaded", typeof(Facebook.Components.EnableIfUserDataLoaded), "Facebook", "Social"),
#if FACEBOOK_SDK
            new ComponentHelp("FacebookConnection", "Support for login / logout functionality.", typeof(Facebook.Components.FacebookConnection), "Facebook", "Social"),
            new ComponentHelp("FacebookManager", "Functionality to handle logging into facebook and interactions such as posting updates, inviting friends etc.", typeof(Facebook.Components.FacebookManager), "Facebook", "Social"),
#endif
            new ComponentHelp("OnButtonClickAppRequest", "Automatically hooks a button on click event to show a Facebook AppRequest dialog", typeof(Facebook.Components.OnButtonClickAppRequest), "Facebook", "UI", "Social"),
            new ComponentHelp("OnButtonClickShareLink", "Automatically hooks a button on click event to show a Facebook ShareLink dialog", typeof(Facebook.Components.OnButtonClickShareLink), "Facebook", "UI", "Social"),

            // Free prize
            new ComponentHelp("EnableIfPrizeAvailable", "Shows an enabled or a disabled gameobject based upon whether there is a free prize available", typeof(FreePrize.Components.EnableIfPrizeAvailable), "FreePrize"),
            new ComponentHelp("FreePrizeManager", "Manager class for handling the Free Prize status and configuration. Add this to a persistant gameobject in your scene if you are using Free Prize functionality", typeof(FreePrize.Components.FreePrizeManager), "FreePrize"),
            new ComponentHelp("OnButtonClickShowFreePrizeDialog", "Show the free prize dialog when the button is clicked.", typeof(FreePrize.Components.OnButtonClickShowFreePrizeDialog), "FreePrize", "UI"),
            new ComponentHelp("TimeToFreePrizeDisplay", "Shows the amount of time until the free prize is available", typeof(FreePrize.Components.TimeToFreePrizeDisplay), "FreePrize", "UI"),

            // GameObjects
            new ComponentHelp("AutoDestructWhenNoChildren", "Automatically destroys the GameObject when there are no children left.", typeof(GameObjects.Components.AutoDestructWhenNoChildren), "Display"),
            new ComponentHelp("EnableBasedUponFlag", "Enable one of two gameobjects based upon a given flag (preferences key)", typeof(GameObjects.Components.EnableBasedUponFlag), "Display"),
            new ComponentHelp("EnableComponentOnce", "Enables a component one time only. This can be useful for e.g. showing an animation the first time accesses a level.", typeof(GameObjects.Components.EnableComponentOnce), "Display"),
            new ComponentHelp("EnableGameObjectOnce", "Enables a gameobject one time only. This can be useful for e.g. showing information on using a game when a user first starts.", typeof(GameObjects.Components.EnableGameObjectOnce), "Display"),
            
            // GameStructure
            new ComponentHelp("GameManager", "A core component that holds and manages information about the game. GameManager is where you can setup the structure of your game and holdes other key information and functionality relating to Preferences, GameStructure, Display, Localisation, Audio, Messaging and more. Please see the online help for full information.", typeof(GameStructure.GameManager), "GameStructure", "Audio", "Player", "Level", "Character", "Localisation", "Display", "Messaging"),
            
            // GameStructure - Characters
            new ComponentHelp("BuyCharacterButton", "Buy button for Characters. Add this to a UI button for automatic handling of purchasing characters.", typeof(GameStructure.Characters.Components.BuyCharacterButton), "GameStructure", "Character", "Billing"),
            new ComponentHelp("CharacterButton", "Character details button. Provides support for a details button including selection, unlocking, IAP and more.", typeof(GameStructure.Characters.Components.CharacterButton), "GameStructure", "Character"),
            new ComponentHelp("CharacterGameItemContext", "Reference a Character that can then be used as the context by other components. This component can amonst others reference an item by number or the currently selecetd item. Other components can then use this context to determine what item to work with / display.", typeof(GameStructure.Characters.Components.CharacterGameItemContext), "GameStructure", "Character"),
            new ComponentHelp("CreateCharacterButtons", "Creates button instances for all Characters using a referenced prefab.", typeof(GameStructure.Characters.Components.CreateCharacterButtons), "GameStructure", "Character"),
            new ComponentHelp("InstantiateCharacterPrefab", "Create an instance of the specified prefab from teh referenced Character", typeof(GameStructure.Characters.Components.InstantiateCharacterPrefab), "GameStructure", "Character"),
            new ComponentHelp("SetGradientBackgroundFromCharacterVariable", "Set gradient background colors from a Character variable", typeof(GameStructure.Characters.Components.SetGradientBackgroundFromCharacterVariable), "Display", "GameStructure", "Character"),
            new ComponentHelp("SetImageToCharacterSprite", "Set an image to the specified sprite from the referenced Character", typeof(GameStructure.Characters.Components.SetImageToCharacterSprite), "GameStructure", "Character"),
            new ComponentHelp("SetPositionFromCharacterVariable", "Set transform position from a Character variable", typeof(GameStructure.Characters.Components.SetPositionFromCharacterVariable), "GameStructure", "Character"),
            new ComponentHelp("SetRotationFromCharacterVariable", "Set transform rotation from a Character variable", typeof(GameStructure.Characters.Components.SetRotationFromCharacterVariable), "GameStructure", "Character"),
            new ComponentHelp("SetScaleFromCharacterVariable", "Set transform scale from a Character variable", typeof(GameStructure.Characters.Components.SetScaleFromCharacterVariable), "GameStructure", "Character"),
            new ComponentHelp("SetSpriteRendererToCharacterSprite", "Set a SpriteRenderer to the specified sprite from the referenced Character", typeof(GameStructure.Characters.Components.SetSpriteRendererToCharacterSprite), "GameStructure", "Character"),
            new ComponentHelp("ShowCharacterInfo", "Show information about the referenced Character in a UI Text component. Information that can be displayed includes the name, number, description and value to unlock.", typeof(GameStructure.Characters.Components.ShowCharacterInfo), "GameStructure", "Character"),
            new ComponentHelp("ShowCharacterPrefab", "Create an instance of the specified prefab from a referenced Character", typeof(GameStructure.Characters.Components.ShowCharacterPrefab), "GameStructure", "Character"),
            new ComponentHelp("UnlockCharacterButton", "Unlock GameItem button for Characters. Add this to a UI button for automatic handling of unlocking characters.", typeof(GameStructure.Characters.Components.UnlockCharacterButton), "GameStructure", "Character"),

            // GameStructure - Colliders
            new ComponentHelp("CoinCollider", "Collider for increasing or decreasing a players / levels coins when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.CoinCollider), "GameStructure", "Collider", "Player", "Level"),
            new ComponentHelp("GenericCollider", "Generic collider for acting when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.GenericCollider), "GameStructure", "Collider", "Player"),
            new ComponentHelp("HealthCollider", "Health collider for increasing or decreasing a players health when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.HealthCollider), "GameStructure", "Collider", "Player"),
            new ComponentHelp("LoseLevelCollider", "Collider for losing the current level when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.LoseLevelCollider), "GameStructure", "Collider", "Player", "Level"),
            new ComponentHelp("PointsCollider", "Collider for increasing or decreasing a players / levels points when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.PointsCollider), "GameStructure", "Collider", "Player", "Level"),
            new ComponentHelp("StarCollider", "Collider for winning a star when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.StarCollider), "GameStructure", "Collider", "Player", "Level"),
            new ComponentHelp("WinLevelCollider", "Collider for winning the current level when a tagged gameobject touches the attached collider or trigger.", typeof(GameStructure.Colliders.WinLevelCollider), "GameStructure", "Collider", "Player", "Level"),

            // GameStructure - Game
            new ComponentHelp("EnableBasedUponGameUnlocked", "Shows one of two gameobjects based upon whether the game is unlocked.", typeof(GameStructure.Game.Components.EnableBasedUponGameUnlocked), "GameStructure"),

            // GameStructure - GenericGameItems
            new ComponentHelp("CreateGenericGameItems", "Creates button instances for all GenericGameItems using a referenced prefab.", typeof(GameStructure.GenericGameItems.Components.CreateGenericGameItemButtons), "GameStructure", "GenericGameItem"),
            new ComponentHelp("GenericGameItemButton", "GenericGameItem details button. Provides support for a details button including selection, unlocking, IAP and more.", typeof(GameStructure.GenericGameItems.Components.GenericGameItemButton), "GameStructure", "GenericGameItem"),
            new ComponentHelp("GenericGameItemManager", "Allows for automatic setup and referencing of a set of generic game items for your own usage.", typeof(GameStructure.GenericGameItems.Components.GenericGameItemManager), "GameStructure", "GenericGameItem"),
            new ComponentHelp("ShowGenericGameItemInfo", "Show information about the referenced GenericGameItem in a UI Text component. Information that can be displayed includes the name, number, description and value to unlock.", typeof(GameStructure.GenericGameItems.Components.ShowGenericGameItemInfo), "GameStructure", "GenericGameItem"),
            new ComponentHelp("UnlockGenericGameItemButton", "Unlock GameItem button for GenericGameItems. Add this to a UI button for automatic handling of unlocking GenericGameItems.", typeof(GameStructure.GenericGameItems.Components.UnlockGenericGameItemButton), "GameStructure", "GenericGameItem"),
            
            // GameStructure - Levels
            new ComponentHelp("BuyLevelButton", "Buy button for Levels. Add this to a UI button for automatic handling of purchasing Levels.", typeof(GameStructure.Levels.Components.BuyLevelButton), "GameStructure", "Level", "Billing"),
            new ComponentHelp("CreateLevelButtons", "Creates button instances for all Levels using a referenced prefab.", typeof(GameStructure.Levels.Components.CreateLevelButtons), "GameStructure", "Level"),
            new ComponentHelp("CreateStarIcons", "Creates instances of a star icon for the referenced Level using a referenced Prefab.", typeof(GameStructure.Levels.Components.CreateStarIcons), "GameStructure", "Level"),
            new ComponentHelp("EnableBasedUponNumberOfStarsWon", "Shows an enabled or a disabled gameobject based upon the number of stars the player has for the current level.", typeof(GameStructure.Levels.Components.EnableBasedUponNumberOfStarsWon), "GameStructure", "Level"),
            new ComponentHelp("InstantiateLevelPrefab", "Create an instance of the specified prefab from teh referenced Level", typeof(GameStructure.Levels.Components.InstantiateLevelPrefab), "GameStructure", "Level"),
            new ComponentHelp("LevelButton", "Level details button. Provides support for a details button including selection, unlocking, IAP and more.", typeof(GameStructure.Levels.Components.LevelButton), "GameStructure", "Level"),
            new ComponentHelp("LevelGameItemContext", "Reference a Level that can then be used as the context by other components. This component can amonst others reference an item by number or the currently selecetd item. Other components can then use this context to determine what item to work with / display.", typeof(GameStructure.Levels.Components.LevelGameItemContext), "GameStructure", "Level"),
            new ComponentHelp("SetGradientBackgroundFromLevelVariable", "Set gradient background colors from a Level variable", typeof(GameStructure.Levels.Components.SetGradientBackgroundFromLevelVariable), "Display", "GameStructure", "Level"),
            new ComponentHelp("SetImageToLevelSprite", "Set an image to the specified sprite from the referenced Level", typeof(GameStructure.Levels.Components.SetImageToLevelSprite), "GameStructure", "Level"),
            new ComponentHelp("SetPositionFromLevelVariable", "Set transform position from a Level variable", typeof(GameStructure.Levels.Components.SetPositionFromLevelVariable), "GameStructure", "Level"),
            new ComponentHelp("SetRotationFromLevelVariable", "Set transform rotation from a Level variable", typeof(GameStructure.Levels.Components.SetRotationFromLevelVariable), "GameStructure", "Level"),
            new ComponentHelp("SetScaleFromLevelVariable", "Set transform scale from a Level variable", typeof(GameStructure.Levels.Components.SetScaleFromLevelVariable), "GameStructure", "Level"),
            new ComponentHelp("SetSpriteRendererToLevelSprite", "Set a SpriteRenderer to the specified sprite from the referenced Level", typeof(GameStructure.Levels.Components.SetSpriteRendererToLevelSprite), "GameStructure", "Level"),
            new ComponentHelp("ShowLevelCoins", "Show the number of coins that have been collected for the referenced Level.", typeof(GameStructure.Levels.Components.ShowLevelCoins), "GameStructure", "Level"),
            new ComponentHelp("ShowLevelHighScore", "Show the high score for the referenced Level.", typeof(GameStructure.Levels.Components.ShowLevelHighScore), "GameStructure", "Level"),
            new ComponentHelp("ShowLevelInfo", "Show information about the referenced Level in a UI Text component. Information that can be displayed includes the name, number, description and value to unlock.", typeof(GameStructure.Levels.Components.ShowLevelInfo), "GameStructure", "Level"),
            new ComponentHelp("ShowLevelPrefab", "Create an instance of the specified prefab from a referenced Level", typeof(GameStructure.Levels.Components.ShowLevelPrefab), "GameStructure", "Level"),
            new ComponentHelp("ShowLevelScore", "Show the score for the referenced Level.", typeof(GameStructure.Levels.Components.ShowLevelScore), "GameStructure", "Level"),
            new ComponentHelp("StarsWonHandlerCoins", "A handler to set the number of stars won based upon the coins obtained for the referenced Level.", typeof(GameStructure.Levels.Components.StarsWonHandlerCoins), "GameStructure", "Level"),
            new ComponentHelp("StarsWonHandlerScore", "A handler to set the number of stars won based upon the amount of points obtained for the referenced Level.", typeof(GameStructure.Levels.Components.StarsWonHandlerScore), "GameStructure", "Level"),
            new ComponentHelp("UnlockLevelButton", "Unlock GameItem button for Levels. Add this to a UI button for automatic handling of unlocking Levels.", typeof(GameStructure.Levels.Components.UnlockLevelButton), "GameStructure", "Level"),
            
            // GameStructure - Players
            new ComponentHelp("CreateLivesIcons", "Creates instances of a life icon using a referenced prefab", typeof(GameStructure.Players.Components.CreateLivesIcons), "GameStructure", "Player"),
            new ComponentHelp("DecreaseLivesWhenHealthIsZero", "Decreases the number of lives that the player has when their health reaches zero.", typeof(GameStructure.Players.Components.DecreaseLivesWhenHealthIsZero), "GameStructure", "Player"),
            new ComponentHelp("EnableBasedUponNumberOfLives", "Shows one of two gameobjects based upon the number of lives the player has.", typeof(GameStructure.Players.Components.EnableBasedUponNumberOfLives), "GameStructure", "Player"),
            //new ComponentHelp("CreatePlayerButtons", "Creates button instances for all Players using a referenced prefab.", typeof(GameStructure.Players.Components.CreatePlayerButtons), "GameStructure", "Player"),
            new ComponentHelp("InstantiatePlayerPrefab", "Create an instance of the specified prefab from teh referenced Player", typeof(GameStructure.Players.Components.InstantiatePlayerPrefab), "GameStructure", "Player"),
            new ComponentHelp("OnLifeLostEnableGameobject", "Enable a specified gameobject when a life is lost.", typeof(GameStructure.Players.Components.OnLifeLostEnableGameobject), "GameStructure", "Player"),
            new ComponentHelp("PlayerButton", "Player details button. Provides support for a details button including selection, unlocking, IAP and more.", typeof(GameStructure.Players.Components.PlayerButton), "GameStructure", "Player"),
            new ComponentHelp("PlayerGameItemContext", "Reference a Player that can then be used as the context by other components. This component can amonst others reference an item by number or the currently selecetd item. Other components can then use this context to determine what item to work with / display.", typeof(GameStructure.Players.Components.PlayerGameItemContext), "GameStructure", "Player"),
            new ComponentHelp("SetHealth", "Sets the current players health to a specified value. Used for initialisation purposes.", typeof(GameStructure.Players.Components.SetHealth), "GameStructure", "Player"),
            new ComponentHelp("SetGradientBackgroundFromPlayerVariable", "Set gradient background colors from a Player variable", typeof(GameStructure.Players.Components.SetGradientBackgroundFromPlayerVariable), "Display", "GameStructure", "Player"),
            new ComponentHelp("SetImageToPlayerSprite", "Set an image to the specified sprite from the referenced Player", typeof(GameStructure.Players.Components.SetImageToPlayerSprite), "GameStructure", "Player"),
            new ComponentHelp("SetPositionFromPlayerVariable", "Set transform position from a Player variable", typeof(GameStructure.Players.Components.SetPositionFromPlayerVariable), "GameStructure", "Player"),
            new ComponentHelp("SetRotationFromPlayerVariable", "Set transform rotation from a Player variable", typeof(GameStructure.Players.Components.SetRotationFromPlayerVariable), "GameStructure", "Player"),
            new ComponentHelp("SetScaleFromPlayerVariable", "Set transform scale from a Player variable", typeof(GameStructure.Players.Components.SetScaleFromPlayerVariable), "GameStructure", "Player"),
            new ComponentHelp("SetLives", "Sets the current players number of lives. Used for initialisation purposes.", typeof(GameStructure.Players.Components.SetLives), "GameStructure", "Player"),
            new ComponentHelp("SetSpriteRendererToPlayerSprite", "Set a SpriteRenderer to the specified sprite from the referenced Player", typeof(GameStructure.Players.Components.SetSpriteRendererToPlayerSprite), "GameStructure", "Player"),
            new ComponentHelp("ShowCoins", "Show the number of coins that have been collected for the referenced Player.", typeof(GameStructure.Players.Components.ShowCoins), "GameStructure", "Player"),
            new ComponentHelp("ShowHealthImage", "Show the health that a player has by updating the fill amount on an image. The referenced image should have it's image type set to filled. This component will also optionally lerps the image color between two values.", typeof(GameStructure.Players.Components.ShowHealthImage), "GameStructure", "Player"),
            new ComponentHelp("ShowHighScore", "Show the high score for the referenced Player.", typeof(GameStructure.Players.Components.ShowHighScore), "GameStructure", "Player"),
            new ComponentHelp("ShowLives", "Show the number of lives that the referenced Player has", typeof(GameStructure.Players.Components.ShowLives), "GameStructure", "Player"),
            new ComponentHelp("ShowPlayerInfo", "Show information about the referenced Player in a UI Text component. Information that can be displayed includes the name, number, description and value to unlock.", typeof(GameStructure.Players.Components.ShowPlayerInfo), "GameStructure", "Player"),
            new ComponentHelp("ShowPlayerPrefab", "Create an instance of the specified prefab from a referenced Player", typeof(GameStructure.Players.Components.ShowPlayerPrefab), "GameStructure", "Player"),
            new ComponentHelp("ShowScore", "Show the score for the referenced Player.", typeof(GameStructure.Players.Components.ShowScore), "GameStructure", "Player"),
            
            // GameStructure - Worlds
            new ComponentHelp("BuyWorldButton", "Buy button for Worlds. Add this to a UI button for automatic handling of purchasing Worlds.", typeof(GameStructure.Worlds.Components.BuyWorldButton), "GameStructure", "World", "Billing"),
            new ComponentHelp("CreateWorldButtons", "Creates button instances for all Worlds using a referenced prefab.", typeof(GameStructure.Worlds.Components.CreateWorldButtons), "GameStructure", "World"),
            new ComponentHelp("InstantiateWorldPrefab", "Create an instance of the specified prefab from teh referenced World", typeof(GameStructure.Worlds.Components.InstantiateWorldPrefab), "GameStructure", "World"),
            new ComponentHelp("SetGradientBackgroundFromWorldVariable", "Set gradient background colors from a World variable", typeof(GameStructure.Worlds.Components.SetGradientBackgroundFromWorldVariable), "Display", "GameStructure", "World"),
            new ComponentHelp("SetImageToWorldSprite", "Set an image to the specified sprite from the referenced World", typeof(GameStructure.Worlds.Components.SetImageToWorldSprite), "GameStructure", "World"),
            new ComponentHelp("SetPositionFromWorldVariable", "Set transform position from a World variable", typeof(GameStructure.Worlds.Components.SetPositionFromWorldVariable), "GameStructure", "World"),
            new ComponentHelp("SetRotationFromWorldVariable", "Set transform rotation from a World variable", typeof(GameStructure.Worlds.Components.SetRotationFromWorldVariable), "GameStructure", "World"),
            new ComponentHelp("SetScaleFromWorldVariable", "Set transform scale from a World variable", typeof(GameStructure.Worlds.Components.SetScaleFromWorldVariable), "GameStructure", "World"),
            new ComponentHelp("SetSpriteRendererToWorldSprite", "Set a SpriteRenderer to the specified sprite from the referenced World", typeof(GameStructure.Worlds.Components.SetSpriteRendererToWorldSprite), "GameStructure", "World"),
            new ComponentHelp("ShowWorldInfo", "Show information about the referenced World in a UI Text component. Information that can be displayed includes the name, number, description and value to unlock.", typeof(GameStructure.Worlds.Components.ShowWorldInfo), "GameStructure", "World"),
            new ComponentHelp("ShowWorldPrefab", "Create an instance of the specified prefab from a referenced World", typeof(GameStructure.Worlds.Components.ShowWorldPrefab), "GameStructure", "World"),
            new ComponentHelp("UnlockWorldButton", "Unlock GameItem button for Worlds. Add this to a UI button for automatic handling of unlocking Worlds.", typeof(GameStructure.Worlds.Components.UnlockWorldButton), "GameStructure", "World"),
            new ComponentHelp("WorldButton", "World details button. Provides support for a details button including selection, unlocking, IAP and more.", typeof(GameStructure.Worlds.Components.WorldButton), "GameStructure", "World"),
            new ComponentHelp("WorldGameItemContext", "Reference a World that can then be used as the context by other components. This component can amonst others reference an item by number or the currently selecetd item. Other components can then use this context to determine what item to work with / display.", typeof(GameStructure.Worlds.Components.WorldGameItemContext), "GameStructure", "World"),

            // Helper
            new ComponentHelp("GenericCallbackFunctions", "A collection of generic callback functions that can be used from UI input,  animators or otherwise that didn't really fit anywhere else!", typeof(Helper.Components.GenericCallbackFunctions), "Helper"),

            // Input
            new ComponentHelp("OnButtonLoadScene", "Loads the given scene when a button is pressed optionally using any setup transitions (requires Beautiful Transitions)", typeof(Input.Components.OnButtonLoadScene), "Input", "GameStructure"),
            new ComponentHelp("OnEscapeLoadScene", "Loads the specified scene when the escape key or android back button is pressed", typeof(Input.Components.OnEscapeLoadScene), "Input", "GameStructure"),
            new ComponentHelp("OnEscapeQuit", "Quit the application when the escape key or android back button is pressed", typeof(Input.Components.OnEscapeQuit), "Input", "GameStructure"),
            new ComponentHelp("OnMouseClickOrTapLoadScene", "Loads the given scene when a mouse button is pressed or the screen is tapped", typeof(Input.Components.OnMouseClickOrTapLoadScene), "Input", "GameStructure"),

            // Localisation
            new ComponentHelp("EnableIfLanguage", "Enabled or a disabled a gameobject based upon whether the specified language matches that currently used by Game Framework", typeof(Localisation.Components.EnableIfLanguage), "Localisation"),
            new ComponentHelp("EnableIfMultipleLanguages", "Enabled or a disabled a gameobject based upon whether multiple languages are in use.", typeof(Localisation.Components.EnableIfMultipleLanguages), "Localisation"),
            new ComponentHelp("LocaliseImage", "Localises an image field based upon the given Key", typeof(Localisation.Components.LocaliseImage), "Localisation", "UI"),
            new ComponentHelp("LocaliseSpriteRenderer", "Localises an sprite renderer based upon the given Key", typeof(Localisation.Components.LocaliseSpriteRenderer), "Localisation"),
            new ComponentHelp("LocaliseText", "Localises a Text field based upon the given Key", typeof(Localisation.Components.LocaliseText), "Localisation"),
            new ComponentHelp("OnButtonClickSetLanguage", "Sets localisation to use the specified language when a button is clicked", typeof(Localisation.Components.OnButtonClickSetLanguage), "Localisation", "UI"),

            // Social
            new ComponentHelp("EnableBasedUponModulusOfGamesPlayed", "Shows one of two gameobjects based upon the modulus of the number of games played", typeof(Social.Components.EnableBasedUponModulusOfGamesPlayed), "Social", "GameStructure"),
            new ComponentHelp("EnableBasedUponModulusOfLevelsPlayed", "Shows one of two gameobjects based upon the modulus of the number of levels played", typeof(Social.Components.EnableBasedUponModulusOfLevelsPlayed), "Social", "GameStructure"),
            new ComponentHelp("EnableBasedUponNumberOfGamesPlayed", "Shows one of two gameobjects based upon the number of games played", typeof(Social.Components.EnableBasedUponNumberOfGamesPlayed), "Social", "GameStructure"),
            new ComponentHelp("EnableBasedUponNumberOfLevelsPlayed", "Shows one of two gameobjects based upon the number of levels played", typeof(Social.Components.EnableBasedUponNumberOfLevelsPlayed), "Social", "GameStructure"),
            new ComponentHelp("ShowGameFeedback", "Callback functions for showing the game feedback dialog", typeof(Social.Components.ShowGameFeedback), "Social"),

            // UI
            new ComponentHelp("OnButtonClickExit", "Quit the game when an attached button is clicked.", typeof(UI.Buttons.Components.OnButtonClickExit), "UI"),
            new ComponentHelp("OnButtonClickLoadScene", "Load the specified scene when an attached button is clicked.", typeof(UI.Buttons.Components.OnButtonClickLoadScene), "UI"),
            new ComponentHelp("OnButtonClickLoadUrl", "When an attached button is clicked then load the specificed Url", typeof(UI.Buttons.Components.OnButtonClickLoadUrl), "UI"),
            new ComponentHelp("SyncStateImageColors", "Syncronises UI Image colors agains changing button states. This can be used where you want button state changes to be reflected across multiple images such as when you have more complex buttons composed of multiple or child images.", typeof(UI.Buttons.Components.SyncStateImageColors), "UI"),
            new ComponentHelp("SyncStateTextColors", "Syncronises UI text, shadows and outline colors agains changing button states. This can be used where you want button state changes to be reflected across multiple Text components such as when you have more complex buttons composed of multiple or child Text components.", typeof(UI.Buttons.Components.SyncStateTextColors), "UI"),

            new ComponentHelp("DialogCallbackShowButtons", "Call back that will show the specified dialog buttons. This might typically be triggered from an animation to only show buttons dialog after an animation is shown. This can be used to stop the user clicking and exiting a dialog before we have shown what we want to show.", typeof(UI.Dialogs.Components.DialogCallbackShowButtons), "UI", "Dialog"),
            new ComponentHelp("DialogInstance", "Represents an instance of a dialog. Allows for animation and managing feedback state.", typeof(UI.Dialogs.Components.DialogInstance), "UI", "Dialog"),
            new ComponentHelp("DialogManager", "Provides dialog creation, display and management functionality.", typeof(UI.Dialogs.Components.DialogManager), "UI", "Dialog"),
            new ComponentHelp("GameOver", "Provides functionality for displaying and managing a game over dialog.", typeof(UI.Dialogs.Components.GameOver), "UI", "Dialog", "GameStructure"),
            new ComponentHelp("OnButtonClickPauseLevel", "Pauses a level when an attached button is clicked.", typeof(UI.Dialogs.Components.OnButtonClickPauseLevel), "UI", "Dialog", "GameStructure"),
            new ComponentHelp("OnButtonClickShowSettings", "Provides a method that you can add to a button click event for showing the settings dialog", typeof(UI.Dialogs.Components.OnButtonClickShowSettings), "UI", "Dialog", "GameStructure"),
            new ComponentHelp("OnButtonClickSwapDialogInstance", "Provides a method that you can add to a button click event for swapping between two dialog instances (windows)", typeof(UI.Dialogs.Components.OnButtonClickSwapDialogInstance), "UI", "Dialog"),
            new ComponentHelp("OnMouseClickOrTapSwapDialogInstance", "Swapping between two dialog instances (windows) when a mouse button is pressed or the screen is tapped anywhere on the screen", typeof(UI.Dialogs.Components.OnMouseClickOrTapSwapDialogInstance), "UI", "Dialog"),
            new ComponentHelp("PauseWindow", "Provides functionality for displaying and managing a pause window", typeof(UI.Dialogs.Components.PauseWindow), "UI", "Dialog", "GameStructure"),
            new ComponentHelp("Settings", "Provides functionality for displaying and managing a settings dialog that contains built in support for settings audio and effect volumes and restoring in app purchases.", typeof(UI.Dialogs.Components.Settings), "UI", "Dialog", "GameStructure", "Audio"),
            new ComponentHelp("OnButtonClickShowSettings", "Shows the given dialog a single time. A record is made of whether the specified dialog has been shown so that it is only shown one time including across game restarts. Use this component for showing things like help tips, guidelines or other one time information.", typeof(UI.Dialogs.Components.ShowDialogOnce), "UI", "Dialog"),

            new ComponentHelp("GradientText", "Provides a gradient effect for UI elements", typeof(UI.Other.Components.GradientText), "UI"),
            new ComponentHelp("ScrollRectEnsureVisible", "Provides support for scrolling a scrollrect to ensure that a specified item is displayed.", typeof(UI.Other.Components.ScrollRectEnsureVisible), "UI"),
            new ComponentHelp("TimeRemaining", "Provides a up / down counter based upon either a specific time target or that specified by the current Level", typeof(UI.Other.Components.TimeRemaining), "UI", "GameStructure"),

            // Weighting
            new ComponentHelp("DistanceWeight", "Used for holding a list of weight values for different distances.", typeof(Weighting.Components.DistanceWeight), "Weighting"),
        };

        // Add menu item for showing the window
        //[MenuItem("Window/Game Framework/Components Help", priority = 1)]
        // Temporarily removed - does this give value? If so needs updating with all components.
        public static void ShowWindow() 
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow<ComponentsHelpWindow>("Components Help", true);
        }


        void OnEnable()
        {
            _categories = new List<string>();
            foreach (var component in _componentHelps)
            {
                foreach (var category in component.Categories)
                    if (!_categories.Contains(category))
                        _categories.Add(category);
            }
            _categories.Sort();
            _categories.Insert(0, "All Categories");
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
            EditorGUILayout.HelpBox("This is a list of components available in Game Framework. There are also many other API's available to help you out. For full information on usage or otherwise please see the online help.", MessageType.Info);
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
                    if (string.IsNullOrEmpty(_searchString) || c.Name.IndexOf(_searchString, System.StringComparison.InvariantCultureIgnoreCase) != -1)
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
            GUILayout.Space(5);

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
