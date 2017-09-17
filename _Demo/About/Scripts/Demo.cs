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
using GameFramework.GameFramework;
using GameFramework.GameStructure;
using GameFramework.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework._Demo.About.Scripts
{
    public class Demo : MonoBehaviour
    {
        public CanvasGroup[] Pages;
        public Text ProgressText;
        public AudioClip ClickAudioClip;

        public float PlaybackSpeed { get; set; }    // Playback Speed in seconds.

        int _pageIndex;
        float _autoCounter;

        void Start()
        {
            PlaybackSpeed = PlayerPrefs.GetFloat("GameFramework.Demo.PlaybackSpeed", 30);
            SetProgressText();
            var isFirstPage = true;
            foreach (var canvasGroup in Pages)
            {
                canvasGroup.alpha = isFirstPage ? 1 : 0;
                canvasGroup.gameObject.SetActive(true);
                isFirstPage = false;
            }
        }

        void Update()
        {
            if (DialogManager.Instance.Count != 0) return;

            _autoCounter += Time.deltaTime;
            if (_autoCounter > PlaybackSpeed)
                NextPageNoAudio();
        }

        public void NextPage()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            NextPageNoAudio();
        }

        private void NextPageNoAudio()
        {
            _autoCounter = 0;
            StartCoroutine(FadeCanvasGroup(_pageIndex, 0, 0.5f, 1, 0));
            StartCoroutine(FadeCanvasGroup(_pageIndex + 1 < Pages.Length ? _pageIndex + 1 : 0, 0.3f, 0.5f, 0, 1));
        }

        public void PreviousPage()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            _autoCounter = 0;
            StartCoroutine(FadeCanvasGroup(_pageIndex, 0, 0.5f, 1, 0));
            StartCoroutine(FadeCanvasGroup(_pageIndex <= 0 ? Pages.Length - 1 : _pageIndex - 1, 0.3f, 0.5f, 0, 1));
        }

        public IEnumerator FadeCanvasGroup(int pageIndex, float delay, float duration, float startAmount, float endAmount)
        {
            if (!Mathf.Approximately(delay, 0)) yield return new WaitForSeconds(delay);

            _pageIndex = pageIndex;
            SetProgressText();

            var lerpPercent = 0f;
            while (lerpPercent < 1)
            {
                lerpPercent += ((1 / duration) * Time.deltaTime);
                Pages[pageIndex].alpha = Mathf.Lerp(startAmount, endAmount, lerpPercent);
                yield return 0;
            }
        }

        void SetProgressText()
        {
            ProgressText.text = string.Format("({0}/{1})", _pageIndex + 1, Pages.Length);
        }

        public void OnlineTutorials()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            GameFrameworkHelper.ShowOnlineTutorials();
        }

        

        public void ShowAssetStorePage()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            GameFrameworkHelper.ShowAssetStorePage();
        }

        public void ShowPaidVersion()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            GameFrameworkHelper.ShowAssetStorePageExtrasBundle();
        }

    }
}