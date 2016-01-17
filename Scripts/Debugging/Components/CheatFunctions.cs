//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.FreePrize.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Debugging.Components {

    /// <summary>
    /// Component for allowing various cheat functions to be called such as increasing score, resetting prefs etc..
    /// 
    /// You can override this class and HandleCheatInput() to provide your own custom cheat functions. Call base.HandleCheatInput()
    /// to run the standard ones in addition.
    /// </summary>
    public class CheatFunctions : MonoBehaviour {
        /// <summary>
        /// The key that needs to be pressed before any cheat input is processed.
        /// </summary>
        public KeyCode ActivationKeyCode = KeyCode.LeftShift;

        /// <summary>
        /// Check whether cheat input should be processed.
        /// </summary>
        public virtual void Update()
        {
            if (!MyDebug.IsDebugBuildOrEditor) return;

            if (UnityEngine.Input.GetKey(ActivationKeyCode))
            {
                HandleCheatInput();
            }
        }

        /// <summary>
        /// Override this to add your own cheat functions. 
        /// Call base().HandleCheatInput() to run the standard ones in addition.
        /// </summary>
        public virtual void HandleCheatInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.Instance.GetPlayer().Coins += 10;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.Instance.GetPlayer().Coins += 100;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.Instance.GetPlayer().Coins = 0;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameManager.Instance.GetPlayer().Score += 10;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameManager.Instance.GetPlayer().Score += 100;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
            {
                GameManager.Instance.GetPlayer().Score = 0;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                foreach (var level in GameManager.Instance.Levels.Items)
                {
                    level.IsUnlocked = true;
                    level.IsUnlockedAnimationShown = true;
                    level.UpdatePlayerPrefs();
                }
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                if (FreePrizeManager.IsActive)
                    FreePrizeManager.Instance.MakePrizeAvailable();
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }
    }
}