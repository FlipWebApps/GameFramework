//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components {
    /// <summary>
    /// Size all the Text components so that they are the same.
    /// </summary>
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