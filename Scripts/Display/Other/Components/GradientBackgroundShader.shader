// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GradientBackgroundShader"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
                struct vertexInput
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                };
               
                struct v2f {
                    float4 pos : SV_POSITION;
                    fixed4 color : COLOR;
                };
           
           
                v2f vert(vertexInput v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.color = v.color;
                    return o;
                }
           
                sampler2D _MainTex;
           
                float4 frag(v2f IN) : COLOR
                {
                    return IN.color;
                }
            ENDCG
        }
    }
}