//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Other.Components
{
    /// <summary>
    /// Set the UV co-ordinates of a cube
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class SetQuadUvs : MonoBehaviour
    {
        public Rect Uvs;

        Mesh _theMesh;
        Vector2[] _theUVs;

        void Awake()
        {
            _theMesh = transform.GetComponent<MeshFilter>().mesh;
            _theUVs = new Vector2[_theMesh.uv.Length];
            _theUVs = _theMesh.uv;

            SetUVs(Uvs);
        }

        public void SetUVs(Rect uvs)
        {
            // bottom left, top right, bottom right, top left.
            Uvs = uvs;

            _theUVs[0] = new Vector2(uvs.x, uvs.y);
            _theUVs[1] = new Vector2(uvs.x + uvs.width, uvs.y + uvs.height);
            _theUVs[2] = new Vector2(uvs.x + uvs.width, uvs.y);
            _theUVs[3] = new Vector2(uvs.x, uvs.y + uvs.height);

            // assign the mesh its new UVs
            _theMesh.uv = _theUVs;
        }
    }
}