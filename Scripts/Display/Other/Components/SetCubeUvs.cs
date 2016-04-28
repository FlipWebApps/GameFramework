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
    [AddComponentMenu("Game Framework/Display/Other/SetCubeUvs")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class SetCubeUvs : MonoBehaviour
    {

        public bool AllSameAsFront;
        public Rect UvsFront;
        public Rect UvsBack;
        public Rect UvsLeft;
        public Rect UvsRight;
        public Rect UvsTop;
        public Rect UvsBottom;

        Mesh _theMesh;
        Vector2[] _theUVs;

        void Awake()
        {
            if (AllSameAsFront)
            {
                UvsBack = UvsFront;
                UvsLeft = UvsFront;
                UvsRight = UvsFront;
                UvsTop = UvsFront;
                UvsBottom = UvsFront;
            }

            _theMesh = transform.GetComponent<MeshFilter>().mesh;
            _theUVs = new Vector2[_theMesh.uv.Length];
            _theUVs = _theMesh.uv;

            SetUVs();
        }

        void SetUVs()
        {
            // - set UV coordinates -

            // FRONT    2    3    0    1
            _theUVs[2] = new Vector2(UvsFront.x, UvsFront.y);
            _theUVs[3] = new Vector2(UvsFront.x + UvsFront.width, UvsFront.y);
            _theUVs[0] = new Vector2(UvsFront.x, UvsFront.y - UvsFront.height);
            _theUVs[1] = new Vector2(UvsFront.x + UvsFront.width, UvsFront.y - UvsFront.height);

            // BACK    6    7   10   11
            _theUVs[6] = new Vector2(UvsBack.x, UvsBack.y);
            _theUVs[7] = new Vector2(UvsBack.x + UvsBack.width, UvsBack.y);
            _theUVs[10] = new Vector2(UvsBack.x, UvsBack.y - UvsBack.height);
            _theUVs[11] = new Vector2(UvsBack.x + UvsBack.width, UvsBack.y - UvsBack.height);

            // LEFT   19   17   16   18
            _theUVs[19] = new Vector2(UvsLeft.x, UvsLeft.y);
            _theUVs[17] = new Vector2(UvsLeft.x + UvsLeft.width, UvsLeft.y);
            _theUVs[16] = new Vector2(UvsLeft.x, UvsLeft.y - UvsLeft.height);
            _theUVs[18] = new Vector2(UvsLeft.x + UvsLeft.width, UvsLeft.y - UvsLeft.height);

            // RIGHT   23   21   20   22
            _theUVs[23] = new Vector2(UvsRight.x, UvsRight.y);
            _theUVs[21] = new Vector2(UvsRight.x + UvsRight.width, UvsRight.y);
            _theUVs[20] = new Vector2(UvsRight.x, UvsRight.y - UvsRight.height);
            _theUVs[22] = new Vector2(UvsRight.x + UvsRight.width, UvsRight.y - UvsRight.height);

            // TOP    4    5    8    9
            _theUVs[4] = new Vector2(UvsTop.x, UvsTop.y);
            _theUVs[5] = new Vector2(UvsTop.x + UvsTop.width, UvsTop.y);
            _theUVs[8] = new Vector2(UvsTop.x, UvsTop.y - UvsTop.height);
            _theUVs[9] = new Vector2(UvsTop.x + UvsTop.width, UvsTop.y - UvsTop.height);

            // BOTTOM   15   13   12   14
            _theUVs[15] = new Vector2(UvsBottom.x, UvsBottom.y);
            _theUVs[13] = new Vector2(UvsBottom.x + UvsBottom.width, UvsBottom.y);
            _theUVs[12] = new Vector2(UvsBottom.x, UvsBottom.y - UvsBottom.height);
            _theUVs[14] = new Vector2(UvsBottom.x + UvsBottom.width, UvsBottom.y - UvsBottom.height);

            // - Assign the mesh its new UVs -
            _theMesh.uv = _theUVs;
        }
    }
}