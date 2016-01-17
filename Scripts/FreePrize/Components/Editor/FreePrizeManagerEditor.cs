//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEditor;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.FreePrize.Components.Editor
{
    [CustomEditor(typeof(FreePrizeManager))]
    public class FreePrizeManagerEditor : UnityEditor.Editor {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            DrawDefaultInspector();

            GUILayout.Label("Test Functions", EditorStyles.boldLabel);
            if (GUILayout.Button("Make Prize Available"))
            {
                FreePrizeManager.Instance.MakePrizeAvailable();
            }
            if (GUILayout.Button("Reset Counter"))
            {
                FreePrizeManager.Instance.StartNewCountdown();
            }
        }
    }
}
