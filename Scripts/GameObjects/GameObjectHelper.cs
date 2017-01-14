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
using System.Linq;
using UnityEngine;

namespace GameFramework.GameObjects
{
    /// <summary>
    /// Helper functions for quickly and simply finding and manipulating game objects
    /// </summary>
    internal class GameObjectHelper
    {
        /// <summary>
        /// Get a component of the first child gameobject with the given name.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="thisGameObject"></param>
        /// <param name="name"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static TComponent GetChildComponentOnNamedGameObject<TComponent>(GameObject thisGameObject, String name, bool includeInactive = false) where TComponent : Component
        {
            TComponent[] components = thisGameObject.GetComponentsInChildren<TComponent>(includeInactive);
            return components.FirstOrDefault(component => component.gameObject.name == name);
        }

        /// <summary>
        /// Get the first child gameobject with the given name
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="name"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static GameObject GetChildNamedGameObject(GameObject thisGameObject, string name, bool includeInactive = false)
        {
            Transform[] components = thisGameObject.GetComponentsInChildren<Transform>(includeInactive);
            return (from component in components where component.gameObject.name == name select component.gameObject).FirstOrDefault();
        }

        /// <summary>
        /// Get the first parent gameobject with teh given name
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetParentNamedGameObject(GameObject thisGameObject, string name)
        {
            while (true)
            {
                if (thisGameObject.name == name) return thisGameObject;
                if (thisGameObject.transform.parent == null) return null;
                thisGameObject = thisGameObject.transform.parent.gameObject;
            }
        }

        /// <summary>
        /// Disable a gameobject for removal by disabling physics and placing out of view.
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DisableGameObjectForRemoval(GameObject gameObject)
        {
            if (gameObject.GetComponent<Collider2D>() != null)
                gameObject.GetComponent<Collider2D>().enabled = false;
            if (gameObject.GetComponent<Rigidbody2D>() != null)
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.transform.position = new Vector3(0, -20); // place out of view
        }

        /// <summary>
        /// Recursively set gameobjects to the specified layer name
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layerName"></param>
        public static void SetLayerRecursive(GameObject gameObject, string layerName)
        {
            SetLayerRecursive(gameObject, LayerMask.NameToLayer(layerName));
        }

        /// <summary>
        /// Recursively set gameobjects to the specified layer
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursive(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetLayerRecursive(gameObject.transform.GetChild(i).gameObject, layer);
            }
        }

        /// <summary>
        /// Destroy all child gameobjects
        /// </summary>
        /// <param name="root"></param>
        public static void DestroyChildren(Transform root)
        {
            int childCount = root.childCount;
            for (int i = 0; i < childCount; i++)
            {
                UnityEngine.Object.Destroy(root.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Safely set a gameobjects active state testing first for whether is has been destroyed.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SafeSetActive(GameObject gameObject, bool value)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(value);
                return true;
            }
            return false;
        }
    }

}