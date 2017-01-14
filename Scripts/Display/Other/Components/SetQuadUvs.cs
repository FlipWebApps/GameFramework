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

using UnityEngine;

namespace GameFramework.Display.Other.Components
{
    /// <summary>
    /// Set the UV co-ordinates of a quad
    /// </summary>
    /// You can also call the SetUVs method at runtime to apply a new set of UV's.
    /// 
    /// This component must be applied to a gameobject that has a MeshFilter attached.
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu("Game Framework/Display/Other/SetQuadUvs")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class SetQuadUvs : MonoBehaviour
    {

        /// <summary>
        /// The UV values to apply to the Quad
        /// </summary>
        [Tooltip("")]
        public Rect Uvs;

        Mesh _mesh;
        Vector2[] _uvs;

        void Awake()
        {
            _mesh = transform.GetComponent<MeshFilter>().mesh;
            _uvs = new Vector2[_mesh.uv.Length];
            _uvs = _mesh.uv;

            SetUVs(Uvs);
        }

        /// <summary>
        /// Assign the passed UV's to the mesh
        /// </summary>
        /// <param name="uvs"></param>
        public void SetUVs(Rect uvs)
        {
            // bottom left, top right, bottom right, top left.
            Uvs = uvs;

            _uvs[0] = new Vector2(uvs.x, uvs.y);
            _uvs[1] = new Vector2(uvs.x + uvs.width, uvs.y + uvs.height);
            _uvs[2] = new Vector2(uvs.x + uvs.width, uvs.y);
            _uvs[3] = new Vector2(uvs.x, uvs.y + uvs.height);

            // assign the mesh its new UVs
            _mesh.uv = _uvs;
        }
    }
}