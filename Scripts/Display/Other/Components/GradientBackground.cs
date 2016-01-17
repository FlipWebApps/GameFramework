//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Other.Components
{
    /// <summary>
    /// Description
    /// Creates a background for the camera, which is a simple gradient blend between two colors.
    /// Usage
    /// Attach this script to your camera, and change the top and bottom colors in the inspector as desired. When run, the clear flags for your camera is set to Depth Only, allowing a newly created background camera to show through. A plane with the gradient colors is created, which only the background camera can see. This is done with GradientLayer, which is the only layer that the background camera sees, and your camera is set to ignore. The default, 7, is a built-in (non-user-editable) layer that's not used for anything as of Unity 3.1. It can be changed as necessary.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class GradientBackground : MonoBehaviour
    {

        public Color TopColor = Color.blue;
        public Color BottomColor = Color.white;
        public Shader Shader;
        public int GradientLayer = 7;

        void Awake()
        {
            var attachedCamera = gameObject.GetComponent<Camera>();
            GradientLayer = Mathf.Clamp(GradientLayer, 0, 31);
            if (!attachedCamera)
            {
                Debug.LogError("Must attach GradientBackground script to the camera");
                return;
            }

            attachedCamera.clearFlags = CameraClearFlags.Depth;
            attachedCamera.cullingMask = attachedCamera.cullingMask & ~(1 << GradientLayer);
            var gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
            gradientCam.depth = attachedCamera.depth - 1;
            gradientCam.cullingMask = 1 << GradientLayer;

            var mesh = new Mesh
            {
                vertices =
                    new[]
                    {
                        new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, -.577f, 1f),
                        new Vector3(100f, -.577f, 1f)
                    },
                colors = new[] {TopColor, TopColor, BottomColor, BottomColor},
                triangles = new[] {0, 1, 2, 1, 3, 2}
            };



            var mat = new Material(Shader);

            var gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

            ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
            var gradRend = gradientPlane.GetComponent<Renderer>();
            gradRend.material = mat;
            gradientPlane.layer = GradientLayer;
        }
    }

    //}


    //using UnityEngine;

    ///// <summary>
    ///// Description
    ///// Creates a background for the camera, which is a simple gradient blend between two colors.
    ///// Usage
    ///// Attach this script to your camera, and change the top and bottom colors in the inspector as desired. When run, the clear flags for your camera is set to Depth Only, allowing a newly created background camera to show through. A plane with the gradient colors is created, which only the background camera can see. This is done with GradientLayer, which is the only layer that the background camera sees, and your camera is set to ignore. The default, 7, is a built-in (non-user-editable) layer that's not used for anything as of Unity 3.1. It can be changed as necessary.
    ///// </summary>
    //public class GradientBackground : MonoBehaviour
    //{
    //    public Camera camera;
    //    public Gradient gradient;

    //    public Color topColor = Color.blue;
    //    public Color bottomColor = Color.white;
    //    public int GradientLayer = 7;
    //    public float bottomPosition = -.577f;
    //    void Awake()
    //    {
    //        GradientLayer = Mathf.Clamp(GradientLayer, 0, 31);
    //        if (!GetComponent<Camera>())
    //        {
    //            Debug.LogError("Must attach GradientBackground script to the camera");
    //            return;
    //        }

    //        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
    //        GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << GradientLayer);
    //        Camera gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
    //        gradientCam.depth = GetComponent<Camera>().depth - 1;
    //        gradientCam.cullingMask = 1 << GradientLayer;

    //        Mesh mesh = new Mesh();
    //        mesh.vertices = new Vector3[4] { new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, bottomPosition, 1f), new Vector3(100f, bottomPosition, 1f) };

    //        mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };

    //        mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

    //        Material mat = new Material("Shader \"Vertex Color Only\"{Subshader{BindChannels{Bind \"vertex\", vertex Bind \"color\", color}Pass{}}}");
    //        GameObject gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

    //        ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
    //        gradientPlane.GetComponent<Renderer>().material = mat;
    //        gradientPlane.layer = GradientLayer;
    //    }

    //}
}