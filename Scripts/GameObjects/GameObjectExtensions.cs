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
using Object = UnityEngine.Object;
#if NETFX_CORE
using System.Reflection;
#endif

namespace GameFramework.GameObjects
{
    /// <summary>
    /// Extension methods for game objects
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns all monobehaviours (casted to T)
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetInterfaces<T>(this GameObject gObj)
        {
#if !NETFX_CORE
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
#endif
            var mObjs = gObj.GetComponents<MonoBehaviour>();


#if NETFX_CORE
            return (from a in mObjs where a.GetType().GetTypeInfo().ImplementedInterfaces.Any(k => k == typeof(T)) select (T)(object)a).ToArray();
#else
            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
#endif
        }

        /// <summary>
        /// Returns the first monobehaviour that is of the interface type (casted to T)
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetInterface<T>(this GameObject gObj)
        {
#if !NETFX_CORE
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
#endif
            return gObj.GetInterfaces<T>().FirstOrDefault();
        }

        /// <summary>
        /// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetInterfaceInChildren<T>(this GameObject gObj)
        {
#if !NETFX_CORE
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
#endif
            return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets all monobehaviours in children that implement the interface of type T (casted to T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
        {
#if !NETFX_CORE
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
#endif
            var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();

#if NETFX_CORE
            return (from a in mObjs where a.GetType().GetTypeInfo().ImplementedInterfaces.Any(k => k == typeof(T)) select (T)(object)a).ToArray();
#else
            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
#endif
        }

        /// <summary>
        /// Calls GameObject.Destroy on all children of transform. and immediately detaches the children
        /// from transform so after this call tranform.childCount is zero.
        /// </summary>
        public static void DestroyChildren(this GameObject gObj)
        {
            for (int i = gObj.transform.childCount - 1; i >= 0; --i)
            {
                Object.Destroy(gObj.transform.GetChild(i).gameObject);
            }
            gObj.transform.DetachChildren();
        }

        /// <summary>
        /// Get the path to the gameobject by iterating through the parent items.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static string GetPath(this GameObject current)
        {
            if (current.transform.parent == null)
                return "/" + current.name;
            return current.transform.parent.gameObject.GetPath() + "/" + current.name;
        }
    }
}