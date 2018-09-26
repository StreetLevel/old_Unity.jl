// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SpriteParticleTest"
{
 
   Properties
   {
     _Sprite("Sprite", 2D) = "Circle.png" {}
     _Color("Color", Color) = (1.0,1.0,1.0,1.0)
     _Size("Size", float) = 0.1
   }
   SubShader
   {
     Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
     Blend SrcAlpha One
     //Blend SrcAlpha OneMinusSrcAlpha
     AlphaTest Greater .01
     ColorMask RGB
     Cull Off Lighting Off ZWrite Off
 
     Pass
     {
     
 
     CGPROGRAM
     #pragma target 4.0
 
     #pragma vertex vert
     #pragma geometry geom
     #pragma fragment frag
 
     #include "UnityCG.cginc"
     //#include "Lighting.cginc"

     fixed4 _Color;
     float _Size;
     sampler2D _Sprite;
     
     struct appdata_t {
       float4 vertex : POSITION;
       fixed4 color : COLOR;
       float2 texcoord : TEXCOORD0;
     };
       
     struct v2g
     {
       float4 pos : SV_POSITION;
       fixed4 color : COLOR;
     };
 
     v2g vert(appdata_t v)
     {
       v2g OUT;
       OUT.pos = UnityObjectToClipPos (float4(v.vertex.xyz,1.0f));
       OUT.color = v.color;
       return OUT;
     }
     
     struct g2f {
       float4 pos : SV_POSITION;
       float2 uv : TEXCOORD0;
       fixed4 color : COLOR;
     };
 
     [maxvertexcount(4)]
     void geom(point v2g IN[1], inout TriangleStream<g2f> outStream)
     {
       float dx = _Size;
       float dy = _Size * _ScreenParams.x / _ScreenParams.y;
       g2f OUT;
       OUT.pos = IN[0].pos + float4(-dx, dy,0,0); OUT.uv=float2(0,0); OUT.color = IN[0].color; outStream.Append(OUT);
       OUT.pos = IN[0].pos + float4( dx, dy,0,0); OUT.uv=float2(1,0); OUT.color = IN[0].color; outStream.Append(OUT);
       OUT.pos = IN[0].pos + float4(-dx,-dy,0,0); OUT.uv=float2(0,1); OUT.color = IN[0].color; outStream.Append(OUT);
       OUT.pos = IN[0].pos + float4( dx,-dy,0,0); OUT.uv=float2(1,1); OUT.color = IN[0].color; outStream.Append(OUT);
       outStream.RestartStrip();
     }
 
     float4 frag (g2f IN) : COLOR
     {
         
       fixed4 col =  IN.color  * _Color *tex2D(_Sprite, IN.uv);
       //fixed4 col = IN.color * _LightColor0;
       return col;    
     }
 
     ENDCG
 
     }
   }
 
Fallback Off
}