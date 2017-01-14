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
    /// Creates a background for the camera, which is a simple gradient blend between three colors.
    /// </summary>
    /// Attach this script to your camera, and change the top and bottom colors in the inspector as desired. When run, 
    /// the clear flags for your camera is set to Depth Only, allowing a newly created background camera to show through. 
    /// A plane with the gradient colors is created, which only the background camera can see. This is done with GradientLayer, 
    /// which is the only layer that the background camera sees, and your camera is set to ignore. The default, 7, is a 
    /// built-in (non-user-editable) layer that's not used for anything as of Unity 3.1. It can be changed as necessary.
    [AddComponentMenu("Game Framework/Display/Other/GradientBackgroundSplitMiddle")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class GradientBackgroundSplitMiddle : MonoBehaviour
    {
        /// <summary>
        /// The color to use at the top of the gradient
        /// </summary>
        [Tooltip("The color to use at the top of the gradient")]
        public Color TopColor = Color.blue;

        /// <summary>
        /// The color to use at the middle of the gradient
        /// </summary>
        [Tooltip("The color to use at the middle of the gradient")]
        public Color MiddleColor = Color.white;

        /// <summary>
        /// The color to use at the bottom of the gradient
        /// </summary>
        [Tooltip("The color to use at the bottom of the gradient")]
        public Color BottomColor = Color.blue;

        /// <summary>
        /// The shader that should be used. Select GradientBackgroundShader here
        /// </summary>
        [Tooltip("The shader that should be used. Select GradientBackgroundShader here")]
        public Shader Shader;

        /// <summary>
        /// The layer on which to generate the gradient
        /// </summary>
        [Tooltip("The layer on which to generate the gradient")]
        public int GradientLayer = 7;

        /// <summary>
        /// The gradient normalised screen top position
        /// </summary>
        [Tooltip("The gradient normalised screen top position")]
        [Range(0, 1)]
        public float NormalisedTop = 1;

        /// <summary>
        /// The gradient normalised screen middle position
        /// </summary>
        [Tooltip("The gradient normalised screen middle position")]
        [Range(0, 1)]
        public float NormalisedMiddle = 0.5f;

        /// <summary>
        /// The gradient normalised screen bottom position
        /// </summary>
        [Tooltip("The gradient normalised screen bottom position")]
        [Range(0, 1)]
        public float NormalisedBottom = 0;

        void Awake()
        {
            var attachedCamera = gameObject.GetComponent<Camera>();
            GradientLayer = Mathf.Clamp(GradientLayer, 0, 31);
            if (!attachedCamera)
            {
                Debug.LogError("You must attach a GradientBackground script to the same gameobject as a Camera");
                return;
            }

            attachedCamera.clearFlags = CameraClearFlags.Depth;
            attachedCamera.cullingMask = attachedCamera.cullingMask & ~(1 << GradientLayer);
            var gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
            gradientCam.depth = attachedCamera.depth - 1;
            gradientCam.cullingMask = 1 << GradientLayer;

            //1.154 is the height of a plane at z=0 with the default camera field of view of 60. .577 is half this.
            var top = -.577f + (NormalisedTop * 1.154f);
            var bottom = -.577f + (NormalisedBottom * 1.154f);
            var middle = -.577f + (NormalisedMiddle * 1.154f);

            // create top plane
            var mesh = new Mesh
            {
                vertices =
                    new[]
                    {
                        new Vector3(-100f, top, 1f), new Vector3(100f, top, 1f), new Vector3(-100f, middle, 1f),
                        new Vector3(100f, middle, 1f)
                    },
                colors = new[] { TopColor, TopColor, MiddleColor, MiddleColor },
                triangles = new[] { 0, 1, 2, 1, 3, 2 }
            };

            var mat = new Material(Shader);
            var gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

            ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
            var gradRend = gradientPlane.GetComponent<Renderer>();
            gradRend.material = mat;
            gradientPlane.layer = GradientLayer;

            // create bottom plane as an inverse copy of the top
            mesh = new Mesh
            {
                vertices =
                    new[]
                    {
                        new Vector3(-100f, middle, 1f), new Vector3(100f, middle, 1f), new Vector3(-100f, bottom, 1f),
                        new Vector3(100f, bottom, 1f)
                    },
                colors = new[] { MiddleColor, MiddleColor, BottomColor, BottomColor },
                triangles = new[] { 0, 1, 2, 1, 3, 2 }
            };

            mat = new Material(Shader);
            gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

            ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
            gradRend = gradientPlane.GetComponent<Renderer>();
            gradRend.material = mat;
            gradientPlane.layer = GradientLayer;

        }

    }
}