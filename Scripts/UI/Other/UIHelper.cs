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
using GameFramework.GameObjects;
using GameFramework.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Other
{
    /// <summary>
    /// UI Helper functions
    /// </summary>
    internal class UIHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="childObjectName"></param>
        /// <param name="text"></param>
        /// <param name="includeInactive"></param>
        /// <returns>bool indicating whether the text is set or not (vhild gameobject not found)</returns>
        public static bool SetTextOnChildGameObject(GameObject thisGameObject, string childObjectName, string text, bool includeInactive = false)
        {
            var uiText = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Text>(thisGameObject, childObjectName, includeInactive);
            if (uiText == null) return false;
            uiText.text = text;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="childObjectName"></param>
        /// <param name="textKey"></param>
        /// <param name="includeInactive"></param>
        /// <returns>bool indicating whether the text is set or not (vhild gameobject not found)</returns>
        public static bool SetTextOnChildGameObjectLocalised(GameObject thisGameObject, string childObjectName, string textKey, bool includeInactive = false)
        {
            return SetTextOnChildGameObject(thisGameObject, childObjectName, GlobalLocalisation.GetText(textKey, missingReturnsKey: true), includeInactive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="childObjectName"></param>
        /// <param name="sprite"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static void SetSpriteOnChildGameObject(GameObject thisGameObject, string childObjectName, Sprite sprite, bool includeInactive = false)
        {
            var uiImage = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Image>(thisGameObject, childObjectName, includeInactive);
            Assert.IsNotNull(uiImage);
            uiImage.sprite = sprite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="childObjectName"></param>
        /// <param name="isOn"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static void SetToggleStateOnChildGameObject(GameObject thisGameObject, string childObjectName, bool isOn, bool includeInactive = false)
        {
            var toggle = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Toggle>(thisGameObject, childObjectName, includeInactive);
            Assert.IsNotNull(toggle);
            toggle.isOn = isOn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="childObjectName"></param>
        /// <param name="value"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static void SetSliderValueOnChildGameObject(GameObject thisGameObject, string childObjectName, float value, bool includeInactive = false)
        {
            var slider = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Slider>(thisGameObject, childObjectName, includeInactive);
            Assert.IsNotNull(slider);
            slider.value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="childObjectName"></param>
        /// <param name="color"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static void SetImageColorOnChildGameObject(GameObject thisGameObject, string childObjectName, Color color, bool includeInactive = false)
        {
            var image = GameObjectHelper.GetChildComponentOnNamedGameObject<UnityEngine.UI.Image>(thisGameObject, childObjectName, includeInactive);
            Assert.IsNotNull(image);
            image.color = color;
        }
        

        /// <summary>
        /// Sets the user interface label text on child game object - Allows passing 2 game object names where the second may be generic among similar groups
        /// (saves having seperate scripts for each group).
        /// </summary>
        /// <param name="thisGameObject">This game object.</param>
        /// <param name="childObjectName1">Child object name1.</param>
        /// <param name="childObjectName2">Child object name2.</param>
        /// <param name="text">Text.</param>
        /// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
        public static void SetTextOnChildGameObject(GameObject thisGameObject, string childObjectName1, string childObjectName2, string text, bool includeInactive = false)
        {
            throw new Exception("This method doesn't seem to work as expected due to internal Unity workings.");
            //GameObject childGameObject1 = GameObjectHelper.GetChildNamedGameObject (thisGameObject, childObjectName1, includeInactive);
            //MyDebug.NotNull(childGameObject1);
            //SetUILabelTextOnChildGameObject(childGameObject1, childObjectName2, text, includeInactive);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="position"></param>
        ///// <returns></returns>
        //static public bool Is3DUIInput(Vector3 position)
        //{
        //    foreach (Camera camera in Camera.allCameras)
        //    {
        //        if (camera.name == "NGUI Camera")
        //        {
        //            // pos is the Vector3 representing the screen position of the input
        //            Ray inputRay = camera.ScreenPointToRay(position);
        //            RaycastHit hit;

        //            if (Physics.Raycast(inputRay.origin, inputRay.direction, out hit, Mathf.Infinity, LayerMask.NameToLayer("NGUI")))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="position"></param>
        ///// <returns></returns>
        //static public bool Is2DUIInput(Vector3 position, string layerName = "NGUI", string cameraName = "NGUI Camera")
        //{
        //    foreach (Camera camera in Camera.allCameras)
        //    {
        //        if (camera.name == cameraName)
        //        {
        //            // pos is the Vector3 representing the screen position of the input
        //            //Ray inputRay = camera.ScreenPointToRay(position);
        //            //RaycastHit hit;

        //            if (Physics2D.OverlapPoint(camera.ScreenToWorldPoint(position), 1 << LayerMask.NameToLayer(layerName)) != null)
        //            //Physics2D.Raycast(inputRay.origin, inputRay.direction, Mathf.Infinity, LayerMask.NameToLayer("NGUI")).collider != null)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}