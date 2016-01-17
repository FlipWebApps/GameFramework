//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Display.Other.Components
{
    /// <summary>
    /// Used for scrolling a material within the gameobjects renderer
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class ScrollMaterialComponent : MonoBehaviour
    {
        public int MaterialIndex;
        public Vector2 AnimationRate = new Vector2(1.0f, 0.0f);
        public string TextureName = "_MainTex";
        public Renderer Renderer;

        Vector2 _uvOffset = Vector2.zero;

        void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        void LateUpdate()
        {
            _uvOffset += AnimationRate * Time.deltaTime;
            if (Renderer.enabled)
            {
                Renderer.materials[MaterialIndex].SetTextureOffset(TextureName, _uvOffset);
            }
        }
    }
}