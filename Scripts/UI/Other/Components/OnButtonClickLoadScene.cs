//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    /// <summary>
    /// Load the specified scene when the button is clicked.
    /// 
    /// This automatically hooks up the button onClick listener
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class OnButtonClickLoadScene : MonoBehaviour
    {
        public string SceneName;
        public bool Fade;

        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            if (Fade)
                FadeLevelManager.Instance.LoadScene(SceneName);
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
    }
}