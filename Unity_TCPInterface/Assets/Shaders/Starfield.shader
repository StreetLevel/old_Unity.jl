// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Starfield" {
    Properties {
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Pass {
            CGPROGRAM
                #pragma exclude_renderers flash
                #pragma vertex vert
                #pragma fragment frag
               
                struct appdata {
                    float4 pos : POSITION;
                };
               
                struct v2f {
                    float4 pos : SV_POSITION;
                    float size : PSIZE;
                };
   
                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.pos);
                    o.size = 15.0;
                    return o;
                }
   
                half4 frag(v2f i) : COLOR
                {
                    return half4(1,0,0,1);
                }
            ENDCG
        }
    }
}