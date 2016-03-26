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

using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel
{
    /// <summary>
    /// Player Game Item
    /// </summary>

    public class Player : GameItem
    {
        public override string IdentifierBase { get { return "Player"; } }
        public override string IdentifierBasePrefs { get { return "P"; } }

        /// <summary>
        /// The number of lives that the current player as.
        /// </summary>
        public int Lives { get; set; }

        public int MaximumWorld;
        public int MaximumLevel;
        public int SelectedWorld;
        public int SelectedLevel;   // only use when not using worlds, other use World.SelectedLevel for world specific level.

        public Player() { }

        /// <summary>
        /// Provides a simple method that you can overload to do custom initialisation in your own classes.
        /// This is called after ParseLevelFileData (if loading from resources) so you can use values setup by that method. 
        /// 
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        /// </summary>
        public override void CustomInitialisation()
        {
            Reset();

            Name = GetSettingString("Name", Name);

            Score = GetSettingInt("TotalScore", 0);
            Coins = GetSettingInt("TotalCoins", 0);

            if (GameManager.IsActive)
                Lives = GameManager.Instance.DefaultLives;

            MaximumWorld = GetSettingInt("MaximumWorld", MaximumWorld);
            MaximumLevel = GetSettingInt("MaximumLevel", MaximumLevel);
            SelectedWorld = GetSettingInt("SelectedWorld", SelectedWorld);
            SelectedLevel = GetSettingInt("SelectedLevel", SelectedLevel);
        }


        public virtual void Reset()
        {
            MaximumWorld = 0;
            MaximumLevel = 0;
            SelectedWorld = 0;
            SelectedLevel = 0;

            Score = 0;
            Coins = 0;
        }


        /// <summary>
        /// Update PlayerPrefs with setting or preferences for this item.
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        public override void UpdatePlayerPrefs()
        {
            SetSetting("Name", Name);

            SetSetting("TotalScore", Score);
            SetSetting("TotalCoins", Coins);

            SetSetting("MaximumWorld", MaximumWorld);
            SetSetting("MaxLevel", MaximumLevel);
            SetSetting("SelectedWorld", SelectedWorld);
            SetSetting("SelectedLevel", SelectedLevel);

            base.UpdatePlayerPrefs();
        }
    }
}