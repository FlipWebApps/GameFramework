using FlipWebApps.GameFramework.Scripts.GameStructure.Levels;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.Debugging.Components
{
    /// <summary>
    /// This is a simple dummy game loop that will allow you to test the structure and interfaces in your game.
    /// 
    /// It gives an on screen meny where you can set whether the game is won or lost.
    /// 
    /// It doesn't make any sense to use this in an actual game!!
    /// </summary>
    public class DummyGameLoop : MonoBehaviour
    {
        public enum PlacementType { TopRight, TopLeft, BottomRight, BottomLeft}
        public PlacementType Placement = PlacementType.BottomRight;

        const int ButtonWidth = 75;
        const int ButtonHeight = 25;

        bool _isWon;
        bool _isLost;

        void Update ()
        {
            if (LevelManager.Instance.IsLevelRunning && (_isWon || _isLost))
            {
                Assert.IsTrue(LevelManager.IsActive, "Please ensure that you have a LevelManager component added to your scene.");
                Assert.IsTrue(GameOver.IsActive, "Please ensure that you have a GameOver component added to your scene, or are using one of the default GameOver prefabs.");

                LevelManager.Instance.LevelFinished();
                GameOver.Instance.Show(_isWon);
            }
        }


        void OnGUI()
        {
            float x = 0, y = 0;
            switch (Placement)
            {
                case PlacementType.TopRight:
                    x = Screen.width - ButtonWidth - 10;
                    y = ButtonHeight + 20;
                    break;
                case PlacementType.TopLeft:
                    x = 10;
                    y = ButtonHeight + 20;
                    break;
                case PlacementType.BottomRight:
                    x = Screen.width - ButtonWidth - 10;
                    y = Screen.height - ButtonHeight - 10;
                    break;
                case PlacementType.BottomLeft:
                    x = 10;
                    y = Screen.height - ButtonHeight - 10;
                    break;
            }

            if (GUI.Button(new Rect(x, y, ButtonWidth, ButtonHeight), "Win"))
            {
                _isWon = true;
            }
            if (GUI.Button(new Rect(x, y - ButtonHeight - 10, ButtonWidth, ButtonHeight), "Lose"))
            {
                _isLost = true;
            }
        }
    }
}
