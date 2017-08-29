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
using GameFramework.GameStructure.GameItems.Messages;
using GameFramework.Messaging;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.GameStructure.Game
{
    /// <summary>
    /// Helper class for GameCondition's
    /// </summary>
    public class GameConditionHelper
    {
        /// <summary>
        /// Initialise gameConditions
        /// </summary>
        /// <returns></returns>
        public static void InitialiseGameConditions(IEnumerable<GameConditionReference> gameConditionReferences)
        {
            foreach (var gameConditionReference in gameConditionReferences)
            {
                if (gameConditionReference.IsReference)
                {
                    if (gameConditionReference != null)
                        gameConditionReference.ScriptableObjectReference.InitialiseCommon();
                }
                else
                {
                    var gameCondition = gameConditionReference.ScriptableObject;
                    gameCondition.InitialiseCommon();
                }
            }
        }

        /// <summary>
        /// Add GameCondition listeners.
        /// </summary>
        /// <returns></returns>
        public static void AddListeners(IEnumerable<GameConditionReference> gameConditionReferences, Messenger.MessageListenerDelegate handler)
        {
            System.Type[] listeningMessageTypes = GetListeningMessages(gameConditionReferences);
            foreach (var type in listeningMessageTypes)
                GameManager.SafeAddListener(type, handler);

        }

        /// <summary>
        /// Remove GameCondition listeners.
        /// </summary>
        /// <returns></returns>
        public static void RemoveListeners(IEnumerable<GameConditionReference> gameConditionReferences, Messenger.MessageListenerDelegate handler)
        {
            System.Type[] listeningMessageTypes = GetListeningMessages(gameConditionReferences);
            foreach (var type in listeningMessageTypes)
                GameManager.SafeRemoveListener(type, handler);
        }


        /// <summary>
        /// Get messages to listen to from a list of GameConditionReference's.
        /// </summary>
        /// <returns></returns>
        static System.Type[] GetListeningMessages(IEnumerable<GameConditionReference> gameConditionReferences)
        {
            //TODO: Get types from conditions, don't add duplicates
            // Until properly implemented
            return new[] { typeof(UpdateMessage) };
        }

        /// <summary>
        /// Tests whether all of a list of GameCondition's are met
        /// </summary>
        /// <returns></returns>
        public static bool AllConditionsMet(IEnumerable<GameConditionReference> gameConditionReferences, MonoBehaviour monoBehaviour)
        {
            var conditionsAreAllTrue = true;
            foreach (var gameConditionReference in gameConditionReferences)
            {
                if (gameConditionReference.IsReference)
                {
                    if (gameConditionReference != null)
                        gameConditionReference.ScriptableObjectReference.EvaluateConditionCommon(monoBehaviour);
                }
                else
                {
                    var gameCondition = gameConditionReference.ScriptableObject;
                   // switch (gameConditionReference.Identifier)
                   // {
                        //case ConditionTypes.CanUnlockWithCoins:
                        //    conditionsAreAllTrue &= CanUnlockWithCoins.EvaluateCondition(GameItem,
                        //        conditionReference.BoolValue);
                        //    break;
                        //case ConditionTypes.CanUnlockWithCompletion:
                        //    conditionsAreAllTrue &= CanUnlockWithCompletion.EvaluateCondition(GameItem,
                        //        conditionReference.BoolValue);
                        //    break;
                        //case ConditionTypes.CanUnlockWithPayment:
                        //    conditionsAreAllTrue &= CanUnlockWithPayment.EvaluateCondition(GameItem,
                        //        conditionReference.BoolValue);
                        //    break;
                        //case ConditionTypes.Coins:
                        //    conditionsAreAllTrue &= Coins.EvaluateCondition(GameItem,
                        //        conditionReference.Comparison, conditionReference.IntValue);
                        //    break;
                        //case ConditionTypes.PlayerHasCoinsToUnlock:
                        //    conditionsAreAllTrue &= PlayerHasCoinsToUnlock.EvaluateCondition(GameItem,
                        //        conditionReference.BoolValue);
                        //    break;
                        //case ConditionTypes.Score:
                        //    conditionsAreAllTrue &= Score.EvaluateCondition(GameItem,
                        //        conditionReference.Comparison, conditionReference.IntValue);
                        //    break;
                        //case ConditionTypes.Selected:
                        //    conditionsAreAllTrue &= Selected.EvaluateCondition(GameItem,
                        //        GetGameItemManager().Selected, conditionReference.BoolValue);
                        //    break;
                        //case ConditionTypes.Unlocked:
                        //    conditionsAreAllTrue &= Unlocked.EvaluateCondition(GameItem,
                        //        conditionReference.BoolValue);
                        //    break;
                        //case ConditionTypes.Counter:
                        //    conditionsAreAllTrue &= conditionReference.ScriptableObject.EvaluateCondition(GameItem);
                        //    break;
                        //case ConditionTypes.Custom:
                            //if (conditionReference.ScriptableObjectReference != null)
                            //    conditionsAreAllTrue &= conditionReference.ScriptableObjectReference.EvaluateCondition(GameItem);
                        //    break;
                   // }
                }
            }
            return conditionsAreAllTrue;
        }
    }
}
