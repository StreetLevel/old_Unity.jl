Shader "Custom/VertexColorWithNoiseTransparent" {
    Properties{
        _NoisePos("Noise Positon", Float) = (1,1,1)
        _NoiseScale("Noise Scale", Range(0,1)) = 1
        _NoiseInfluence("Noise Influence", Range(0,1)) = 0.2
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        #include "noiseSimplex.cginc"
 
 
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
            float4 color : COLOR;
            float3 worldPos;
        };