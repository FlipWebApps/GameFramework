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

using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels.Messages;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.Levels.ObjectModel
{
    /// <summary>
    /// Level Game Item
    /// </summary>
    [CreateAssetMenu(fileName = "Level_x", menuName = "Game Framework/Level")]
    public class Level : GameItem
    {
        #region Editor Parameters

        /// <summary>
        /// The total number of stars that can be gotten.
        /// </summary>
        public int StarsTotalCount
        {
            get
            {
                return _starTotalCount;
            }
            set
            {
                _starTotalCount = value;
            }
        }
        [Tooltip("An override for the default star total count.")]
        [SerializeField]
        int _starTotalCount = 3;


        /// <summary>
        /// The target for getting the first star or -1 if no target
        /// </summary>
        public float Star1Target
        {
            get
            {
                return _star1Target;
            }
            set
            {
                _star1Target = value;
            }
        }
        [Tooltip("The target for getting 1 star or -1 if no target")]
        [SerializeField]
        float _star1Target = 10;


        /// <summary>
        /// A target for getting the second star or -1 if no target.
        /// </summary>
        public float Star2Target
        {
            get
            {
                return _star2Target;
            }
            set
            {
                _star2Target = value;
            }
        }
        [Tooltip("The target for getting the second star or -1 if no target")]
        [SerializeField]
        float _star2Target = 15;


        /// <summary>
        /// A target for getting the third star or -1 if no target.
        /// </summary>
        public float Star3Target
        {
            get
            {
                return _star3Target;
            }
            set
            {
                _star3Target = value;
            }
        }
        [Tooltip("The target for getting the third star or -1 if no target")]
        [SerializeField]
        float _star3Target = 20;


        /// <summary>
        /// A target for getting the fourth star or -1 if no target.
        /// </summary>
        public float Star4Target
        {
            get
            {
                return _star4Target;
            }
            set
            {
                _star4Target = value;
            }
        }
        [Tooltip("The target for getting the fourth star or -1 if no target")]
        [SerializeField]
        float _star4Target = 25;


        /// <summary>
        /// The time target for completing the level
        /// </summary>
        /// You can also use StarxTarget if you want individual times for winning different stars.
        public float TimeTarget
        {
            get
            {
                return _timeTarget;
            }
            set
            {
                _timeTarget = value;
            }
        }
        [Tooltip("The time target for completing the level.")]
        [SerializeField]
        float _timeTarget;

        /// <summary>
        /// A field that you can set from json, extensions or code that represents a target score for completing the level
        /// </summary>
        /// You can also use StarxTarget if you want individual scores for winning different stars.
        public int ScoreTarget
        {
            get
            {
                return _scoreTarget;
            }
            set
            {
                _scoreTarget = value;
            }
        }
        [Tooltip("The score target for completing the level.")]
        [SerializeField]
        int _scoreTarget;

        /// <summary>
        /// A field that you can set from json, extensions or code that represents a target coins for completing the level
        /// </summary>
        /// You can also use StarxTarget if you want individual scores for winning different stars.
        public int CoinTarget
        {
            get
            {
                return _coinTarget;
            }
            set
            {
                _coinTarget = value;
            }
        }
        [Tooltip("The coins target for completing the level.")]
        [SerializeField]
        int _coinTarget;
        #endregion Editor Parameters

        /// <summary>
        /// A unique identifier for this type of GameItem
        /// </summary>
        public override string IdentifierBase { get { return "Level"; }}

        /// <summary>
        /// A unique shortened version of IdentifierBase to save memory.
        /// </summary>
        public override string IdentifierBasePrefs { get { return "L"; } }

        /// <summary>
        /// Override in subclasses to return a list of custom counter configuration entries that should also
        /// be added to this GameItem.
        /// </summary>
        /// <returns></returns>
        public override List<CounterConfiguration> GetCounterConfiguration()
        {
            return GameConfiguration.LevelCounterConfiguration;
        }

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
                    Messenger.QueueMessage(new StarsWonMessage(this, StarsWon, oldStarsWon));
            }
        }
        int _starsWon;

        /// <summary>
        /// The total number of stars that have been won for this level (max is StarTotalCount). 
        /// </summary>
        public int StarsWonCount
        {
            get
            {
                var starsWon = 0;
                for (var i = 1; i < StarsTotalCount + 1; i++)
                {
                    if (IsStarWon(i))
                        starsWon++;
                }
                return starsWon;
            }
        }

        /// <summary>
        /// A field that you can optionally use for recording the progress. Typically this should be in the range 0..1
        /// </summary>
        public float Progress
        {
            get { return _progressCounter.FloatAmount; }
            set { _progressCounter.FloatAmount = value; }
        }
        Counter _progressCounter;

        /// <summary>
        /// A field that you can optionally use for recording the highest progress obtained. Typically this should be in the range 0..1
        /// </summary>
        public float ProgressBest
        {
            get { return _progressCounter.FloatAmountBest; }
        }

        /// <summary>
        /// A field that you can optionally use for recording the best time that the player has achieved.
        /// </summary>
        public float TimeBest { get; set; }


        /// <summary>
        /// Provides a simple method that you can overload to do custom initialisation in your own classes.
        /// This is called after ParseLevelFileData (if loading from resources) so you can use values setup by that method. 
        /// 
        /// If overriding from a base class be sure to call base.CustomInitialisation()
        /// </summary>
        public override void CustomInitialisation()
        {
            //_timeCounter = GetCounter("Time");
            _progressCounter = GetCounter("Progress");
            //Assert.IsNotNull(_timeCounter, "All GameItems must have a counter defined with the Key 'Time'");
            Assert.IsNotNull(_progressCounter, "All GamItems must have a counter defined with the Key 'Progress'");

            StarsWon = GetSettingInt("SW", 0);
            TimeBest = GetSettingFloat("TimeBest", 0);
            //ProgressBest = GetSettingFloat("ProgressBest", 0);
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
        /// Update PlayerPrefs with setting or preferences for this item.
        /// Note: This does not call PlayerPrefs.Save()
        /// 
        /// If overriding from a base class be sure to call base.ParseGameData()
        /// </summary>
        public override void UpdatePlayerPrefs()
        {
            base.UpdatePlayerPrefs();
            SetSetting("SW", StarsWon);
            SetSettingFloat("TimeBest", TimeBest);
            //SetSettingFloat("ProgressBest", ProgressBest);
        }


        #region Score and Coin Messaging Overrides
        /// <summary>
        /// Sends a PlayerScoreChangedMessage whenever the players score changes.
        /// </summary>
        /// <param name="newScore"></param>
        /// <param name="oldScore"></param>
        public override void SendScoreChangedMessage(int newScore, int oldScore)
        {
            Messenger.QueueMessage(new LevelScoreChangedMessage(this, newScore, oldScore));
        }


        /// <summary>
        /// Sends a PlayerHighScoreChangedMessage whenever the players high score changes.
        /// </summary>
        /// <param name="newHighScore"></param>
        /// <param name="oldHighScore"></param>
        public override void SendHighScoreChangedMessage(int newHighScore, int oldHighScore)
        {
            Messenger.QueueMessage(new LevelHighScoreChangedMessage(this, newHighScore, oldHighScore));
        }


        /// <summary>
        /// Sends a PlayerCoinsChangedMessage whenever the players coin count changes.
        /// </summary>
        /// <param name="newCoins"></param>
        /// <param name="oldCoins"></param>
        public override void SendCoinsChangedMessage(int newCoins, int oldCoins)
        {
            Messenger.QueueMessage(new LevelCoinsChangedMessage(this, newCoins, oldCoins));
        }

        #endregion Score and Coin Messaging Overrides

    }
}
