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
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.GameStructure.Game
{
    /// <summary>
    /// Helper class for GameAction's
    /// </summary>
    public class GameActionHelper
    {
        // enum for target types
        public enum TargetType { ThisGameObject, Specified, CollidingGameObject }

        /// <summary>
        /// Initialise actions
        /// </summary>
        /// <returns></returns>
        public static void InitialiseGameActions(IEnumerable<GameActionReference> gameActionReferences, MonoBehaviour owner)
        {
            foreach (var actionReference in gameActionReferences)
            {
                if (actionReference.IsReference)
                {
                    if (actionReference.ScriptableObjectReference != null)
                        actionReference.ScriptableObjectReference.InitialiseInternal(owner);
                }
                else
                {
                    var action = actionReference.ScriptableObject;
                    //var actionDelayed = action as GameActionDelayed;

                    //if (actionDelayed)
                    //    actionDelayed.InitialiseCommon();
                    //else
                    action.InitialiseInternal(owner);
                }
            }
        }


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        public static void ExecuteGameActions(IEnumerable<GameActionReference> actionReferences, bool isStart, GameAction.GameActionInvocationContext context)
        {
            foreach (var actionReference in actionReferences)
            {
                if (actionReference.IsReference)
                {
                    if (actionReference.ScriptableObjectReference != null)
                    {
                        actionReference.ScriptableObjectReference.InvocationContext = context;
                        actionReference.ScriptableObjectReference.ExecuteInternal(isStart);
                    }
                }
                else
                {
                    var action = actionReference.ScriptableObject;
                    actionReference.ScriptableObject.InvocationContext = context;
                    action.ExecuteInternal(isStart);
                }
            }
        }


        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        public static void ExecuteGameActions(IEnumerable<GameActionReference> actionReferences, bool isStart)
        {
            ExecuteGameActions(actionReferences, isStart, new GameAction.GameActionInvocationContext());
        }

        /// <summary>
        /// Resolve the target gameobject based upon the specified TargetType
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="gameAction"></param>
        /// <param name="specified"></param>
        /// <param name="warnIfNull"></param>
        /// <returns></returns>
        public static GameObject ResolveTarget(TargetType targetType, GameAction gameAction, GameObject specified, bool warnIfNull = true)
        {
            switch (targetType)
            {
                case TargetType.ThisGameObject:
                    return gameAction.Owner.gameObject;
                case TargetType.CollidingGameObject:
                    return gameAction.InvocationContext.OtherGameObject;
                case TargetType.Specified:
                    return specified;
            }
            if (warnIfNull)
                Debug.LogWarningFormat("No Target is specified for the action {0} on {1}", gameAction.GetType().Name, gameAction.Owner.gameObject.name);
            return null;
        }


        public static T ResolveTargetComponent<T>(TargetType targetType, GameAction gameAction, T specified, bool warnIfNull = true) where T : Component
        {
            switch (targetType)
            {
                case TargetType.ThisGameObject:
                    return gameAction.Owner.gameObject.GetComponent<T>();
                case TargetType.CollidingGameObject:
                    return gameAction.InvocationContext.OtherGameObject.GetComponent<T>();
                case TargetType.Specified:
                    return specified;
            }
            if (warnIfNull)
                Debug.LogWarningFormat("No Target is specified for the action {0} on {1}", gameAction.GetType().Name, gameAction.Owner.gameObject.name);
            return null;
        }
    }
}
