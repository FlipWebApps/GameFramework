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

    [System.Obsolete("Discontinued in favour of the cheat editor window")]
    public class CheatFunctions : MonoBehaviour {
        /// <summary>
        /// The key that needs to be pressed before any cheat input is processed.
        /// </summary>
        public KeyCode ActivationKeyCode = KeyCode.LeftShift;

        void Start()
        {
            Debug.LogWarning(
                "The CheatFunctions component used on " + gameObject.name + " is deprecated in favour of the cheat functions editor window.");

        }
 
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