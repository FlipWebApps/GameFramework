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
using BeautifulTransitions.Scripts.Transitions;
#endif

using GameFramework.Display.Other;
using GameFramework.GameObjects;
using GameFramework.Localisation;
using GameFramework.Localisation.ObjectModel;
using GameFramework.UI.Other;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

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
            YesNo,
            Text
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
        /// A callback that can be used to validate any input before returning
        /// </summary>
        Func<DialogInstance, DialogResultType, int, bool> ValidationCallback;

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
        GameObject _textTemplateButton;


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

            // get any other references
            _textTemplateButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "TextButton", true);

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


        ///// <summary>
        ///// Show the dialog instance substituting in passed values and running any transitions.
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="text"></param>
        ///// <param name="text2"></param>
        ///// <param name="sprite"></param>
        ///// <param name="doneCallback"></param>
        ///// <param name="destroyOnClose"></param>
        ///// <param name="dialogButtons"></param>
        //public void Show(LocalisableText title, LocalisableText text, LocalisableText text2 = null, Sprite sprite = null,
        //    Action<DialogInstance> doneCallback = null, bool destroyOnClose = true,
        //    DialogButtonsType dialogButtons = DialogButtonsType.Custom)
        //{
        //    var tTitle = title == null ? null : title.GetValue();
        //    var tText = title == null ? null : text.GetValue();
        //    var tText2 = title == null ? null : text2.GetValue();
        //    Show(title: tTitle, text: tText, text2: tText2, sprite: sprite, doneCallback: doneCallback, destroyOnClose: destroyOnClose, dialogButtons: dialogButtons);
        //}

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
        /// <param name="buttonText"></param>
        public void Show(string title = null, string titleKey = null, string text = null, string textKey = null,
            string text2 = null, string text2Key = null, Sprite sprite = null,
            Action<DialogInstance> doneCallback = null, bool destroyOnClose = true,
            DialogButtonsType dialogButtons = DialogButtonsType.Custom, LocalisableText[] buttonText = null,
            Func<DialogInstance, DialogResultType, int, bool> validationCallback = null)
        {
            GameObject childGameObject;

            _dialogButtons = dialogButtons;
            ValidationCallback = validationCallback;
            DoneCallback = doneCallback;
            _destroyOnClose = destroyOnClose;

            // increase open count - not thread safe, but should be ok!
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have added a DialogManager component to your scene before showing a dialog!");
            DialogManager.Instance.Count++;
            IsShown = true;

            // default result
            DialogResult = DialogResultType.Ok;

            if (!string.IsNullOrEmpty(titleKey))
                title = GlobalLocalisation.GetText(titleKey);
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

            if (!string.IsNullOrEmpty(textKey))
                text = GlobalLocalisation.GetText(textKey);
            if (text != null)
                UIHelper.SetTextOnChildGameObject(gameObject, "ph_Text", text, true);
            else
            {
                childGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ph_Text", true);
                if (childGameObject != null)
                    childGameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(text2Key))
                text2 = GlobalLocalisation.GetText(text2Key);
            if (text2 != null)
                UIHelper.SetTextOnChildGameObject(gameObject, "ph_Text2", text2, true);
            else
            {
                childGameObject = GameObjectHelper.GetChildNamedGameObject(gameObject, "ph_Text2", true);
                if (childGameObject != null)
                    childGameObject.SetActive(false);
            }

            GameObject okButton, cancelButton;
            switch (_dialogButtons)
            {

                case DialogButtonsType.Ok:
                    okButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "OkButton", true);
                    if (okButton != null)
                    {
                        okButton.SetActive(true);
                    }
                    else
                    {
                        Assert.IsNotNull(_textTemplateButton, "If using Ok buttons, ensure the Dialog a GameObject named OkButton or a GameObject named TextButton that is a template for text buttons");
                        var button = CreateTextButton(LocalisableText.CreateLocalised("Button.Ok"));
                        button.GetComponent<Button>().onClick.AddListener(() => DoneOk());
                    }
                    break;
                case DialogButtonsType.OkCancel:
                    okButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "OkButton", true);
                    cancelButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "CancelButton", true);
                    if (okButton != null && cancelButton != null)
                    {
                        okButton.SetActive(true);
                        cancelButton.SetActive(true);
                    }
                    else
                    {
                        Assert.IsNotNull(_textTemplateButton, "If using OkCancel buttons, ensure the Dialog has GameObjects named OkButton and CancelButton or a GameObject named TextButton that is a template for text buttons");
                        var button = CreateTextButton(LocalisableText.CreateLocalised("Button.Ok"));
                        button.GetComponent<Button>().onClick.AddListener(() => DoneOk());
                        button = CreateTextButton(LocalisableText.CreateLocalised("Button.Cancel"));
                        button.GetComponent<Button>().onClick.AddListener(() => DoneCancel());
                    }
                    break;
                case DialogButtonsType.Cancel:
                    cancelButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "CancelButton", true);
                    if (cancelButton != null)
                    {
                        cancelButton.SetActive(true);
                    }
                    else
                    {
                        Assert.IsNotNull(_textTemplateButton, "If using a Cancel button, ensure the Dialog a GameObject named CancelButton or a GameObject named TextButton that is a template for text buttons");
                        var button = CreateTextButton(LocalisableText.CreateLocalised("Button.Cancel"));
                        button.GetComponent<Button>().onClick.AddListener(() => DoneCancel());
                    }
                    break;
                case DialogButtonsType.YesNo:
                    var yesButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "YesButton", true);
                    var noButton = GameObjectHelper.GetChildNamedGameObject(gameObject, "NoButton", true);
                    if (yesButton != null && noButton != null)
                    {
                        yesButton.SetActive(true);
                        noButton.SetActive(true);
                    }
                    else
                    {
                        Assert.IsNotNull(_textTemplateButton, "If using YesNo buttons, ensure the Dialog has GameObjects named YesButton and NoButton or a GameObject named TextButton that is a template for text buttons");
                        var button = CreateTextButton(LocalisableText.CreateLocalised("Button.Yes"));
                        button.GetComponent<Button>().onClick.AddListener(() => DoneYes());
                        button = CreateTextButton(LocalisableText.CreateLocalised("Button.No"));
                        button.GetComponent<Button>().onClick.AddListener(() => DoneNo());
                    }
                    break;
                case DialogButtonsType.Text:
                    Assert.IsNotNull(_textTemplateButton, "If using Text buttons, ensure the Dialog has a GameObject named TextButton that is a template for text buttons");
                    Assert.IsNotNull(buttonText, "If using Text buttons, ensure you pass a valid array of localisable texts into the show method.");
                    var counter = 0;
                    foreach (var localisableText in buttonText)
                    {
                        var button = CreateTextButton(localisableText);
                        var counter1 = counter;
                        button.GetComponent<Button>().onClick.AddListener(() => DoneCustom(counter1));
                        counter++;
                    }
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


        /// <summary>
        /// Create an instance from teh text template button
        /// </summary>
        /// <param name="localisableText"></param>
        /// <returns></returns>
        GameObject CreateTextButton(LocalisableText localisableText)
        {
            var button = Instantiate(_textTemplateButton);
            var textComponent = button.GetComponentInChildren<Text>(true);
            Assert.IsNotNull(textComponent, "If using Text buttons, ensure you the TextButton gameobject or one of it's children contains a Text component.");
            textComponent.text = localisableText.GetValue();
            button.transform.SetParent(_textTemplateButton.transform.parent);
            button.transform.localScale = Vector3.one;
            button.SetActive(true);
            return button;
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
            if (ValidationCallback == null || ValidationCallback(this, DialogResultType.Ok, -1))
            {
                DialogResult = DialogResultType.Ok;
                Done();
            }
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Cancel
        /// </summary>
        public void DoneCancel()
        {
            if (ValidationCallback == null || ValidationCallback(this, DialogResultType.Cancel, -1))
            {
                DialogResult = DialogResultType.Cancel;
                Done();
            }
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Yes
        /// </summary>
        public void DoneYes()
        {
            if (ValidationCallback == null || ValidationCallback(this, DialogResultType.Yes, -1))
            {
                DialogResult = DialogResultType.Yes;
                Done();
            }
        }


        /// <summary>
        /// Complete the dialog with DialogResult.No
        /// </summary>
        public void DoneNo()
        {
            if (ValidationCallback == null || ValidationCallback(this, DialogResultType.No, -1))
            {
                DialogResult = DialogResultType.No;
                Done();
            }
        }


        /// <summary>
        /// Complete the dialog with DialogResult.Custom and then specified custom return code.
        /// </summary>
        public void DoneCustom(int customReturnCode)
        {
            if (ValidationCallback == null || ValidationCallback(this, DialogResultType.Custom, customReturnCode))
            {
                DialogResult = DialogResultType.Custom;
                DialogResultCustom = customReturnCode;
                Done();
            }
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