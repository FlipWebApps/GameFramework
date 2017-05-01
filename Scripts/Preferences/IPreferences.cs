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


using UnityEngine;

namespace GameFramework.Preferences
{
    /// <summary>
    /// Interface that is used for communicating with third party assets.
    /// </summary>
    public  interface IPreferences
    {
        /// <summary>
        /// Flag indicating whether the current factory implementation supports secure prefs.
        /// </summary>
        bool SupportsSecurePrefs { get; }

        /// <summary>
        /// Flag indicating whether to use secure prefs.
        /// </summary>
        bool UseSecurePrefs { get; set; }

        /// <summary>
        /// Flag indicating whether to migrate unsecure values automatically (only when UseSecurePrefs is set).
        /// </summary>
        bool AutoConvertUnsecurePrefs { get; set; }

        /// <summary>
        /// The pass phrase that should be used. Be sure to override this with your own value.
        /// </summary>
        string PassPhrase { set; }

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        void DeleteKey(string key, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        float GetFloat(string key, float defaultValue = 0.0f, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        int GetInt(string key, int defaultValue = 0, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        string GetString(string key, string defaultValue = "", bool? useSecurePrefs = null);

        /// <summary>
        /// Get boolean preferences
        /// </summary>
        bool GetBool(string key, bool defaultValue = false, bool? useSecurePrefs = null);

        /// <summary>
        /// Get Vector2 preferences
        /// </summary>
        Vector2? GetVector2(string key, Vector2? defaultValue = null, bool? useSecurePrefs = null);

        /// <summary>
        /// Get Vector3 preferences
        /// </summary>
        Vector3? GetVector3(string key, Vector3? defaultValue = null, bool? useSecurePrefs = null);

        /// <summary>
        /// Get Color preferences
        /// </summary>
        Color? GetColor(string key, Color? defaultValue = null, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        bool HasKey(string key, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        void Save();

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        void SetFloat(string key, float value, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        void SetInt(string key, int value, bool? useSecurePrefs = null);

        /// <summary>
        /// For the similar method in PlayerPrefs.
        /// </summary>
        void SetString(string key, string value, bool? useSecurePrefs = null);

        /// <summary>
        /// Set boolean preferences
        /// </summary>
        void SetBool(string key, bool value, bool? useSecurePrefs = null);

        /// <summary>
        /// Set Vector2 preferences
        /// </summary>
        void SetVector2(string key, Vector2 value, bool? useSecurePrefs = null);

        /// <summary>
        /// Set Vector3 preferences
        /// </summary>
        void SetVector3(string key, Vector3 value, bool? useSecurePrefs = null);

        /// <summary>
        /// Set Color preferences
        /// </summary>
        void SetColor(string key, Color value, bool? useSecurePrefs = null);
    }
}

