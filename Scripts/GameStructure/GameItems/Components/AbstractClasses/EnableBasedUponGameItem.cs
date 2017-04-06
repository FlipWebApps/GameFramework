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

using System.Collections.Generic;
using GameFramework.GameStructure.GameItems.Messages;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel.Conditions;
using GameFramework.Messaging;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// abstract base for enabling or a disabling a gameobject based upon the settings of a specific Level.
    /// </summary>
    /// <typeparam name="T">The type of the GameItem that we are creating a button for</typeparam>
    public abstract class EnableBasedUponGameItem<T> : GameItemContextConditionallyEnable<T> where T : GameItem
    {
        // NOTE: don't change the order of the below as this is recorded - add to the end and sort
        public enum ConditionTypes { CanUnlockWithCoins, CanUnlockWithCompletion, CanUnlockWithPayment, Coins, PlayerHasCoinsToUnlock, Score, Selected, Unlocked, Custom }

        [Header("Conditions")]
        public ConditionReference[] ConditionReferences = new ConditionReference[0];

        public System.Type[] ListeningMessageTypes;

        /// <summary>
        /// Setup
        /// </summary>
        protected override void Start()
        {
            base.Start();
            ListeningMessageTypes = new [] {typeof (UpdateMessage)};    // Until properly implemented
            //TODO: Get types from conditions, don't add duplicates
            foreach (var type in ListeningMessageTypes)
                GameManager.SafeAddListener(type, EvaluateConditionChanges);

            ////TODO: Selection changes etc...
            //foreach (var conditionReference in ConditionReferences)
            //{
            //    switch ((ConditionTypes)conditionReference.Identifier)
            //    {
            //        case ConditionTypes.CanUnlockWithCoins:
            //        case ConditionTypes.CanUnlockWithCompletion:
            //        case ConditionTypes.CanUnlockWithPayment:
            //            //Selection
            //            break;
            //        case ConditionTypes.Coins:
            //            // Coins
            //            break;
            //        case ConditionTypes.PlayerHasCoinsToUnlock:
            //            // Player Coins
            //            break;
            //        case ConditionTypes.Score:
            //            // Score
            //            break;
            //        case ConditionTypes.Selected:
            //            // Selected
            //            break;
            //        case ConditionTypes.Unlocked:
            //            // Unlocked
            //            break;
            //        case ConditionTypes.Custom:
            //            var triggerMessageTypes = conditionReference.ScriptableObject.GetTriggerMessages();
            //            break;
            //    }
            //}

            //GetGameItemManager().Unlocked += Unlocked;
            //GameManager.SafeAddListener<PlayerCoinsChangedMessage>(EvaluateConditionChanges);

            // add selection changed handler always, but not multiple times. This is needed for the Selected Condition
            //if (Context.GetReferencedContextMode() != ObjectModel.GameItemContext.ContextModeType.Selected)
            //    GetGameItemManager().SelectedChanged += SelectedChanged;
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var type in ListeningMessageTypes)
                GameManager.SafeRemoveListener(type, EvaluateConditionChanges);

            //GetGameItemManager().Unlocked -= Unlocked;
            //GameManager.SafeRemoveListener<PlayerCoinsChangedMessage>(EvaluateConditionChanges);

            // add selection changed handler always, but not multiple times.. This is needed for the Selected Condition
            //if (Context.GetReferencedContextMode() != ObjectModel.GameItemContext.ContextModeType.Selected)
            //    GetGameItemManager().SelectedChanged -= SelectedChanged;
        }


        /// <summary>
        /// Called when a GameItem is unlocked.
        /// </summary>
        /// <param name="gameItem"></param>
        //void Unlocked(T gameItem)
        //{
        //    if (gameItem.Number == GameItem.Number)
        //        RunMethod(false);
        //}


        /// <summary>
        /// NOTE: This is an update for now but will probalby move to messaging in the future for performance
        /// </summary>
        //void Update()
        //{
        //    RunMethod(false);
        //}


        /// <summary>
        /// Do nothing for now - handled by Update method. Remove if moving to messaging.
        /// </summary>
        /// <param name="oldItem"></param>
        /// <param name="item"></param>
        protected override void SelectedChanged(T oldItem, T item)
        {
        }


        /// <summary>
        /// Called when a message is received that indicates a possible condition change.
        /// </summary>
        /// <param name="message"></param>
        bool EvaluateConditionChanges(BaseMessage message)
        {
            RunMethod(false);
            return true;
        }


        /// <summary>
        /// Implement this to return whether to show the condition met gameobject (true) or the condition not met one (false)
        /// </summary>
        /// <returns></returns>
        public override bool IsConditionMet(T gameItem)
        {
            var conditionsAreAllTrue = true;
            foreach (var conditionReference in ConditionReferences)
            {
                switch ((ConditionTypes)conditionReference.Identifier)
                {
                    case ConditionTypes.CanUnlockWithCoins:
                        conditionsAreAllTrue &= CanUnlockWithCoins.EvaluateCondition(GameItem,
                            conditionReference.BoolValue);
                        break;
                    case ConditionTypes.CanUnlockWithCompletion:
                        conditionsAreAllTrue &= CanUnlockWithCompletion.EvaluateCondition(GameItem,
                            conditionReference.BoolValue);
                        break;
                    case ConditionTypes.CanUnlockWithPayment:
                        conditionsAreAllTrue &= CanUnlockWithPayment.EvaluateCondition(GameItem,
                            conditionReference.BoolValue);
                        break;
                    case ConditionTypes.Coins:
                        conditionsAreAllTrue &= Coins.EvaluateCondition(GameItem,
                            conditionReference.Comparison, conditionReference.IntValue);
                        break;
                    case ConditionTypes.PlayerHasCoinsToUnlock:
                        conditionsAreAllTrue &= PlayerHasCoinsToUnlock.EvaluateCondition(GameItem,
                            conditionReference.BoolValue);
                        break;
                    case ConditionTypes.Score:
                        conditionsAreAllTrue &= Score.EvaluateCondition(GameItem,
                            conditionReference.Comparison, conditionReference.IntValue);
                        break;
                    case ConditionTypes.Selected:
                        conditionsAreAllTrue &= Selected.EvaluateCondition(GameItem,
                            GetGameItemManager().Selected, conditionReference.BoolValue);
                        break;
                    case ConditionTypes.Unlocked:
                        conditionsAreAllTrue &= ObjectModel.Conditions.Unlocked.EvaluateCondition(GameItem,
                            conditionReference.BoolValue);
                        break;
                    case ConditionTypes.Custom:
                        if (conditionReference.UseScriptableObject)
                            conditionsAreAllTrue &= conditionReference.ScriptableObject.EvaluateCondition(GameItem);
                        break;
                }
            }
            return conditionsAreAllTrue;
        }
    }
}