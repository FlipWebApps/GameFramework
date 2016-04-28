using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.UI.Other.Components
{
    /// <summary>
    /// code changed from Unity3D gradient effect for Unity3D 5.2
    /// REF : http://pastebin.com/dJabCfWn
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Other/GradientText")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class GradientText : BaseMeshEffect
    {
        [SerializeField] Color32 _topColor = Color.white;
        [SerializeField] Color32 _bottomColor = Color.black;

        public override void ModifyMesh(Mesh mesh)
        {
            if (!IsActive()) { return; }
            if (mesh == null || mesh.vertexCount <= 0) return;

            int count = mesh.vertexCount;

            float bottomY = mesh.vertices[0].y;
            float topY = mesh.vertices[0].y;

            for (int i = 1; i < count; i++)
            {
                float y = mesh.vertices[i].y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
            }

            float uiElementHeight = topY - bottomY;

            List<Color32> newClr = new List<Color32>();
            for (int i = 0; i < count; i++)
            {
                newClr.Add(Color32.Lerp(_bottomColor, _topColor, (mesh.vertices[i].y - bottomY) / uiElementHeight));
            }
            mesh.SetColors(newClr);
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            //throw new NotImplementedException();
        }
    }
}