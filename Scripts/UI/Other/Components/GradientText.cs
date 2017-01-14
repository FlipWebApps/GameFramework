using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI.Other.Components
{
    /// <summary>
    /// Provides a gradient effect for UI elements
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Other/GradientText")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class GradientText : BaseMeshEffect
    {
        /// <summary>
        /// The color to use for the top of the text gradient.
        /// </summary>
        public Color32 TopColor {
            get { return _topColor; }
            set { _topColor = value; }
        }
        [Tooltip("The color to use for the top of the text color gradient.")]
        [SerializeField]
        Color32 _topColor = Color.white;

        /// <summary>
        /// The color to use for the bottom of the text gradient.
        /// </summary>
        public Color32 BottomColor
        {
            get { return _bottomColor; }
            set { _bottomColor = value; }
        }
        [Tooltip("The color to use for the bottom of the text color gradient.")]
        [SerializeField]
        Color32 _bottomColor = Color.black;


        /// <summary>
        /// Modify the mesh colors to give a gradient affect.
        /// </summary>
        /// <param name="mesh"></param>
        public override void ModifyMesh(Mesh mesh)
        {
            if (!IsActive()) { return; }
            if (mesh == null || mesh.vertexCount <= 0) return;

            var vertexCount = mesh.vertexCount;

            // get bottom / top y positions.
            var bottomY = mesh.vertices[0].y;
            var topY = mesh.vertices[0].y;
            for (var i = 1; i < vertexCount; i++)
            {
                var y = mesh.vertices[i].y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
            }

            // height
            var height = topY - bottomY;

            var colours = new List<Color32>();
            for (var i = 0; i < vertexCount; i++)
            {
                colours.Add(Color32.Lerp(_bottomColor, _topColor, (mesh.vertices[i].y - bottomY) / height));
            }
            mesh.SetColors(colours);
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            //throw new NotImplementedException();
        }
    }
}