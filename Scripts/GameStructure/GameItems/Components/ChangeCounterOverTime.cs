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
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Levels;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components
{
    /// <summary>
    /// Sets a counter to either the default or a specified value.
    /// </summary>
    /// Used for initialisation purposes.
    [AddComponentMenu("Game Framework/GameStructure/Common/Change Counter Over Time")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class ChangeCounterOverTime : GameItemContextBase
    {
        /// <summary>
        /// Type of the GameItem that we are referencing
        /// </summary>
        public GameConfiguration.GameItemType GameItemType
        {
            get { return _gameItemType; }
            set { _gameItemType = value; }
        }
        [Tooltip("Type of the GameItem that we are referencing.")]
        [SerializeField]
        GameConfiguration.GameItemType _gameItemType;

        /// <summary>
        /// The counter that we want to use.
        /// </summary>
        public string Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }
        [Tooltip("The counter that we want to use.")]
        [SerializeField]
        string _counter;

        /// <summary>
        /// Whether to only change the counter when the level is running (requries a LevelManager in your scene).
        /// </summary>
        public bool OnlyWhenLevelRunning
        {
            get { return _onlyWhenLevelRunning; }
            set { _onlyWhenLevelRunning = value; }
        }
        [Tooltip("Whether to only change the counter when the level is running (requries a LevelManager in your scene).")]
        [SerializeField]
        bool _onlyWhenLevelRunning = true;

        /// <summary>
        /// The amount that the counter should be set to when not using the default value.
        /// </summary>
        public int IntAmount
        {
            get { return _intAmount; }
            set { _intAmount = value; }
        }
        [Tooltip("The amount that the counter should be changed by per second.")]
        [SerializeField]
        int _intAmount = 1;

        /// <summary>
        /// The amount that the counter should be set to when not using the default value.
        /// </summary>
        public float FloatAmount
        {
            get { return _floatAmount; }
            set { _floatAmount = value; }
        }
        [Tooltip("The amount that the counter should be changed by per second.")]
        [SerializeField]
        float _floatAmount = 1f;

        Counter _counterReference;
        int _oldGameItemNumber;
        float _counterIncrease; // needed for int counters as we won't necessarily update every frame.

        /// <summary>
        /// Awake - if you override then call through to the base class 
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _oldGameItemNumber = GameItem.Number;
            _counterReference = GameItem.GetCounter(Counter);
            Assert.IsNotNull(_counterReference, string.Format("The specified Counter '{0}' was not found. Check that is exists in the game configuration.", Counter));
        }


        /// <summary>
        /// Returns the current GameItemManager as type IBaseGameItemManager
        /// </summary>
        /// <returns></returns>
        protected override IBaseGameItemManager GetIBaseGameItemManager()
        {
            return GameManager.Instance.GetIBaseGameItemManager(GameItemType);
        }


        /// <summary>
        /// Update method to update the counter. 
        /// </summary>
        /// We need to use Update to get accurate timing incase the level is paused.
        public void Update()
        {
            if (OnlyWhenLevelRunning)
                Assert.IsTrue(LevelManager.IsActive, string.Format("Ensure you have a LevelManager added to your scene to use ChangeCounterOverTime (GameObject: {0})", gameObject.name));

            if (!OnlyWhenLevelRunning || LevelManager.Instance.IsLevelRunning)
            {
                // update counter reference if gameitem has changed (e.g. if selected mode).
                if (_oldGameItemNumber != GameItem.Number)
                    _counterReference = GameItem.GetCounter(Counter);

                // int counter we can't update each frame as it is an int, so wait until > 1 before increasing
                if (_counterReference.Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
                {
                    _counterIncrease += (float)IntAmount * Time.deltaTime;
                    if (_counterIncrease > 1)
                    {
                        _counterReference.Increase(1);
                        _counterIncrease = _counterIncrease - 1;
                    }
                    else if (_counterIncrease < -1)
                    {
                        _counterReference.Decrease(1);
                        _counterIncrease = _counterIncrease + 1;
                    }
                }
                else
                {
                    _counterReference.Increase(FloatAmount * Time.deltaTime);
                }
            }
        }
    }
}