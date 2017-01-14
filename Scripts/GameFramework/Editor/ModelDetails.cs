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

using UnityEditor;
using UnityEngine;
using DB = UnityEngine.Debug;

namespace GameFramework.GameFramework.Editor {
    /// <summary>
    /// Various helper options for models
    /// </summary>
    public class ModelDetails : MonoBehaviour {
        /// <summary>
        /// Return some information about the selected model
        /// </summary>
        static void GetModelDetails()
        {
            var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);

            int numTriangles = 0;
            int numVertices = 0;

            foreach (var obj in objs)
            {
                var go = obj as GameObject;

                if (go == null) continue;

                var meshes = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach (var mesh in meshes)
                {
                    numVertices += mesh.sharedMesh.vertexCount;
                    numTriangles += mesh.sharedMesh.triangles.Length / 3;
                }
            }

            DB.Log(string.Format("Found {0} vertices and {1} triangles", numVertices, numTriangles));
        }

        #region Menu items
        [MenuItem("Window/Game Framework/3D/Model Details")]
        static void ModelDetails0()
        {
            GetModelDetails();
        }

        #endregion
    }
}