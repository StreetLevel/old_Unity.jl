Shader "defaultSpriteShader" {

 Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
 _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _BumpMap ("Normalmap", 2D) = "bump" {}
 }

 SubShader {
 Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
 LOD 700
 CGPROGRAM
 #pragma surface surf BlinnPhong alpha
// surface = it's a surface shader
// BlinnPhong = a type of built-in lighting formula
// alpha = this shader use transparency
// surf = the name of the fragment surface shader defined later

 sampler2D _MainTex;
 sampler2D _BumpMap;
 float4 _Color;
 float _Shininess;

 // definition of the input structure sent to the fragment surface
 // shader for each fragment/pixel
 struct Input {
     float2 uv_MainTex;
     float2 uv2_BumpMap;
     // here we tell Unity that we intend to use vertex color 
     // in the fragment shader
     float4 color : COLOR;
 };

 // the fragment surface shader definition
 void surf (Input IN, inout SurfaceOutput o) 
 {
     // the base color of each pixel is defined
     // by the main texture
     half4 tex = tex2D(_MainTex, IN.uv_MainTex);

     // _Color is the color defined in the material inspector
     // it's not the vertex color!
     half4 baseColor = tex * _Color ;
     half4 c = tex * _Color;

     // IN.color is the vertex color value interpolated for this
     // particular fragment/pixel.
     // Here, we choose once more to multiply the albedo(=diffuse)
     // value of this pixel by the vertex color and then by 2
     o.Albedo = c.rgb * IN.color.rgb * 2;

     // the final alpha of each pixel is also multiplied by
     // the vertex alpha
     o.Alpha = c.a * IN.color.a; 

     // Here we add normal mapping and specular effect
     // that's why surface shaders are cool, quite simple...
     o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv2_BumpMap));
     o.Specular = _Shininess;
     o.Gloss = 1;
 }
 ENDCG
 }
}