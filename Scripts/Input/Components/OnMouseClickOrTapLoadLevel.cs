//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using FlipWebApps.GameFramework.Scripts.GameStructure;

namespace FlipWebApps.GameFramework.Scripts.Input.Components
{
    /// <summary>
    /// Loads the given level when a mouse button is pressed or the screen is tapped anywhere on teh screen
    /// 
    /// By setting a list of UI game objects you can set areas that wonwill block the change.
    /// </summary>
    public class OnMouseClickOrTapLoadLevel : MonoBehaviour
    {
        public string SceneName;
        public List<GameObject> BlockingGameObjects;

        void Update()
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0)) return;                       // don't allow if click / tap not active
            if (DialogManager.IsActive && DialogManager.Instance.Count > 0) return;     // don't allow if popup dialog shown.

            // check agains blocking UI? game objects
            if (BlockingGameObjects != null && EventSystem.current != null)
            {
                var pe = new PointerEventData(EventSystem.current)
                {
                    position = UnityEngine.Input.mousePosition
                };

                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);

                if (hits.Any(hit => BlockingGameObjects.Contains(hit.gameObject)))
                {
                    return;
                }
            }

            // if we got here then load the new scene
            GameManager.LoadSceneWithTransitions(SceneName);
        }
    }
}