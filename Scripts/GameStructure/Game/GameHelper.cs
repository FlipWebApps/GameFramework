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

using GameFramework.Preferences;
using UnityEngine;

namespace GameFramework.GameStructure.Game
{
    /// <summary>
    /// This class contains helper functions about the overall game structure. For more specific use of
    /// individual items such as worlds and levels see the corresponding subfolder.
    /// </summary>
    public class GameHelper : MonoBehaviour
    {
        /// <summary>
        /// Returns whether we are using worlds.
        /// </summary>
        /// <returns></returns>
        public static bool IsUsingWorlds()
        {
            return GameManager.Instance.Worlds.Items.Length != 0;
        }

        /// <summary>
        /// Returns whether this is the last level in the whole game, based upon the current state. See also
        /// Player.IsGameWon to test whether the game has already been completed before.
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentLevelLastInGame()
        {
            var nextLevel = GameManager.Instance.Levels.GetNextItem();

            return (!IsUsingWorlds() && nextLevel == null) ||
                (IsUsingWorlds() && GameManager.Instance.Worlds.GetNextItem() == null && nextLevel == null);
        }


        /// <summary>
        /// Returns whether this is the last level in the current world based upon the current state. The
        /// game might also be complete if this returns true, so check IsCurrentLevelLastInGame() if you want to 
        /// distinguish between the two.
        /// See also IsNextWorldUnlocked to test whether the next world has already been unlocked before.
        /// 
        /// If we are not using worlds then this will always return false;
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentLevelLastInWorld()
        {
            if (!IsUsingWorlds()) return false;

            var nextLevel = GameManager.Instance.Levels.GetNextItem();

            return nextLevel == null;
        }


        /// <summary>
        /// Returns whether the next world is unlocked.
        /// 
        /// If we are not using worlds then this will always return false;
        /// </summary>
        /// <returns></returns>
        public static bool IsNextWorldUnlocked()
        {
            if (!IsUsingWorlds()) return false;

            var nextWorld = GameManager.Instance.Worlds.GetNextItem();

            return nextWorld == null ? false : nextWorld.IsUnlocked;
        }


        /// <summary>
        /// Update any other items as needed once the current level is won. This includes unlocking subsequent worlds
        /// and levels if so necessary.
        /// </summary>
        public static void ProcessCurrentLevelComplete()
        {
            var player = GameManager.Instance.Player;
            var nextWorld = IsUsingWorlds() ? GameManager.Instance.Worlds.GetNextItem() : null; // might be null if not using worlds
            var nextLevel = GameManager.Instance.Levels.GetNextItem();

            // is the game won
            if (IsCurrentLevelLastInGame())
            {
                player.IsGameWon = true;
                player.UpdatePlayerPrefs();
            }

            // else is a world won - world load code will automatically unlock the first level if needed.
            else if (IsCurrentLevelLastInWorld())
            {
                if (nextWorld != null && nextWorld.UnlockWithCompletion)
                {
                    nextWorld.IsUnlocked = true;
                    nextWorld.UpdatePlayerPrefs();
                }
            }

            // else is at least a level won
            else
            {
                if (nextLevel != null && nextLevel.UnlockWithCompletion)
                {
                    nextLevel.IsUnlocked = true;
                    nextLevel.UpdatePlayerPrefs();
                }
            }

            // save any settings and preferences.
            PreferencesFactory.Save();
        }
    }
}