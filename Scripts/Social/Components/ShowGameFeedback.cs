//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Social.Components
{
    /// <summary>
    /// Functions for showing the game feedback dialog
    /// </summary>
    public class ShowGameFeedback : MonoBehaviour
    {
        /// <summary>
        /// If you are unsure whether they like your game then call this method to get additional confirmation 
        /// before getting a rating.
        /// </summary>
        public void GameFeedbackUnsureIfTheyLike()
        {
            GameFeedback gameFeedback = new GameFeedback();
            gameFeedback.GameFeedbackUnsureIfTheyLike();
        }

        /// <summary>
        /// If you assume that they like the game (e.g. after a given number of plays) then call this method. This should be
        /// invoked as a direct result of a user action to rate (call GameFeedbackAssumeTheyLikeOptional otherwise).
        /// </summary>
        public void GameFeedbackAssumeTheyLike()
        {
            GameFeedback gameFeedback = new GameFeedback();
            gameFeedback.GameFeedbackAssumeTheyLike();
        }
    }
}