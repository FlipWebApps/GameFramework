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

using System.Collections;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    [System.Obsolete("Discontinued in favour of the Beautiful Transitions Assets package!")]
    public class FadeLevelManager : Singleton<FadeLevelManager>
    {
        public Color FadeColour = Color.black;
        public float FadeTime = 0.2f;
        public bool ShouldFadeIn = true;

        CanvasRenderer _canvasRenderer;
        RawImage _fadeRawImage;

        protected override void GameSetup()
        {
            base.GameSetup();

            Debug.LogWarning(
                "The FadeLevelManager component used on " + gameObject.name + " is deprecated in favour of the Beautiful Transitions assets included in the pro bundle or avialable seperately.");

            var fadeLevelGameObject = new GameObject {name = "_FadeLevel"};
            fadeLevelGameObject.SetActive(false);

            Canvas myCanvas = fadeLevelGameObject.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            myCanvas.sortingOrder = 999;

            _fadeRawImage = fadeLevelGameObject.AddComponent<RawImage>();
            _fadeRawImage.color = FadeColour;

            _canvasRenderer = fadeLevelGameObject.GetComponent<CanvasRenderer>();
            _canvasRenderer.SetAlpha(0);

            if (ShouldFadeIn)
            {
                StartCoroutine(FadeIn());
            }
        }

        public void LoadScene(string level)
        {
            StartCoroutine(FadeOut(level));
        }

        IEnumerator FadeIn()
        {
            _canvasRenderer.SetAlpha(1);
            _fadeRawImage.gameObject.SetActive(true);

            _fadeRawImage.CrossFadeAlpha(0, FadeTime, false);

            yield return new WaitForSeconds(FadeTime);

            _fadeRawImage.gameObject.SetActive(false);
        }

        IEnumerator FadeOut(string level)
        {
            _canvasRenderer.SetAlpha(0);
            _fadeRawImage.gameObject.SetActive(true);

            _fadeRawImage.CrossFadeAlpha(1, FadeTime, false);

            yield return new WaitForSeconds(FadeTime);

            UnityEngine.SceneManagement.SceneManager.LoadScene(level);
        }
    }
}