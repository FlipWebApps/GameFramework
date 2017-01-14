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

using GameFramework.Debugging;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI.Other.Components
{
    /// <summary>
    /// NOTE: Is this of any use?
    /// NOTE: MAY NOT WORK!
    /// </summary>
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Game Framework/UI/Other/ScaleTextBasedOnScreenSize")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class ScaleTextBasedOnScreenSize : MonoBehaviour
    {
        public enum RunType { OnStart, OnEnable, OnUpdate };

        public RunType Run;
        public int MinSize, MaxSize;
        public float ReferenceSizeInInches;

        //public float ReferenceDpi, ReferenceScreenHeight;
        public float ScreenDpiEditForTesting;

        Text _text;
        float _startTextSize;

        void Awake()
        {
            _text = GetComponent<Text>();
            _startTextSize = _text.fontSize;

            ScreenDpiEditForTesting = Screen.dpi;
            if (Mathf.Approximately(ScreenDpiEditForTesting, 0))
                Debug.LogWarning("Screen.dpi is 0. Not performing text scaling");
//#if UNITY_EDITOR
//            if (ScreenDpi != ReferenceDpi)
//                Debug.LogWarning("Screen.dpi is not the same as reference dpi. this may give distorted results");
//#endif
        }

        void Start()
        {

            if (Run == RunType.OnStart)
                SetTextSize();
        }

        void OnEnable()
        {
            if (Run == RunType.OnEnable)
                SetTextSize();
        }

        void Update()
        {
            if (Run == RunType.OnUpdate)
                SetTextSize();
        }

        void SetTextSize()
        {
            if (!Mathf.Approximately(Screen.dpi, 0))
            {
                // we assume that the canvas scaler already scales the text up based upon screen size and considering dpi
                // therefor we just adjust by the physical size, so if the reference height is 5" and the new size is 10" then we half the font size.
                float screenHeightInches = Screen.height / ScreenDpiEditForTesting;
                MyDebug.Log("Estimated screen height in inches is : " + screenHeightInches);

                float scaleFactor = ReferenceSizeInInches / screenHeightInches;
                _text.fontSize = Mathf.CeilToInt(Mathf.Clamp(_startTextSize * scaleFactor, MinSize, MaxSize));

                // OLD CODE WHERE WE TRIED WORKING IN DPI etc... Not sure 100% how this should all work best!!!
                //// adjust font size by height scale and then adjust by dpi scale 
                //int size = (int)(ReferenceTextSize * (ReferenceScreenHeight / Screen.height) * (ScreenDpi / ReferenceDpi));
                //Debug.LogError("size" + ReferenceScreenHeight);
                //Debug.LogError("size" + Screen.height);
                //Debug.LogError("size" + ReferenceScreenHeight / Screen.height);
                //Debug.LogError("size" + ScreenDpi / ReferenceDpi);
                //Debug.LogError("size" + size);
                //Text.fontSize = Mathf.Clamp(size, MinSize, MaxSize);
            }
        }
    }
}