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

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using GameFramework.Debugging;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.ObjectModel
{
    /// <summary>
    /// Allows for setting of global game configuration. This is seperate from GameManager and a Scriptable Object so that it
    /// can be used from unit testing and loaded by editor components etc.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game Framework/Game Configuration", order = 30)]
    [System.Serializable]
    public class GameConfiguration : ScriptableObject
    {
        #region Static Singleton Reference
        static GameConfiguration _instance;
        public static GameConfiguration Instance
        {
            get
            {
                if (_instance == null)
                    _instance = LoadSingletonGameConfiguration();
                if (_instance == null)
                    _instance = CreateInstance<GameConfiguration>();
                return _instance;
            }
        }

        /// <summary>
        /// To allow for reloading - e.g. so the static instance gets updated if we make changes in the editor
        /// </summary>
        public static GameConfiguration LoadSingletonGameConfiguration()
        {
            return GameManager.LoadResource<GameConfiguration>("GameConfiguration");
        }
        #endregion Static Singleton Reference

        #region Scores
        /// <summary>
        /// List of score configurations. You can read from this, but should not manipulate this - use the other methods.
        /// </summary>
        public List<ScoreConfigurationEntry> ScoreConfigurationEntries
        {
            get
            {
                return _scoreConfigurationEntry;
            }
        }
        [SerializeField]
        List<ScoreConfigurationEntry> _scoreConfigurationEntry = new List<ScoreConfigurationEntry>();
        #endregion Scores
    }


    /// <summary>
    /// Holds information about a single score entry including the key that identifies it.
    /// </summary>
    [System.Serializable]
    public class ScoreConfigurationEntry
    {
        #region Configuration Properties
        /// <summary>
        /// A unique key that identifies this entry.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }
        [SerializeField]
        string _key;


        // Persist
        // Persist Key
        #endregion Configuration Properties

        #region Runtime Properties
        // Value (float / int)
        #endregion Runtime Properties

    }
}