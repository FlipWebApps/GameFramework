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

using FlipWebApps.GameFramework.Scripts.GameStructure.Levels;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Helper.Components
{
    /// <summary>
    /// A collection of generic callback functions that can be used from animators or otherwise.
    /// 
    /// You can specify a reference to have these operate on a gameobject other than the one where this is currently attached.
    /// </summary>
    [AddComponentMenu("Game Framework/Helper/GenericCallbackFunctions")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GenericCallbackFunctions : MonoBehaviour
    {
        public GameObject GameObject;

        public void Awake()
        {
            if (GameObject == null)
                GameObject = gameObject;
        }

        public void DestroyGameObject()
        {
            Destroy(GameObject);
        }

        public void DestroyParentGameObject()
        {
            Destroy(GameObject.transform.parent.gameObject);
        }

        public void DisableGameObject()
        {
            GameObject.SetActive(false);
        }

        public void EnableGameObject()
        {
            GameObject.SetActive(true);
        }

        public void LevelStarted()
        {
            LevelManager.Instance.LevelStarted();
        }

    }
}