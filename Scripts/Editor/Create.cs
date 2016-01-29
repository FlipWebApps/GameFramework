//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FlipWebApps.GameFramework.Scripts.Editor {
    /// <summary>
    /// Adds commands for opening in certain applications.
    /// See https://unity3d.com/learn/tutorials/modules/intermediate/editor/menu-items
    /// </summary>

    public class Create : MonoBehaviour {

        [MenuItem("Assets/Create/Localisation File", false, -8002)]
        static void CreateLocalisationFile()
        {
            var selected = Selection.activeObject;

            var filename = Path.Combine(AssetDatabase.GetAssetPath(selected), "Localisation.csv");
            if (!File.Exists(filename))
            {
                string[] lines = { "KEY,English" };
                File.WriteAllLines(filename, lines);
                AssetDatabase.Refresh();
            }
            else
                Debug.LogWarning(filename + " already exists.");
        }

    }
}