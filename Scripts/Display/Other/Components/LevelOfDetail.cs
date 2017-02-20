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

using System.Collections;
using UnityEngine;

namespace GameFramework.Display.Other.Components
{
    /// <summary>
    /// Simple level of detail component that enables different gameobjects based upon the distance from the camera to help improve performance.
    /// </summary>
    /// You can use this script to improve the performance of your games by replacing distant objects with 
    /// lower detail models. Select the gameobjects to use for the different distance thresholds. They will 
    /// be enabled and disabled according to the calculated distance.
    [AddComponentMenu("Game Framework/Display/Other/Level Of Detail")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class LevelOfDetail : MonoBehaviour
    {
        /// <summary>
        /// The level of detail entries. This must be sorted by ascending cutoff distance
        /// </summary>
        [Tooltip("The level of detail entries. This must be sorted by ascending cutoff distance")]
        public LevelOfDetailEntry[] LevelOfDetailEntries;

        /// <summary>
        /// The origin object used to determine the distance. Typically the scene camera or player
        /// </summary>
        [Tooltip("The origin object used to determine the distance. Typically the scene camera or player")]
        public GameObject DistanceFrom;

        /// <summary>
        /// Whether to disable all items when distance is greater than the maximum supplied
        /// </summary>
        [Tooltip("Whether to disable all items when distance is greater than the maximum supplied")]
        public bool DisableOutOfBounds;

        /// <summary>
        /// How often to perform the distance check
        /// </summary>
        [Tooltip("How often to perform the distance check")]
        public float CheckInterval = 1f;

        void Start()
        {
            StartCoroutine(DistanceCheck());
        }

        IEnumerator DistanceCheck()
        {
            while (true)
            {
                // get distance and enable corresponding LOD
                var distance = Vector3.Distance(DistanceFrom.transform.position, transform.position);
                if (EnableLevelOfDetailItem(distance) == false && DisableOutOfBounds)
                {
                    // out of bounds.
                    foreach(var levelOfDetail in LevelOfDetailEntries)
                    {
                        levelOfDetail.Target.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(CheckInterval);
            }

        }

        private bool EnableLevelOfDetailItem(float distance)
        {
            for (int i = 0; i < LevelOfDetailEntries.Length; i++)
            {
                if (distance < LevelOfDetailEntries[i].CutOffDistance)
                {
                    // Match so disable all but this
                    for (int j = 0; j < LevelOfDetailEntries.Length; j++)
                    {
                        LevelOfDetailEntries[j].Target.SetActive(j == i);
                    }
                    return true;
                }
            }
            return false; // not found
        }

        [System.Serializable]
        public class LevelOfDetailEntry {
            /// <summary>
            /// The target to enable for this level of detail
            /// </summary>
            [Tooltip("The target to enable for this level of detail")]
            public GameObject Target;

            /// <summary>
            /// The cut off distance this level of detail
            /// </summary>
            [Tooltip("The cut off distance this level of detail")]
            public float CutOffDistance;
        }
    }
}