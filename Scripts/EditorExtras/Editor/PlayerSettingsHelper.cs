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
using System.Collections.Generic;
using UnityEditor;

namespace GameFramework.EditorExtras.Editor
{
    /// <summary>
    /// Helper methods for interacting with the player settings
    /// </summary>
    public class PlayerSettingsHelper
    {
        #region Scripting Defines

        /// <summary>
        /// Returns whether the Player Settings scripting define group contains the specified value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buildTarget"></param>
        /// <returns></returns>
        public static bool IsScriptingDefineSet(string value, BuildTargetGroup buildTarget = BuildTargetGroup.Standalone)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget).Split(';');
            var list = new List<string>(defines);
            return list.Contains(value);
        }


        /// <summary>
        /// Remove a scripting define value from all targets
        /// </summary>
        /// <param name="value"></param>
        public static void RemoveScriptingDefineAllTargets(string value)
        {
            if (!IsScriptingDefineSet(value)) return;

            foreach (BuildTargetGroup group in System.Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (!IsValid(group)) continue;
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';');
                var list = new List<string>(defines);
                list.Remove(value);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", list.ToArray()));
            }
        }


        /// <summary>
        /// Add a scripting define value from all targets
        /// </summary>
        /// <param name="name"></param>
        public static void AddScriptingDefineAllTargets(string name)
        {
            if (IsScriptingDefineSet(name)) return;

            foreach (BuildTargetGroup group in System.Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (!IsValid(group)) continue;
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';');
                var list = new List<string>(defines);
                list.Add(name);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", list.ToArray()));
            }
        }


        /// <summary>
        /// Returns whether the specified BuildTargetGroup is valid
        /// </summary>
        /// Some groups are deprecated and so we don't want to set these to avoid errors.
        /// <param name="group"></param>
        /// <returns></returns>
        static bool IsValid(BuildTargetGroup group)
        {
            if (group == BuildTargetGroup.Unknown || IsObsolete(group)) return false;

            // Additional checks incase not correctly marked as obsolete by Unity.
            //#if UNITY_5_3
            //            if ((int)group == 15) return false;
            //#endif

            return true;
        }


        /// <summary>
        /// Tese whether the specified enum valid is marked as obsolete.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static bool IsObsolete(System.Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes.Length > 0;
        }
        #endregion Scripting Defines
    }
}