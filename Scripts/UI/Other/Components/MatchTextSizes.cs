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

using GameFramework.GameObjects.Components.AbstractClasses;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI.Other.Components {
    /// <summary>
    /// Size all the Text components so that they are the same.
    /// NOTE: MAY NOT WORK!
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Other/MatchTextSizes")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class MatchTextSizes : RunOnState
    {
        public Text[] TextComponents;

        [Header("Text Best Fit Sizes")]
        public int MinSize;
        public int MaxSize;

        public override void RunMethod()
        {
            RecalculateTextSizes();
        }

        public void RecalculateTextSizes()
        {
            var minimumSize = int.MaxValue;
            var textGenerator = new TextGenerator();
            foreach (var text in TextComponents)
            {
                //get current text generation settings
                var textGenerationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);

                //update to best fit settings so we can best fit size
                textGenerationSettings.resizeTextForBestFit = true;
                textGenerationSettings.resizeTextMinSize = MinSize;
                textGenerationSettings.resizeTextMaxSize = MaxSize;

                //populate to calculate the new size
                textGenerator.Populate(text.text, textGenerationSettings);

                //set new bestFit if new calculation is smaller than current bestFit
                minimumSize = textGenerator.fontSizeUsedForBestFit < minimumSize
                    ? textGenerator.fontSizeUsedForBestFit
                    : minimumSize;
            }

            // update all text components with new calculated size
            foreach (var text in TextComponents)
            {
                text.fontSize = minimumSize*(int) text.canvas.scaleFactor;
            }
        }
    }
}