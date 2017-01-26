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


using System;
#if BEAUTIFUL_TRANSITIONS
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
#endif

using GameFramework.Display.Other;
using GameFramework.GameObjects;
using GameFramework.Localisation;
using GameFramework.UI.Other;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.UI.Dialogs.Components
{
    /// <summary>
    /// Represents an instance of a dialog. Allows for animation and managing feedback state.
    /// </summary>
    /// Dialog Instance should always be on the root of a dialog and be accompanied by an Animator. It should have a child gameobject called Dialog.
    /// Check teh existing prefabs for an example of how this should be setup.
    [AddComponentMenu("Game Framework/UI/Dialogs/DialogInstance")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/dialogs/")]
    public class DialogInstance : MonoBehaviour
    {
        #region enums

        /// <summary>
        /// Results that can be returned from the dialog.
        /// </summary>
        public enum DialogResultType
        {
            Ok,
            Cancel,
            Yes,
            No,
            Custom
        }

        /// <summary>
        /// The dialog buttons that should be shown
        /// </summary>
        public enum DialogButtonsType
        {
            Custom,
            Ok,
            OkCancel,
            Cancel,
            YesNo
        }

        #endregion enums

        #region editor fields

        /// <summary>
        /// Whether to show the dialog on start
        /// </summary>
        [Tooltip("Whether to show the dialog on start")]
        public bool ShowOnStart;

        /// <summary>
        /// The target / gameobject root for the dialog. If not specified then will use a child gameobject assuming there is only a single child gameobject.
        /// </summary>
        /// See Level for more information.
        public GameObject Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }
        [Tooltip("The target / gameobject root for the dialog. If not specified then will use a child gameobject assuming there is only a single child gameobject.")]
        [SerializeField]
        GameObject _target;

        /// <summary>
        /// An optional gameobject underwhich the dialog content is located. Used for inserting custom content. If not specified then will use a child gameobject named 'Content' if found.
        /// </summary>
        /// See Level for more information.
        public GameObject Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }
        [Tooltip("An optional gameobject underwhich the dialog content is located. Used for inserting custom content. If not specified then will use a child gameobject named 'Content' if found.")]
        [Header("Advanced")]
        [SerializeField]
        GameObject _content;

        #endregion editor fields


        [Obsolete("Use Target instead.")]
        public GameObject DialogGameObject
        {
            get { return Target; }
            set { Target = value; }
        }

        /// <summary>
        /// An animator used for animating the content
        /// </summary>
        public Animator ContentAnimator { get; set; }

        /// <summary>
        /// Instanciated instance of any passed custom content prefab
        /// </summary>
        public GameObject CustomContentItem { get; set; }

        /// <summary>
        /// Whether the dialog is currently shown
        /// </summary>
        public bool IsShown { get; set; }


        /// <summary>
        /// A callback that will be triggered when the dialog completes
        /// </summary>
        public Action<DialogInstance> DoneCallback;

        /// <summary>
        /// The dialog result
        /// </summary>
        public DialogResultType DialogResult { get; set; }

        /// <summary>
        /// A custom result value for your own use.
        /// </summary>
        public int DialogResultCustom { get; set; }


        DialogButtonsType _dialogButtons;
        DialogInstance _swapToDialogInstance;
        bool _destroyOnClose;


        void Awake()
        {
            // if target is not specified then use a child gameobject
            if (Target == null)
                Target = gameObject.transform.childCount == 1 ? 
                    gameObject.transform.GetChild(0).gameObject : 
                    GameObjectHelper.GetChildNamedGameObject(gameObject, "Dialog", true);
            Assert.IsNotNull(Target, "If there are multiple children gameobjects to DialogInstance then you must specify Target.");
            Assert.AreNotEqual(gameObject, Target, "The DialogInstance Target should not be the same as the gameobject to which DialogInstance is added. Usually Target will be the immediate child of the gameobject with DialogInstance.");

            // set content from child Content gameobject if not specified.
            if (Content == null)
                Content = GameObjectHelper.GetChildNamedGameObject(Target, "Content", true);
            if (Content != null)
                ContentAnimator = Content.GetComponent<Animator>();

            IsShown = Target.activeSelf;
        }


        /// <summary>
        /// Show immediately if show on start is specified.
        /// </summary>
        void Start()
        {
            if (ShowOnStart)
            {
                Show(destroyOnClose: false);
            }
        }


        /// <summary>
        /// Show the dialog instance substituting in passed values and running any transitions.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="titleKey"></param>
        /// <param name="text"></param>
        /// <param name="textKey"></param>
        /// <param name="text2"></param>
        /// <param name="text2Key"></param>
        /// <param name="sprite"></param>
        /// <param name="doneCallback"></param>
        /// <param name="destroyOnClose"></param>
        /// <param name="dialogButtons"></param>
        public void Show(string title = null, string titleKey = null, string text = null, string textKey = null,
            string text2 = null, string text2Key = null, Sprite sprite = null,
            Action<DialogInstance> doneCallback = null, bool destroyOnClose = true,
            DialogButtonsType dialogButtons = DialogButtonsType.Custom)
        {
            GameObject childGameObject;

            _dialogButtons = dialogButtons;
            DoneCallback = doneCallback;
            _destroyOnClose = destroyOnClose;

            // increase open count - not thread safe, but should be ok!
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
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

            switch (_dialogButtons)
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
            Target.SetActive(true);
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


        /// <summary>
        /// Complete the current dialog and swap to the specified one, running any transitions (requires Beautiful Transitions)
        /// </summary>
        /// <param name="dialogInstance"></param>
        public void SwapTo(DialogInstance dialogInstance)
        {
            _swapToDialogInstance = dialogInstance;
            Done();
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Ok
        /// </summary>
        public void DoneOk()
        {
            DialogResult = DialogResultType.Ok;
            Done();
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Cancel
        /// </summary>
        public void DoneCancel()
        {
            DialogResult = DialogResultType.Cancel;
            Done();
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Yes
        /// </summary>
        public void DoneYes()
        {
            DialogResult = DialogResultType.Yes;
            Done();
        }


        /// <summary>
        /// Complete the dialog with DialogResult.No
        /// </summary>
        public void DoneNo()
        {
            DialogResult = DialogResultType.No;
            Done();
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Custom and then specified custom return code.
        /// </summary>
        public void DoneCustom(int customReturnCode)
        {
            DialogResult = DialogResultType.Custom;
            DialogResultCustom = customReturnCode;
            Done();
        }


        /// <summary>
        /// Complete the dialog
        /// </summary>

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


        /// <summary>
        /// Callback when the dialog is ready to close
        /// </summary>
        public void DoneFinished()
        {
            if (DoneCallback != null)
                DoneCallback(this);

            Target.SetActive(false);

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