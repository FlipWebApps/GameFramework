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
    /// Description
    /// Creates a background for the camera, which is a simple gradient blend between two colors.
    /// Usage
    /// Attach this script to your camera, and change the top and bottom colors in the inspector as desired. When run, the clear flags for your camera is set to Depth Only, allowing a newly created background camera to show through. A plane with the gradient colors is created, which only the background camera can see. This is done with GradientLayer, which is the only layer that the background camera sees, and your camera is set to ignore. The default, 7, is a built-in (non-user-editable) layer that's not used for anything as of Unity 3.1. It can be changed as necessary.
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Other/GradientBackgroundSplitMiddle")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GradientBackgroundSplitMiddle : MonoBehaviour
    {

        public Color TopColor = Color.blue;
        public Color MiddleColor = Color.white;
        public Color BottomColor = Color.blue;
        public int GradientLayer = 7;
        public float MiddlePosition;

        void Awake()
        {
            Debug.LogError("Script needs updating");
            /*
            GradientLayer = Mathf.Clamp(GradientLayer, 0, 31);
            if (!GetComponent<Camera>())
            {
                Debug.LogError("Must attach GradientBackground script to the camera");
                return;
            }

            GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
            GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << GradientLayer);
            Camera gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
            gradientCam.depth = GetComponent<Camera>().depth - 1;
            gradientCam.cullingMask = 1 << GradientLayer;

            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[4] { new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, middlePosition, 1f), new Vector3(100f, middlePosition, 1f) };

            mesh.colors = new Color[4] { topColor, topColor, middleColor, middleColor };

            mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

            Material mat = new Material("Shader \"Vertex Color Only\"{Subshader{BindChannels{Bind \"vertex\", vertex Bind \"color\", color}Pass{}}}");
            GameObject gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

            ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
            gradientPlane.GetComponent<Renderer>().material = mat;
            gradientPlane.layer = GradientLayer;

            // create bottom gradient as reverse of top
            mesh = new Mesh();
            mesh.vertices = new Vector3[4] { new Vector3(-100f, middlePosition, 1f), new Vector3(100f, middlePosition, 1f), new Vector3(-100f, -.577f, 1f), new Vector3(100f, -.577f, 1f) };

            mesh.colors = new Color[4] { middleColor, middleColor, bottomColor, bottomColor };

            mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

            gradientPlane = new GameObject("Gradient Plane Bottom", typeof(MeshFilter), typeof(MeshRenderer));

            ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
            gradientPlane.GetComponent<Renderer>().material = mat;
            gradientPlane.layer = GradientLayer;
        */
        }

    }
}