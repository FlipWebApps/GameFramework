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

#if BEAUTIFUL_TRANSITIONS
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
#endif

using FlipWebApps.GameFramework.Scripts.Display.Other;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.Localisation;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components
{
    /// <summary>
    /// Represents an instance of a dialog. Allows for animation and managing feedback state.
    /// 
    /// Dialog Instance should always be on the root of a dialog and be accompanied by an Animator. It should have a child gameobject called Dialog.
    /// Check teh existing prefabs for an example of how this should be setup.
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Dialogs/DialogInstance")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class DialogInstance : MonoBehaviour
    {
        public bool ShowOnStart;

        public System.Action<DialogInstance> DoneCallback;
        bool _destroyOnClose;

        public GameObject DialogGameObject { get; set; }
        public GameObject Content { get; set; }
        public Animator ContentAnimator { get; set; }
        public GameObject CustomContentItem { get; set; }
        public bool IsShown { get; set; }

        public enum DialogResultType
        {
            Ok,
            Cancel,
            Yes,
            No,
            Custom
        }

        public DialogResultType DialogResult { get; set; }
        public int DialogResultCustom { get; set; }

        public enum DialogButtonsType
        {
            Custom,
            Ok,
            OkCancel,
            Cancel,
            YesNo
        }

        public DialogButtonsType DialogButtons { get; set; }

        DialogInstance _swapToDialogInstance;

        void Awake()
        {
            DialogGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "Dialog", true);
            Assert.IsNotNull(DialogGameObject, "A DialogInstance component must always have a direct child called 'Dialog'");
            Assert.IsNotNull(GetComponent<Animator>(), "A DialogInstance component must always have an attached Animator");

            Content = GameObjectHelper.GetChildNamedGameObject(gameObject, "Content", true);
            if (Content != null)
                ContentAnimator = Content.GetComponent<Animator>();
            IsShown = DialogGameObject.activeSelf;
        }

        void Start()
        {
            if (ShowOnStart)
            {
                Show(destroyOnClose: false);
            }
        }

        public void Show(string title = null, string titleKey = null, string text = null, string textKey = null,
            string text2 = null, string text2Key = null, Sprite sprite = null,
            System.Action<DialogInstance> doneCallback = null, bool destroyOnClose = true,
            DialogButtonsType dialogButtons = DialogButtonsType.Custom)
        {
            GameObject childGameObject;

            DialogButtons = dialogButtons;
            DoneCallback = doneCallback;
            _destroyOnClose = destroyOnClose;

            // increase open count - not thread safe, but should be ok!
            DialogManager.Instance.Count++;
            IsShown = true;

            // default result
            DialogResult = DialogResultType.Ok;

            if (titleKey != null)
                title = LocaliseText.Get(titleKey);
            if (title != null)
                UIHelper.SetTextOnChildGameObject(gameObject, "ph_Title", title, true);

            if (sprite != null)
                UIHelper.SetSpriteOnChildGameObject(gameObject, "ph_Image", sprite, true);
            else
            {
                childGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ph_Image", true);
                if (childGameObject != null)
                    childGameObject.SetActive(false);
            }

            if (textKey != null)
                text = LocaliseText.Get(textKey);
            if (text != null)
                UIHelper.SetTextOnChildGameObject(gameObject, "ph_Text", text, true);
            else
            {
                childGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ph_Text", true);
                if (childGameObject != null)
                    childGameObject.SetActive(false);
            }

            if (text2Key != null)
                text2 = LocaliseText.Get(text2Key);
            if (text2 != null)
                UIHelper.SetTextOnChildGameObject(gameObject, "ph_Text2", text2, true);
            else
            {
                childGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ph_Text2", true);
                if (childGameObject != null)
                    childGameObject.SetActive(false);
            }

            switch (DialogButtons)
            {
                case DialogButtonsType.Ok:
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "OkButton", true).SetActive(true);
                    break;
                case DialogButtonsType.OkCancel:
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "OkButton", true).SetActive(true);
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "CancelButton", true).SetActive(true);
                    break;
                case DialogButtonsType.Cancel:
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "CancelButton", true).SetActive(true);
                    break;
                case DialogButtonsType.YesNo:
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "YesButton", true).SetActive(true);
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "NoButton", true).SetActive(true);
                    break;
            }

            // show / transition in and when done call coroutine
            float transitionTime = 0;
            DialogGameObject.SetActive(true);
#if BEAUTIFUL_TRANSITIONS
            //if (TransitionHelper.ContainsTransition(gameObject))
            //{
                transitionTime = TransitionHelper.GetTransitionInTime(TransitionHelper.TransitionIn(gameObject));
            //}
#endif
            StartCoroutine(CoRoutines.DelayedCallback(transitionTime, ShowFinished));
        }
            
        public void ShowFinished()
        {
        }

        public void SwapTo(DialogInstance dialogInstance)
        {
            _swapToDialogInstance = dialogInstance;
            Done();
        }

        public void DoneOk()
        {
            DialogResult = DialogResultType.Ok;
            Done();
        }

        public void DoneCancel()
        {
            DialogResult = DialogResultType.Cancel;
            Done();
        }

        public void DoneYes()
        {
            DialogResult = DialogResultType.Yes;
            Done();
        }

        public void DoneNo()
        {
            DialogResult = DialogResultType.No;
            Done();
        }

        public void DoneCustom(int customReturnCode)
        {
            DialogResult = DialogResultType.Custom;
            DialogResultCustom = customReturnCode;
            Done();
        }

        public void Done()
        {
            // show / transition in and when done call coroutine
            float transitionTime = 0;
#if BEAUTIFUL_TRANSITIONS
            //if (TransitionHelper.ContainsTransition(gameObject))
            //{
                transitionTime = TransitionHelper.GetTransitionOutTime(TransitionHelper.TransitionOut(gameObject));
            //}
#endif
            StartCoroutine(CoRoutines.DelayedCallback(transitionTime, DoneFinished));
        }

        public void DoneFinished()
        {
            if (DoneCallback != null)
                DoneCallback(this);

            DialogGameObject.SetActive(false);

            if (_destroyOnClose)
                Destroy(gameObject);

            // decrease open count - not thread safe, but should be ok!
            DialogManager.Instance.Count--;
            IsShown = false;

            if (_swapToDialogInstance != null)
            {
                _swapToDialogInstance.Show(destroyOnClose: false);
                _swapToDialogInstance = null;
            }
        }
    }
}