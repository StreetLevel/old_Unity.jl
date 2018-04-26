// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ExampleVertexColorShader" {
    Properties {
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque"}       
        pass
        {
            CGPROGRAM
            #pragma vertex wfiVertCol
            #pragma fragment passThrough
            #include "UnityCG.cginc"

         struct VertOut
         {
             float4 position : POSITION;
             float4 color : COLOR;
         };
         struct VertIn
         {
             float4 vertex : POSITION;
             float4 color : COLOR;
         };
         VertOut wfiVertCol(VertIn input, float3 normal : NORMAL)
         {
             VertOut output;
             output.position = UnityObjectToClipPos(input.vertex);
             output.color = input.color;
             return output;
         }
         struct FragOut
         {
             float4 color : COLOR;
         };
         FragOut passThrough(float4 color : COLOR)
         {
             FragOut output;
             output.color = color;
             return output;
         }
         ENDCG
     }
 }
 FallBack "Diffuse"
}