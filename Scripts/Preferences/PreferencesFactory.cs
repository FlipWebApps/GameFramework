﻿//----------------------------------------------
// Flip Web Apps: Prefs Editor
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

using FlipWebApps.GameFramework.Scripts.Preferences.PlayerPrefsIntegration;
#if PREFS_EDITOR
using FlipWebApps.GameFramework.Scripts.Preferences.PrefsEditorIntegration;
#endif

/// <summary>
/// Extentions to PlayerPrefs providing encrypted preferences, new data types and integration with third party assets./// 
/// </summary>
/// For further information please see http://www.flipwebapps.com/unity-assets/game-framework/preferences/
namespace FlipWebApps.GameFramework.Scripts.Preferences
{
    /// <summary>
    /// Extends PlayerPrefs to provide encrypted preferences, new data types and integration with third party assets.
    /// </summary>
    /// By default this will use PlayerPrefs and so encryption support will not be available. You can enable the different
    /// third party assets through the Game Framework integration window.
    public static class PreferencesFactory
    {
        /// <summary>
        /// Get the current IPreferences instance, setting it up if not already set.
        /// </summary>
        static IPreferences Instance
        {
            get {
#if PREFS_EDITOR
                if (_instance == null)
                    _instance = new PrefsEditorHandler();
#else
                if (_instance == null)
                    _instance = new PlayerPrefsHandler();
#endif
                return _instance;
            }
            set { _instance = value; }
        }
        static IPreferences _instance;

        /// <summary>
        /// Flag indicating whether the current implementation supports secure prefs.
        /// </summary>
        public static bool SupportsSecurePrefs
        {
            get
            {
                return Instance.SupportsSecurePrefs;
            }
        }

        /// <summary>
        /// Flag indicating whether to use secure prefs.
        /// 
        /// Note: All prefs used through this interface must adhere to this flag unless overwritten by the individual Get/Set calls.
        /// </summary>
        public static bool UseSecurePrefs {
            get
            {
                return Instance.UseSecurePrefs;
            }
            set
            {
                Instance.UseSecurePrefs = value;
            }
        }

        /// <summary>
        /// Flag indicating whether to migrate unsecure values automatically (only when UseSecurePrefs is set).
        /// </summary>
        public static bool AutoConvertUnsecurePrefs
        {
            get
            {
                return Instance.AutoConvertUnsecurePrefs;
            }
            set
            {
                Instance.AutoConvertUnsecurePrefs = value;
            }
        }

        /// <summary>
        /// The pass phrase that should be used. Be sure to override this with your own value.
        /// </summary>
        public static string PassPhrase
        {
            set
            {
                Instance.PassPhrase = value;
            }
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        public static void DeleteAll()
        {
            Instance.DeleteAll();
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        public static void DeleteKey(string key)
        {
            Instance.DeleteKey(key);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        /// This method adds the optional useSecurePrefs parameter which will override the global encryption setting
        /// (assuming that the backend implementation supports this functionality).
        public static float GetFloat(string key, float defaultValue = 0.0f, bool? useSecurePrefs = null)
        {
            return Instance.GetFloat(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        /// This method adds the optional useSecurePrefs parameter which will override the global encryption setting
        /// (assuming that the backend implementation supports this functionality).
        public static int GetInt(string key, int defaultValue = 0, bool? useSecurePrefs = null)
        {
            return Instance.GetInt(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        /// This method adds the optional useSecurePrefs parameter which will override the global encryption setting
        /// (assuming that the backend implementation supports this functionality).
        public static string GetString(string key, string defaultValue = "", bool? useSecurePrefs = null)
        {
            return Instance.GetString(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        public static bool HasKey(string key)
        {
            return Instance.HasKey(key);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        public static void Save()
        {
            Instance.Save();
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        /// This method adds the optional useSecurePrefs parameter which will override the global encryption setting
        /// (assuming that the backend implementation supports this functionality).
        public static void SetFloat(string key, float value, bool? useSecurePrefs = null)
        {
            Instance.SetFloat(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        /// This method adds the optional useSecurePrefs parameter which will override the global encryption setting
        /// (assuming that the backend implementation supports this functionality).
        public static void SetInt(string key, int value, bool? useSecurePrefs = null)
        {
            Instance.SetInt(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Factory method for the similar method in PlayerPrefs.
        /// </summary>
        /// This method adds the optional useSecurePrefs parameter which will override the global encryption setting
        /// (assuming that the backend implementation supports this functionality).
        public static void SetString(string key, string value, bool? useSecurePrefs = null)
        {
            Instance.SetString(key, value, useSecurePrefs);
        }
    }
}

