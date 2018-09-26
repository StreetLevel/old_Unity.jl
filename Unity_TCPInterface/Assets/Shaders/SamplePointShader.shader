// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SamplePointShader" {
    Properties {
        _MainTex ("Texture Image", 2D) = "" {}
        _SpriteSize("SpriteSize", Float) = 10
    }
    SubShader {
        Pass{
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"       
       
        sampler2D _MainTex;
        fixed4 _TintColor;
        float _SpriteSize;
       
        struct vertexInput {
            float4 pos : POSITION;
            fixed4 col : COLOR;
            float2 UV : TEXCOORD0;
        };
 
        struct fragmentInput {
            float4 pos : SV_POSITION;
            float size : PSIZE;
            fixed4 col : COLOR;
            float2 UV: TEXCOORD0;
        };
       
             
             
        fragmentInput vert(vertexInput input) {
           
            fragmentInput output = (fragmentInput)0;
           
            output.pos = UnityObjectToClipPos(input.pos);
            output.col = input.col;
            output.size = _SpriteSize;
            output.UV = input.UV;
            return output;         
        }
       
        float4 frag(fragmentInput input) : COLOR
        {          
            return tex2D(_MainTex, float2(input.UV.x / _SpriteSize,input.UV.y / _SpriteSize));
            }
        ENDCG
    }
   
    }
}