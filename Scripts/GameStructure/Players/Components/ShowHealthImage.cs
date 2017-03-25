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

using GameFramework.Display.Other;
using GameFramework.GameStructure.Players.Messages;
using GameFramework.Messaging.Components.AbstractClasses;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.GameStructure.Players.Components
{
    /// <summary>
    /// Show the health that a player has by updating the fill amount on an image.
    /// </summary>
    /// The referenced image should have it's image type set to filled. This component will also optionally lerps the image color between two values.
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Game Framework/GameStructure/Players/ShowHealthImage")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/players/")]
    public class ShowHealthImage : RunOnMessage<HealthChangedMessage>
    {
        /// <summary>
        /// A tint to apply to the attached image when the user has full health.
        /// </summary>
        [Tooltip("A tint to apply to the attached image when the user has full health.")]
        public Color HealthTintFull = Color.white;

        /// <summary>
        /// A tint to apply to the attached image when the user has no health left.
        /// </summary>
        [Tooltip("A tint to apply to the attached image when the user has no health left.")]
        public Color HealthTintEmpty = Color.white;

        Image _image;
        ColorHSV _fullColor;
        ColorHSV _emptyColor;

        /// <summary>
        /// Get a reference to the attached image and cache for later use.
        /// </summary>
        public override void Awake()
        {
            _image = GetComponent<Image>();
            _fullColor = HealthTintFull.ToHSV();
            _emptyColor = HealthTintEmpty.ToHSV();
        }


        /// <summary>
        /// Cache player reference and call base class.
        /// </summary>
        public override void Start()
        {
            RunMethod(new HealthChangedMessage(GameManager.Instance.Player.Health, GameManager.Instance.Player.Health));
            base.Start();
        }


        /// <summary>
        /// Update the attached imaged fill amount with the new health.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool RunMethod(HealthChangedMessage message)
        {
            _image.fillAmount = message.NewHealth;
            _image.color = ColorHelper.LerpHSV(_emptyColor, _fullColor, message.NewHealth);
            return true;
        }
    }
}