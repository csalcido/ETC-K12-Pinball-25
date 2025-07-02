Shader "Unlit/Grayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" } //for transparent objects
        Blend SrcAlpha OneMinusSrcAlpha //enables alpha blending
        ZWrite Off
        Cull Off  //renders both sides of face

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;  //processes each vertex of a mesh
            }

            fixed4 frag(v2f i) : SV_Target //calculates the color of each pixel
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float grey = dot(col.rgb, float3(0.299, 0.587, 0.114)); //calculates "brightness of color" according to our eyes
                return fixed4(grey, grey, grey, col.a); //changing all rgb values, keeping the alpha/brightness the same

            }
            ENDCG

            }
            
        }
        FallBack "Diffuse"
    }

