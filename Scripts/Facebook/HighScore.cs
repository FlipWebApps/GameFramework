//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

namespace FlipWebApps.GameFramework.Scripts.Facebook
{
    /// <summary>
    /// DO NOT USE
    /// </summary>
    public class HighScore
    {
        public string UserId;
        public string UserName;

        //public string AppId;
        //public string AppName;

        public int Score;

        public HighScore()
        {
        }

        public HighScore(string userId, string userName, int score)
        {
            UserId = userId;
            UserName = userName;
            Score = score;
        }

        public override string ToString()
        {
            return "HighScore: " + UserId + ", " + UserName + ": " + Score;
        }
    }
}