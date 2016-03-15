//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections;
using FlipWebApps.GameFramework.Scripts.GameStructure;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Demo.Scripts
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
            Application.OpenURL("http://www.flipwebapps.com/game-framework/tutorials/");
        }

        

        public void ShowAssetStorePage()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/55334");
        }

        public void ShowPaidVersion()
        {
            GameManager.Instance.PlayEffect(ClickAudioClip);
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/50893");
        }

    }
}