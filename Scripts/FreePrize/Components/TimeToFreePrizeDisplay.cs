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
using GameFramework.GameObjects.Components.AbstractClasses;
using GameFramework.Localisation;
using UnityEngine;
using GameFramework.Messaging;
using GameFramework.GameStructure;
using GameFramework.Localisation.Messages;

namespace GameFramework.FreePrize.Components
{
    /// <summary>
    /// Shows the amount of time until the free prize is available
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    [AddComponentMenu("Game Framework/FreePrize/TimeToFreePrizeDisplay")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class TimeToFreePrizeDisplay : RunOnState
    {
        UnityEngine.UI.Text _text;

        public new void Awake()
        {
            base.Awake();

            _text = GetComponent<UnityEngine.UI.Text>();

            GameManager.SafeAddListener<LocalisationChangedMessage>(LocalisationChangedHandler);
        }

        void OnDestroy()
        {
            GameManager.SafeRemoveListener<LocalisationChangedMessage>(LocalisationChangedHandler);
        }

        bool LocalisationChangedHandler(BaseMessage message)
        {
            RunMethod();
            return true;
        }

        public override void RunMethod()
        {
            TimeSpan time = FreePrizeManager.Instance.GetTimeToPrize();
            _text.text = GlobalLocalisation.FormatText("FreePrize.NewPrize", time.Hours, time.Minutes, time.Seconds);
 }
    }
}