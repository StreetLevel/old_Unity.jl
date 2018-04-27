Shader "Custom/Circle"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        ZWrite Off
        Cull Off
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "UnityCG.cginc"
 
            // Quality level
            // 2 == high quality
            // 1 == medium quality
            // 0 == low quality
            #define QUALITY_LEVEL 1
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
         
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord - 0.5;
                return o;
            }
 
            fixed4 _Color;
 
            fixed4 frag (v2f i) : SV_Target
            {
                float dist = length(i.uv);
 
            #if QUALITY_LEVEL == 2
                // length derivative, 1.5 pixel smoothstep edge
                float pwidth = length(float2(ddx(dist), ddy(dist)));
                float alpha = smoothstep(0.5, 0.5 - pwidth * 1.5, dist);
            #elif QUALITY_LEVEL == 1
                // fwidth, 1.5 pixel smoothstep edge
                float pwidth = fwidth(dist);
                float alpha = smoothstep(0.5, 0.5 - pwidth * 1.5, dist);
            #else // Low
                // fwidth, 1 pixel linear edge
                float pwidth = fwidth(dist);
                float alpha = saturate((0.5 - dist) / pwidth);
            #endif
 
                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}