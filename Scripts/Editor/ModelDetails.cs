//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using DB = UnityEngine.Debug;

namespace FlipWebApps.GameFramework.Scripts.Editor {
    /// <summary>
    /// Various helper options for models
    /// </summary>
    public class ModelDetails : MonoBehaviour {
        /// <summary>
        /// Return some information about the selected model
        /// </summary>
        static void GetModelDetails()
        {
            var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);

            int numTriangles = 0;
            int numVertices = 0;

            foreach (var obj in objs)
            {
                var go = obj as GameObject;

                if (go == null) continue;

                var meshes = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach (var mesh in meshes)
                {
                    numVertices += mesh.sharedMesh.vertexCount;
                    numTriangles += mesh.sharedMesh.triangles.Length / 3;
                }
            }

            DB.Log(string.Format("Found {0} vertices and {1} triangles", numVertices, numTriangles));
        }

        #region Menu items
        [MenuItem("Window/Flip Web Apps/3D/Model Details")]
        static void ModelDetails0()
        {
            GetModelDetails();
        }

        #endregion
    }
}