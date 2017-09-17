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
using GameFramework.Preferences;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Runtime information about a single counter entry including the key that identifies it.
    /// </summary>
    /// Counter can contain either int or a float values. The type currently being used is defined by the CounterType prperty.
    /// 
    /// To allow for rollback, counters maintain the notion of the current value (e.g. IntAmount, HighestIntAmount) and last 
    /// saved value (e.g. IntAmountSaved, HighestIntAmountSaved). You will generally work with the current value however there
    /// may be situations where you don't want to keep the current value such as if the player loses a level. The last saved 
    /// values give you an option to roll back.
    public class Counter
    {
        /// <summary>
        /// A reference to the CounterConfigurationEntry that this entry is based upon.
        /// </summary>
        public CounterConfiguration Configuration { get; set; }

        /// <summary>
        /// Identifier - can be used for referencing rather than comparing the Key which is slower.
        /// </summary>
        public int Identifier { get; set; }            

        string _prefsPrefix;
        ICounterChangedCallback _counterChangedCallbacks;
        string _prefsKey, _prefsKeyBest;

        #region Runtime Properties

        /// <summary>
        /// The current amount that this counter is at.
        /// </summary>
        public int IntAmount
        {
            get { return _intAmount; }
            set
            {
                if (_intAmount != value)
                {
                    var oldAmount = _intAmount;
                    if (value < Configuration.IntMinimum)
                        value = Configuration.IntMinimum;
                    else if (value > Configuration.IntMaximum)
                        value = Configuration.IntMaximum;
                    _intAmount = value;
                    if (Configuration.Save == CounterConfiguration.SaveType.Always)
                        IntAmountSaved = _intAmount;
                    if (_counterChangedCallbacks != null)
                        _counterChangedCallbacks.CounterIntAmountChanged(this, oldAmount);
                    if (_intAmount > IntAmountBest)
                        IntAmountBest = _intAmount;
                }
            }
        }
        int _intAmount;

        /// <summary>
        /// The saved version of the int amount. 
        /// </summary>
        /// Can be used for rolling back so e.g. a user doesn't keep their score if they don't win the level.
        /// Can also be used to show / calculate the difference between what they had and have now.
        /// Note that you should call UpdatePlayerPrefs / PlayerPrefs.Save to copy and save this value to player prefs
        public int IntAmountSaved { get; private set; }

        /// <summary>
        /// The highest value that the int variable has reached.
        /// </summary>
        public int IntAmountBest
        {
            get { return _intAmountBest; }
            private set
            {
                if (_intAmountBest != value)
                {
                    var oldAmount = _intAmountBest;
                    _intAmountBest = value;
                    if (Configuration.SaveBest == CounterConfiguration.SaveType.Always)
                        IntAmountBestSaved = _intAmountBest;
                    if (_counterChangedCallbacks != null)
                        _counterChangedCallbacks.CounterIntAmountBestChanged(this, oldAmount);
                }
            }
        }
        int _intAmountBest;

        /// <summary>
        /// The saved version of the highest int amount.
        /// </summary>
        /// Can be used for rolling back so e.g. a user doesn't keep their score if they don't win the level.
        /// Can also be used to show / calculate the difference between what they had and have now.
        /// Note that you should call UpdatePlayerPrefs / PlayerPrefs.Save to copy and save this value to player prefs
        public int IntAmountBestSaved { get; private set; }


        /// <summary>
        /// The current amount that this counter is at.
        /// </summary>
        public float FloatAmount
        {
            get { return _floatAmount; }
            set
            {
                if (!Mathf.Approximately(_floatAmount, value)) {
                    var oldAmount = _floatAmount;
                    if (value < Configuration.FloatMinimum)
                        value = Configuration.FloatMinimum;
                    else if (value > Configuration.FloatMaximum)
                        value = Configuration.FloatMaximum;
                    _floatAmount = value;
                    if (Configuration.Save == CounterConfiguration.SaveType.Always)
                        FloatAmountSaved = _floatAmount;
                    if (_counterChangedCallbacks != null)
                        _counterChangedCallbacks.CounterFloatAmountChanged(this, oldAmount);
                    if (_floatAmount > FloatAmountBest)
                        FloatAmountBest = _floatAmount;
                }
            }
        }
        float _floatAmount;

        /// <summary>
        /// The saved version of the float amount.
        /// </summary>
        /// Can be used for rolling back so e.g. a user doesn't keep their score if they don't win the level.
        /// Can also be used to show / calculate the difference between what they had and have now.
        /// Note that you should call UpdatePlayerPrefs / PlayerPrefs.Save to copy and save this value to player prefs
        public float FloatAmountSaved { get; private set; }

        /// <summary>
        /// The highest value that the float variable has reached.
        /// </summary>
        public float FloatAmountBest
        {
            get { return _floatAmountBest; }
            private set
            {
                if (!Mathf.Approximately(_floatAmountBest, value))
                {
                    var oldAmount = _floatAmountBest;
                    _floatAmountBest = value;
                    if (Configuration.SaveBest == CounterConfiguration.SaveType.Always)
                        FloatAmountBestSaved = _floatAmountBest;
                    if (_counterChangedCallbacks != null)
                        _counterChangedCallbacks.CounterFloatAmountBestChanged(this, oldAmount);
                }
            }
        }
        float _floatAmountBest;


        /// <summary>
        /// The saved version of the highest float amount.
        /// </summary>
        /// Can be used for rolling back so e.g. a user doesn't keep their score if they don't win the level.
        /// Can also be used to show / calculate the difference between what they had and have now.
        /// Note that you should call UpdatePlayerPrefs / PlayerPrefs.Save to copy and save this value to player prefs
        public float FloatAmountBestSaved { get; private set; }

        #endregion Runtime Properties

        #region Initialisation

        /// <summary>
        /// Constructor to hold necessary references.
        /// </summary>
        /// <param name="counterConfigurationEntry"></param>
        /// <param name="prefsPrefix"></param>
        /// <param name="identifier"></param>
        /// <param name="counterChangedCallbacks"></param>
        public Counter(CounterConfiguration counterConfigurationEntry, string prefsPrefix = null,
            int identifier = -1, ICounterChangedCallback counterChangedCallbacks = null)
        {
            Assert.IsTrue(prefsPrefix != null || 
                (counterConfigurationEntry.Save == CounterConfiguration.SaveType.None && counterConfigurationEntry.SaveBest == CounterConfiguration.SaveType.None), 
                "If persisting a counter then ensure to pass a valid prefs prefix to use.");

            Configuration = counterConfigurationEntry;
            Identifier = identifier;
            if (counterConfigurationEntry.CounterType == CounterConfiguration.CounterTypeEnum.Int)
                IntAmount = counterConfigurationEntry.IntDefault;
            else
                FloatAmount = counterConfigurationEntry.FloatDefault;

            _prefsPrefix = prefsPrefix;
            _counterChangedCallbacks = counterChangedCallbacks;

            SetupPrefsKeys();
        }


        /// <summary>
        /// Load this item from perferences within the context of the specified GameItem
        /// </summary>
        public void LoadFromPrefs()
        {
            if (Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
            {
                if (Configuration.Save == CounterConfiguration.SaveType.None)
                    IntAmountSaved = IntAmount = Configuration.IntDefault;
                else
                    IntAmountSaved = IntAmount = PreferencesFactory.GetInt(_prefsKey, Configuration.IntDefault); 

                if (Configuration.SaveBest == CounterConfiguration.SaveType.None)
                    IntAmountBestSaved = IntAmountBest = Configuration.IntDefault;
                else
                    IntAmountBestSaved = IntAmountBest = PreferencesFactory.GetInt(_prefsKeyBest, Configuration.IntDefault);
            }
            else
            {
                if (Configuration.Save == CounterConfiguration.SaveType.None)
                    FloatAmountSaved = FloatAmount = Configuration.FloatDefault;
                else
                    FloatAmountSaved = FloatAmount = PreferencesFactory.GetFloat(_prefsKey, Configuration.FloatDefault);
                

                if (Configuration.SaveBest == CounterConfiguration.SaveType.None)
                    FloatAmountBestSaved = FloatAmountBest = Configuration.FloatDefault;
                else
                    FloatAmountBestSaved = FloatAmountBest = PreferencesFactory.GetFloat(_prefsKeyBest, Configuration.FloatDefault);
            }
        }

        /// <summary>
        /// Set the saved properties to perferences within the context of the specified GameItem.
        /// </summary>
        /// Save / SaveBest mode of 'Always' will cause XxAmount(Best) be copied to the corresponding saved 
        /// variables before saving to prefs, other modes of updating the should be triggered coopied.
        public void UpdatePlayerPrefs()
        {
            if (Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
            {
                if (Configuration.Save != CounterConfiguration.SaveType.None)
                    PreferencesFactory.SetInt(_prefsKey, IntAmountSaved);       // CI = CounterInt)

                if (Configuration.SaveBest != CounterConfiguration.SaveType.None)
                    PreferencesFactory.SetInt(_prefsKeyBest, IntAmountBestSaved);  // CIH = CounterIntHighest)
            }
            else
            {
                if (Configuration.Save != CounterConfiguration.SaveType.None)
                    PreferencesFactory.SetFloat(_prefsKey, FloatAmountSaved);

                if (Configuration.SaveBest != CounterConfiguration.SaveType.None)
                    PreferencesFactory.SetFloat(_prefsKeyBest, FloatAmountBestSaved);
            }
        }


        /// <summary>
        /// For legacy settings, set an override for the default counter prefs key format
        /// </summary>
        /// <returns></returns>
        void SetupPrefsKeys()
        {
            // defaults
            if (Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
            {
                _prefsKey = "CI." + Configuration.Name;
                _prefsKeyBest = "CIH." + Configuration.Name;
            }
            else
            {
                _prefsKey = "CF." + Configuration.Name;
                _prefsKeyBest = "CFH." + Configuration.Name;
            }

            // overrides for legacy properties
            if (_counterChangedCallbacks is Players.ObjectModel.Player)
            {
                if (Configuration.Name.Equals("Score")) {
                    _prefsKey = "TotalScore";
                    _prefsKeyBest = "HS";
                }
                else if (Configuration.Name.Equals("Coins")) {
                    _prefsKey = "TotalCoins";
                }
                if (Configuration.Name.Equals("Lives")) {
                    _prefsKey = "Lives";
                }
                else if (Configuration.Name.Equals("Health")) {
                    _prefsKey = "Health";
                }
            }
            else if (_counterChangedCallbacks is Levels.ObjectModel.Level) {
                if (Configuration.Name.Equals("Score"))
                {
                    _prefsKey = "TotalScore";
                    _prefsKeyBest = "HS";
                }
                if (Configuration.Name.Equals("Progress"))
                {
                    _prefsKeyBest = "ProgressBest";
                }
            }
            else if (_counterChangedCallbacks is GameItem)
            {
                if (Configuration.Name.Equals("Score")) {
                    _prefsKeyBest = "HS";
                }

            }

            // append prefix
            _prefsKey = _prefsPrefix + _prefsKey;
            _prefsKeyBest = _prefsPrefix + _prefsKeyBest;
        }

        /// <summary>
        /// Reset the counter to the default amount
        /// </summary>
        public void Reset()
        {
            if (Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Int)
                IntAmount = Configuration.IntDefault;
            else
                FloatAmount = Configuration.FloatDefault;
        }

        #endregion Initialisation

        #region Get and Set
        /// <summary>
        /// Increase the counter by the specified amount
        /// </summary>
        public void Increase(float amount = 1)
        {
            FloatAmount += amount;
        }

        /// <summary>
        /// Increase the counter by the specified amount
        /// </summary>
        public void Increase(int amount = 1)
        {
            IntAmount += amount;
        }

        /// <summary>
        /// Decrease the counter by the specified amount
        /// </summary>
        public void Decrease(float amount = 1)
        {
            FloatAmount -= amount;
        }

        /// <summary>
        /// Decrease the counter by the specified amount
        /// </summary>
        public void Decrease(int amount = 1)
        {
            IntAmount -= amount;
        }

        /// <summary>
        /// Set the amount of this counter
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void Set(float amount)
        {
            FloatAmount = amount;
        }

        /// <summary>
        /// Set the amount of this counter
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void Set(int amount)
        {
            IntAmount = amount;
        }

        /// <summary>
        /// Get the amount that this counter is currently set to
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            return FloatAmount;
        }


        /// <summary>
        /// Get the amount that this counter is currently set to
        /// </summary>
        /// <returns></returns>
        public float GetInt()
        {
            return IntAmount;
        }


        /// <summary>
        /// Get the normalised amount (0..1) as factor of the minimum and maximum values.
        /// </summary>
        /// <returns></returns>
        public float GetNormalisedAmount()
        {
            if (Configuration.CounterType == CounterConfiguration.CounterTypeEnum.Float)
                return (FloatAmount - Configuration.FloatMinimum) / (Configuration.FloatMaximum - Configuration.FloatMinimum);
            else
                return ((float)(IntAmount - Configuration.IntMinimum)) / ((float)(Configuration.IntMaximum - Configuration.IntMinimum));
        }


        /// <summary>
        /// Get the amount as a percent of the minimum and maximum values.
        /// </summary>
        /// <returns></returns>
        public float GetAsPercent()
        {
            return GetNormalisedAmount() * 100;
        }


        /// <summary>
        /// Push values to the XxxSaved properties if saved mode is not set to none. 
        /// Note you should still call UpdatePrefs / Prefs.Save to persist these values.
        /// </summary>
        public void PushToSaved()
        {
            if (Configuration.Save != CounterConfiguration.SaveType.None)
            {
                IntAmountSaved = IntAmount;
                FloatAmountSaved = FloatAmount;
            }
            if (Configuration.SaveBest != CounterConfiguration.SaveType.None)
            {
                IntAmountBestSaved = IntAmountBest;
                FloatAmountBestSaved = FloatAmountBest;
            }
        }
        #endregion Get and Set
    }

    public interface ICounterChangedCallback
    {
        void CounterIntAmountChanged(Counter counter, int oldAmount);
        void CounterIntAmountBestChanged(Counter counter, int oldAmount);
        void CounterFloatAmountChanged(Counter counter, float oldAmount);
        void CounterFloatAmountBestChanged(Counter counter, float oldAmount);
    }
}