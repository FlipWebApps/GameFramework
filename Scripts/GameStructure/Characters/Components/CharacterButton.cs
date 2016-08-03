﻿//----------------------------------------------
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

#if UNITY_PURCHASING
using FlipWebApps.GameFramework.Scripts.Billing.Components;
using FlipWebApps.GameFramework.Scripts.Billing.Messages;
#endif
using FlipWebApps.GameFramework.Scripts.GameStructure.Characters.ObjectModel;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Components;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Messaging;
using FlipWebApps.GameFramework.Scripts.Billing.Messages;

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Characters.Components
{
    /// <summary>
    /// Character Details Button
    /// </summary>
    [AddComponentMenu("Game Framework/GameStructure/Characters/CharacterButton")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class CharacterButton : GameItemButton<Character>
    {

        public new void Awake()
        {
            base.Awake();
            GameManager.SafeAddListener<CharacterPurchasedMessage>(CharacterPurchasedHandler);
        }

        protected new void OnDestroy()
        {
            GameManager.SafeRemoveListener<CharacterPurchasedMessage>(CharacterPurchasedHandler);
            base.OnDestroy();
        }

        bool CharacterPurchasedHandler(BaseMessage message)
        {
            var characterPurchasedMessage = message as CharacterPurchasedMessage;
            UnlockIfNumberMatches(characterPurchasedMessage.CharacterNumber);
            return true;
        }

        protected override GameItemsManager<Character, GameItem> GetGameItemsManager()
        {
            return GameManager.Instance.Characters;
        }
    }
}