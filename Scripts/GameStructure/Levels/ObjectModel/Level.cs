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
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using UnityEngine;

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
        /// Constructor
        /// </summary>
        public Level() : base() { }


        /// <summary>
        /// Parse the loaded level file data for level specific values. If overriding from a base class be sure to call base.ParseLevelFileData()
        /// </summary>
        /// <param name="jsonObject"></param>
        public override void ParseLevelFileData(Helper.JSONObject jsonObject)
        {
            base.ParseLevelFileData(jsonObject);

            if (jsonObject.ContainsKey("star1target"))
                Star1Target = (float)jsonObject.GetNumber("star1target");
            if (jsonObject.ContainsKey("star2target"))
                Star2Target = (float)jsonObject.GetNumber("star2target");
            if (jsonObject.ContainsKey("star3target"))
                Star3Target = (float)jsonObject.GetNumber("star3target");
        }
    }
}
