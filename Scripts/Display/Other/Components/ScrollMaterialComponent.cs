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
    /// Used for scrolling a material within the gameobjects renderer
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    [AddComponentMenu("Game Framework/Display/Other/ScrollMaterialComponent")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class ScrollMaterialComponent : MonoBehaviour
    {

        /// <summary>
        /// The index of the material to scroll
        /// </summary>
        [Tooltip("The index of the material to scroll")]
        public int MaterialIndex;

        /// <summary>
        /// The rate to animate the material
        /// </summary>
        [Tooltip("The rate to animate the material")]
        public Vector2 AnimationRate = new Vector2(1.0f, 0.0f);

        /// <summary>
        /// The name of the shader texture property (usually _MainTex)
        /// </summary>
        [Tooltip("The name of the shader texture property")]
        public string TextureName = "_MainTex";

        Renderer _renderer;
        Vector2 _uvOffset = Vector2.zero;

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        void LateUpdate()
        {
            _uvOffset += AnimationRate * Time.deltaTime;
            if (_renderer.enabled)
            {
                _renderer.materials[MaterialIndex].SetTextureOffset(TextureName, _uvOffset);
            }
        }
    }
}