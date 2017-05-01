//----------------------------------------------
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

#if PREFS_EDITOR
using PrefsEditor;
using UnityEngine;

namespace GameFramework.Preferences.PrefsEditorIntegration
{
    /// <summary>
    /// Implementation of IPreferences interface for the Prefs Editor asset.
    /// </summary>
    public class PrefsEditorHandler : IPreferences
    {
        /// <summary>
        /// Indicate that this implementaiton supports secure prefs.
        /// </summary>
        public bool SupportsSecurePrefs {
            get { return true; }
        }

        /// <summary>
        /// Flag indicating whether to use secure prefs.
        /// 
        /// Note: at the current time all prefs used through this interface must adhere to this flag. The only way to mix 
        /// secure and standard prefs is to mix calls with standard PlayerPrefs calls.
        /// </summary>
        public bool UseSecurePrefs {
            get { return SecurePlayerPrefs.UseSecurePrefs; }
            set { SecurePlayerPrefs.UseSecurePrefs = value; }
        }

        /// <summary>
        /// Flag indicating whether to migrate unsecure values automatically (only when UseSecurePrefs is set).
        /// 
        /// Note: at the current time all prefs used through this interface must adhere to this flag. The only way to mix 
        /// secure and standard prefs is to mix calls with standard PlayerPrefs calls.
        /// </summary>
        public bool AutoConvertUnsecurePrefs
        {
            get { return SecurePlayerPrefs.AutoConvertUnsecurePrefs; }
            set { SecurePlayerPrefs.AutoConvertUnsecurePrefs = value; }
        }

        /// <summary>
        /// The pass phrase that should be used. Be sure to override this with your own value.
        /// 
        /// Note: This property has no effect for standard Player Prefs.
        /// </summary>
        public string PassPhrase
        {
            set { SecurePlayerPrefs.PassPhrase = value; }
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public void DeleteAll()
        {
            SecurePlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public void DeleteKey(string key, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.DeleteKey(key, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public float GetFloat(string key, float defaultValue = 0.0f, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetFloat(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public int GetInt(string key, int defaultValue = 0, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetInt(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public string GetString(string key, string defaultValue = "", bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetString(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Get boolean preferences
        /// </summary>
        public bool GetBool(string key, bool defaultValue = false, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetBool(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Get Vector2 preferences
        /// </summary>
        public Vector2? GetVector2(string key, Vector2? defaultValue = null, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetVector2(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Get Vector3 preferences
        /// </summary>
        public Vector3? GetVector3(string key, Vector3? defaultValue = null, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetVector3(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Get Color preferences
        /// </summary>
        public Color? GetColor(string key, Color? defaultValue = null, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.GetColor(key, defaultValue, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public bool HasKey(string key, bool? useSecurePrefs = null)
        {
            return SecurePlayerPrefs.HasKey(key, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public void Save()
        {
            SecurePlayerPrefs.Save();
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public void SetFloat(string key, float value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetFloat(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public void SetInt(string key, int value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetInt(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Wrapper for the same method in PlayerPrefs but works with encrypted player prefs.
        /// </summary>
        public void SetString(string key, string value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetString(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Set boolean preferences
        /// </summary>
        public void SetBool(string key, bool value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetBool(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Set Vector2 preferences
        /// </summary>
        public void SetVector2(string key, Vector2 value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetVector2(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Set Vector3 preferences
        /// </summary>
        public void SetVector3(string key, Vector3 value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetVector3(key, value, useSecurePrefs);
        }

        /// <summary>
        /// Set Color preferences
        /// </summary>
        public void SetColor(string key, Color value, bool? useSecurePrefs = null)
        {
            SecurePlayerPrefs.SetColor(key, value, useSecurePrefs);
        }
    }
}

#endif
