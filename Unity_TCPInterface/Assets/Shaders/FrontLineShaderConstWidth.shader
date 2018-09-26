// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FrontLine/FrontLineShaderConstWidth" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _LineWidth ("Line Width", Range(0.01, 100)) = 1.0
        _LightSaberFactor ("LightSaberFactor", Range(0.0, 1.0)) = 0.9
    }
    SubShader {
        Tags { "RenderType"="Geometry" "Queue" = "Transparent" }
        LOD 200
 
        Pass {
 
            Cull Off
            ZWrite Off
            //ZTest LEqual
            Blend One One
            Lighting Off            
 
            CGPROGRAM
            #pragma exclude_renderers d3d11
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #pragma debug
            #pragma glsl_no_auto_normalization
 
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            float _LineWidth;
            float _LightSaberFactor;
 
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 color : COLOR;
            };
 
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
 
            v2f vert (a2v v)
            {
                v2f o;
                o.uv = v.texcoord.xy;
                float4 vMVP = UnityObjectToClipPos(v.vertex);          
                float4 otherMVP = UnityObjectToClipPos(float4(v.vertex + v.normal,1.0));  
                float2 lineDirProj = _LineWidth *  normalize((vMVP.xy/vMVP.w) - (otherMVP.xy/otherMVP.w));
                float4 vMV = mul(UNITY_MATRIX_MV, v.vertex);
 
                vMV.x = vMV.x + lineDirProj.x * v.texcoord1.x + lineDirProj.y * v.texcoord1.y;
                vMV.y = vMV.y + lineDirProj.y * v.texcoord1.x - lineDirProj.x * v.texcoord1.y;
 
                o.pos = mul(UNITY_MATRIX_P, vMV);
                o.color = v.color;
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {
                float4 tx = tex2D (_MainTex, i.uv);
                               
                if (tx.a > _LightSaberFactor)
                {
                    return float4(1.0, 1.0, 1.0, tx.a);
                }
                else
                {
                    return tx * i.color; //_Color;
                }
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}