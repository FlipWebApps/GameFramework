//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
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

            var fadeLevelGameObject = new GameObject {name = "_FadeLevel"};
            fadeLevelGameObject.SetActive(false);

            Canvas myCanvas = fadeLevelGameObject.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

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