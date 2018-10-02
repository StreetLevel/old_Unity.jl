Shader "Custom/VertexColors2" {
    Properties {
        _Glossiness ("Smoothness", Range(0,1)) = 0.25
        _Metallic ("Metallic", Range(0,1)) = 0.25
    }
    SubShader {
      Tags { "Queue" = "Transparent" } 
         // draw after all opaque geometry has been drawn
      Pass {
         Cull Front // first pass renders only back faces 
             // (the "inside")
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

         CGPROGRAM 
         
         
         #pragma vertex vert 
         #pragma fragment frag
        #include "UnityCG.cginc"
         //#include "Lighting.cginc"
         /*
         
         float4 vert(float4 vertexPos : POSITION) : SV_POSITION 
         {
            return UnityObjectToClipPos(vertexPos);
         }
 
         float4 frag(void) : COLOR 
         {
            return float4(1.0, 0.0, 0.0, .25);
               // the fourth component (alpha) is important: 
               // this is semitransparent red
         }
         */

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
         VertOut vert(VertIn input, float3 normal : NORMAL)
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

        half _Glossiness;
        half _Metallic;

         FragOut frag(float4 color : COLOR)
         {
             FragOut output;
             output.color = color ;//* _LightColor0;
             
             return output;
         }
 
         ENDCG  
      }

      /*Pass {
         Cull Back // second pass renders only front faces 
             // (the "outside")
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

         CGPROGRAM 
 
         #pragma vertex vert 
         #pragma fragment frag
 
         float4 vert(float4 vertexPos : POSITION) : SV_POSITION 
         {
            return UnityObjectToClipPos(vertexPos);
         }
 
         float4 frag(void) : COLOR 
         {
            return float4(0.0, 1.0, 0.0, 0.3);
               // the fourth component (alpha) is important: 
               // this is semitransparent green
         }
 
         ENDCG  
      }*/
   }

    /*
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _GridTex ("Grid Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
        // Tags {"RenderType"="Opaque"}
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"}
        LOD 200
 
        Pass {
            Cull Off
            //Blend One OneMinusSrcAlpha
            ColorMask 0
        }
        // Pass {
            ZWrite Off
 
            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows alpha:blend
            #pragma target 3.5
 
            sampler2D _MainTex;
            sampler2D _GridTex;
 
            struct Input {
                float2 uv_MainTex;
                float4 color : COLOR;
                float3 worldPos;
            };
 
            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
 
            void surf (Input IN, inout SurfaceOutputStandard o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
 
                float2 gridUV = IN.worldPos.xz;
                gridUV.x *= 1 / (4 * 8.66025404);
                gridUV.y *= 1 / (2 * 15.0);
                fixed4 grid = tex2D(_GridTex, gridUV);
 
                o.Albedo = c.rgb * IN.color * grid;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = IN.color.a;
            }
            ENDCG
        // }
    }
    FallBack "Diffuse"*/
}