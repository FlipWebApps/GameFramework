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
using UnityEditor;
using UnityEngine;
using DB = UnityEngine.Debug;

namespace GameFramework.GameFramework.Editor {
    /// <summary>
    /// Various helper options for selecting items with in the current scene
    /// </summary>
    public class SelectItems : MonoBehaviour {

        /// <summary>
        /// select all items within the numbered layer
        /// </summary>
        /// <param name="layerNum"></param>
        static void SelectLayer(int layerNum) {
            var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);

            var list = new List<Object>(objs.Length);

            foreach (var obj in objs) {
                var go = obj as GameObject;

                if (go == null) continue;

                if (go.layer == layerNum)
                {
                    list.Add(go);
                }

                Selection.objects = list.ToArray();
            }

            var layerName = LayerMask.LayerToName(layerNum);
            DB.Log(string.Format("Found {0} objects in layer \"{1}\"", list.Count, string.IsNullOrEmpty(layerName) ? layerNum.ToString() : layerName));
        }


        /// <summary>
        /// select all items visible from teh main camera
        /// </summary>
        /// <param name="visible"></param>
        static void SelectVisibleFromMainCamera(bool visible)
        {
            var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);

            var list = new List<Object>(objs.Length);

            foreach (var obj in objs)
            {
                var go = obj as GameObject;

                if (go == null) continue;

                if (go.GetComponent<Renderer>() != null)
                {
                    if (IsVisibleFrom(go.GetComponent<Renderer>(), Camera.main) == visible)
                        list.Add(go);
                }

                Selection.objects = list.ToArray();
            }

            DB.Log(string.Format("Found {0} objects", list.Count));
        }

        public static bool IsVisibleFrom(Renderer renderer, Camera camera)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }


        /// <summary>
        /// Finds and optionally delete all colliders from the selected gameobjects
        /// </summary>
        /// <param name="remove"></param>
        static void FindPhysicsComponentsFromSelected(bool remove)
        {
            var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);

            var list = new List<Object>(objs.Length);

            foreach (var obj in objs)
            {
                var go = obj as GameObject;

                if (go == null) continue;

                if (go.GetComponent<Collider>() != null)
                {
                    list.Add(go);
                    if (remove)
                    {
                        DestroyImmediate(go.GetComponent<Collider>());
                    }
                }

            }

            if (!remove)
                Selection.objects = list.ToArray();

            DB.Log(string.Format("Found {0} objects", list.Count));
        }


        /// <summary>
        /// Select all game objects that are empty and don't have any components
        /// </summary>
        static void SelectEmptyGameObjects() //bool remove)
        {
            var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);

            var list = new List<Object>(objs.Length);

            foreach (var obj in objs)
            {
                var go = obj as GameObject;

                if (go == null) continue;

                if (go.GetComponents(typeof(Component)).Length <= 1 && go.transform.childCount == 0)
                {
                    list.Add(go);
                }

            }

            Selection.objects = list.ToArray();

            DB.Log(string.Format("Found {0} objects", list.Count));
        }


        #region Menu items
        [MenuItem("Window/Game Framework/Select Items/Visible From Camera/Visible")]
        static void SelectVisibleFromMainCamera0()
        {
            SelectVisibleFromMainCamera(true);
        }
        [MenuItem("Window/Game Framework/Select Items/Visible From Camera/Not Visible")]
        static void SelectVisibleFromMainCamera1()
        {
            SelectVisibleFromMainCamera(false);
        }


        [MenuItem("Window/Game Framework/Select Items/Select Colliders")]
        static void RemovePhysicsComponents0() {
            FindPhysicsComponentsFromSelected(false);
        }

        [MenuItem("Window/Game Framework/Select Items/Remove Colliders")]
        static void RemovePhysicsComponents1()
        {
            FindPhysicsComponentsFromSelected(true);
        }

        [MenuItem("Window/Game Framework/Select Items/Select Empty")]
        static void SelectEmptyGameObjects0()
        {
            SelectEmptyGameObjects();
        }

        [MenuItem("Window/Game Framework/Select Items/With Layer/0")]
        static void SelectLayer0()
        {
            SelectLayer(0);
        }

        [MenuItem("Window/Game Framework/Select Items/With Layer/1")]
        static void SelectLayer1()
        {
            SelectLayer(1);
        }

        #endregion
    }
}