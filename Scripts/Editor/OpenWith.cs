//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Editor {
    /// <summary>
    /// Adds commands for opening in certain applications.
    /// See https://unity3d.com/learn/tutorials/modules/intermediate/editor/menu-items
    /// </summary>

    public class OpenWith : MonoBehaviour {

        [MenuItem("Assets/Open With.../Notepad++", false, -8002)]
        static void OpenWithNotepadPlusPlus()
        {
            var selected = Selection.activeObject;
            //TODO find pat\
            Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", "\"" + AssetDatabase.GetAssetPath(selected) + "\"");
        }

        [MenuItem("Assets/Open With.../Excel", false, -8002)]
        static void OpenWithExcel()
        {
            var selected = Selection.activeObject;
            //TODO find path
            Process.Start(@"C:\Program Files\Microsoft Office\root\Office16\Excel.exe", "\"" + AssetDatabase.GetAssetPath(selected) + "\"");
        }

        /// <summary>
        /// 
        /// Note that we pass the same path, and also pass "true" to the second argument.
        /// </summary>
        /// <returns></returns>
        [MenuItem("Assets/Open With.../Notepad++", true, -8002)]
        static bool OpenWithNotepadPlusPlusValidation()
        { 
            // This returns true when the selected object is of the given type (the menu item will be disabled otherwise).
            return true; // Selection.activeObject.GetType() == typeof(MonoScript);
        }

        /// <summary>
        /// 
        /// Note that we pass the same path, and also pass "true" to the second argument.
        /// </summary>
        /// <returns></returns>
        [MenuItem("Assets/Open With.../Excel", true, -8002)]
        static bool OpenWithExcelValidation()
        {
            // This returns true when the selected object is of the given type (the menu item will be disabled otherwise).
            return true; // Selection.activeObject.GetType() == typeof(MonoScript);
        }
    }
}