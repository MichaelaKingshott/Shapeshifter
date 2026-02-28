Shader "Custom/NightVision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Brightness", Float) = 2
        _GreenTint ("Green Tint", Color) = (0.1,1,0.1,1)
    }

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Intensity;
            float4 _GreenTint;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Convert to grayscale
                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));

                // Increase brightness
                gray *= _Intensity;

                // Apply green tint
                return float4(gray * _GreenTint.rgb, 1);
            }
            ENDCG
        }
    }
}