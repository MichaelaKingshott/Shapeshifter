Shader "Custom/UnderwaterTint"
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (0.0, 0.4, 0.7, 1)
        _Strength ("Strength", Range(0,1)) = 0.35
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _TintColor;
            float _Strength;

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                col.rgb = lerp(col.rgb, col.rgb + _TintColor.rgb, _Strength);

                return col;
            }

            ENDCG
        }
    }
}