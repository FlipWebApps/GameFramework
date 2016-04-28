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

namespace FlipWebApps.GameFramework.Scripts.Display.Other.Components
{
    /// <summary>
    /// Set the UV co-ordinates of a cube
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu("Game Framework/Display/Other/SetQuadUvs")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class SetQuadUvs : MonoBehaviour
    {
        public Rect Uvs;

        Mesh _theMesh;
        Vector2[] _theUVs;

        void Awake()
        {
            _theMesh = transform.GetComponent<MeshFilter>().mesh;
            _theUVs = new Vector2[_theMesh.uv.Length];
            _theUVs = _theMesh.uv;

            SetUVs(Uvs);
        }

        public void SetUVs(Rect uvs)
        {
            // bottom left, top right, bottom right, top left.
            Uvs = uvs;

            _theUVs[0] = new Vector2(uvs.x, uvs.y);
            _theUVs[1] = new Vector2(uvs.x + uvs.width, uvs.y + uvs.height);
            _theUVs[2] = new Vector2(uvs.x + uvs.width, uvs.y);
            _theUVs[3] = new Vector2(uvs.x, uvs.y + uvs.height);

            // assign the mesh its new UVs
            _theMesh.uv = _theUVs;
        }
    }
}