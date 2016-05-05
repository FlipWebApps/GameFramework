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
using FlipWebApps.GameFramework.Scripts.GameStructure.Levels.Messages;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Levels.ObjectModel
{
    /// <summary>
    /// Level Game Item
    /// </summary>
    public class Level : GameItem
    {
        public override string IdentifierBase { get { return "Level"; }}
        public override string IdentifierBasePrefs { get { return "L"; } }

        /// <summary>
        /// The total number of stars that can be gotten. Automatically loaded from JSON configuration if present.
        /// </summary>
        public float StarTotalCount { get; set; }

        /// <summary>
        /// The number of stars that have been won for this level. Represented as a bitmask.
        /// StarsWonMessage is sent whenever this value changes.
        /// </summary>
        public int StarsWon { get { return _starsWon; }
            set
            {
                var oldStarsWon = _starsWon;
                _starsWon = value;
                if (IsInitialised && oldStarsWon != StarsWon)
                    GameManager.SafeQueueMessage(new StarsWonMessage(this, StarsWon, oldStarsWon));
            }
        }
        int _starsWon;

        /// <summary>
        /// A value that can be used for holding a target the the first star. Automatically loaded from JSON configuration if present.
        /// </summary>
        public float Star1Target { get; set; }

        /// <summary>
        /// A value that can be used for holding a target the the second star. Automatically loaded from JSON configuration if present.
        /// </summary>
        public float Star2Target { get; set; }

        /// <summary>
        /// A value that can be used for holding a target the the third star. Automatically loaded from JSON configuration if present.
        /// </summary>
        public float Star3Target { get; set; }

        /// <summary>
        /// A value that can be used for holding a target the the third star. Automatically loaded from JSON configuration if present.
        /// </summary>
        public float Star4Target { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public Level()
        {
            StarTotalCount = 3;
        }

        /// <summary>
        /// Provides a simple method that you can overload to do custom initialisation in your own classes.
        /// This is called after ParseLevelFileData (if loading from resources) so you can use values setup by that method. 
        /// 
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        /// </summary>
        public override void CustomInitialisation()
        {
            StarsWon = GetSettingInt("SW", 0);
        }

        /// <summary>
        /// Returns whether the specified star has been obtained for this level.
        /// </summary>
        /// <param name="starNumber"></param>
        /// <returns></returns>
        public bool IsStarWon(int starNumber)
        {
            return (StarsWon & (1 << (starNumber - 1))) != 0;
        }


        /// <summary>
        /// Set whether a specified star has been won.
        /// </summary>
        /// <param name="starNumber"></param>
        /// <param name="isWon"></param>
        /// <returns></returns>
        public void StarWon(int starNumber, bool isWon)
        {
            if (isWon)
                StarsWon |= (1 << (starNumber - 1));
            else
                StarsWon &= (~(1 << (starNumber - 1)));
        }


        /// <summary>
        /// Parse the loaded level file data for level specific values. If overriding from a base class be sure to call base.ParseLevelFileData()
        /// </summary>
        /// <param name="jsonObject"></param>
        public override void ParseLevelFileData(Helper.JSONObject jsonObject)
        {
            base.ParseLevelFileData(jsonObject);

            if (jsonObject.ContainsKey("startotalcount"))
                StarTotalCount = (float)jsonObject.GetNumber("startotalcount");
            if (jsonObject.ContainsKey("star1target"))
                Star1Target = (float)jsonObject.GetNumber("star1target");
            if (jsonObject.ContainsKey("star2target"))
                Star2Target = (float)jsonObject.GetNumber("star2target");
            if (jsonObject.ContainsKey("star3target"))
                Star3Target = (float)jsonObject.GetNumber("star3target");
            if (jsonObject.ContainsKey("star4target"))
                Star4Target = (float)jsonObject.GetNumber("star4target");
        }


        /// <summary>
        /// Update PlayerPrefs with setting or preferences for this item.
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        public override void UpdatePlayerPrefs()
        {
            base.UpdatePlayerPrefs();
            SetSetting("SW", StarsWon);
        }


        #region Score and Coin Messaging Overrides
        /// <summary>
        /// Sends a PlayerScoreChangedMessage whenever the players score changes.
        /// </summary>
        /// <param name="newScore"></param>
        /// <param name="oldScore"></param>
        public override void SendScoreChangedMessage(int newScore, int oldScore)
        {
            GameManager.Messenger.QueueMessage(new LevelScoreChangedMessage(this, newScore, oldScore));
        }


        /// <summary>
        /// Sends a PlayerHighScoreChangedMessage whenever the players high score changes.
        /// </summary>
        /// <param name="newHighScore"></param>
        /// <param name="oldHighScore"></param>
        public override void SendHighScoreChangedMessage(int newHighScore, int oldHighScore)
        {
            GameManager.Messenger.QueueMessage(new LevelHighScoreChangedMessage(this, newHighScore, oldHighScore));
        }


        /// <summary>
        /// Sends a PlayerCoinsChangedMessage whenever the players coin count changes.
        /// </summary>
        /// <param name="newCoins"></param>
        /// <param name="oldCoins"></param>
        public override void SendCoinsChangedMessage(int newCoins, int oldCoins)
        {
            GameManager.Messenger.QueueMessage(new LevelCoinsChangedMessage(this, newCoins, oldCoins));
        }

        #endregion Score and Coin Messaging Overrides

    }
}
