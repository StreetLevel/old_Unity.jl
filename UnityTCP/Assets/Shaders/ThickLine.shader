// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ThickLine" 
{        
 Properties 
 {
   _MainTex ("TileTexture", 2D) = "white" {}
   _PointSize("Point Size", Float) = 0.15
   _Width ("Width", Range(0,1)) = 0.0125
 }

 SubShader 
 {
   LOD 200

   Pass 
   {
     CGPROGRAM

             //#pragma only_renderers d3d11
             #pragma target 4.0

             #include "UnityCG.cginc"
             #include "Lighting.cginc"

             #pragma vertex   myVertexShader
             #pragma geometry myGeometryShader
             #pragma fragment myFragmentShader

             #define TAM 36

             struct vIn // Into the vertex shader
             {
               float4 vertex : POSITION;
               float4 color  : COLOR0;
             };
             
             struct gIn // OUT vertex shader, IN geometry shader
             {
               float4 pos : SV_POSITION;
               float4 col : COLOR0;
             };
             
              struct v2f // OUT geometry shader, IN fragment shader 
              {
               float4 pos           : SV_POSITION;
               float2 uv_MainTex : TEXCOORD0;
               float4 col : COLOR0;
             };
             
             float4       _MainTex_ST;
             sampler2D _MainTex; 
             float     _PointSize;    
             float     _Width;           
             // ----------------------------------------------------
             gIn myVertexShader(vIn v)
             {
                 gIn o; // Out here, into geometry shader
                 // Passing on color to next shader (using .r/.g there as tile coordinate)
                 o.col = v.color;                
                 // Passing on center vertex (tile to be built by geometry shader from it later)
                 o.pos = v.vertex;

                 return o;
               }

             // ----------------------------------------------------

               [maxvertexcount(TAM)] 
             // ----------------------------------------------------
             // Using "point" type as input, not "triangle"
               void myGeometryShader(line gIn vert[2], inout TriangleStream<v2f> triStream)
               {

                 float weight = _Width;

                 float4 p1 = vert[0].pos;
                 float4 p2 = vert[1].pos;

                 float3 dir = normalize(p2 - p1);
                 float3 dxf3 = normalize(float3(-dir.y, dir.x, 0.0f));
                 float3 dyf3 = cross(dir,dxf3);

                 dyf3 = dyf3*weight;
                 dxf3 = dxf3*weight;

                 float4 dx = float4(dxf3.x, dxf3.y, dxf3.z, 0.0f);
                 float4 dy = float4(dyf3.x, dyf3.y, dyf3.z, 0.0f);

                 float4 _1 = p2-dx+dy;
                 float4 _2 = p2+dx+dy;
                 float4 _3 = p2+dx-dy;
                 float4 _4 = p2-dx-dy;

                 float4 _5 = p1+dx+dy;
                 float4 _6 = p1+dx-dy;
                 float4 _7 = p1-dx-dy;
                 float4 _8 = p1-dx+dy;
                 const float4 vc[TAM] = {
                                          _1, _2, _3,    //Top                                 
                                          _3, _4, _1,    //Top
                                          _3, _2, _5,     //Right
                                          _5, _6, _3,     //Right
                                          _4, _3, _6,     //Front
                                          _6, _7, _4,     //Front
                                          _7, _6, _5,    //Bottom                                         
                                          _5, _8, _7,     //Bottom
                                          _1, _4, _7,    //Left
                                          _7, _8, _1,    //Left
                                          _1, _8, _5,    //Back
                                          _5, _2, _1     //Back
                                          };


                 const float2 UV1[TAM] = { float2( 0.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ),         //Esta em uma ordem
                                           float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ),         //aleatoria qualquer.
                                           
                                           float2( 0.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), 
                                           float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ),
                                           
                                           float2( 0.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), 
                                           float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ),
                                           
                                           float2( 0.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), 
                                           float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ),
                                           
                                           float2( 0.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), 
                                           float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ),
                                           
                                           float2( 0.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), 
                                           float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f ), float2( 1.0f,    0.0f )                                            
                                         };    

                                         const int TRI_STRIP[TAM]  = {  0, 1, 2,  3, 4, 5,
                                          6, 7, 8,  9,10,11,
                                          12,13,14, 15,16,17,
                                          18,19,20, 21,22,23,
                                          24,25,26, 27,28,29,
                                          30,31,32, 33,34,35  
                                        }; 

                                        v2f v[TAM];
                                        int i;

                 // Assign new vertices positions 
                 //for (i=0;i<TAM;i++) { v[i].pos = vert[0].pos + vc[i]; v[i].col = vert[0].col;    }
                                        for (i=0;i<TAM;i++) { v[i].pos =  vc[i]; v[i].col = vert[0].col;    }

                 // Assign UV values
                                         for (i=0;i<TAM;i++) v[i].uv_MainTex = TRANSFORM_TEX(UV1[i],_MainTex); 

                 // Position in view space
                                           for (i=0;i<TAM;i++) { v[i].pos = UnityObjectToClipPos(v[i].pos); }

                 // Build the cube tile by submitting triangle strip vertices
                                             for (i=0;i<TAM/3;i++)
                                             { 
                                               triStream.Append(v[TRI_STRIP[i*3+0]]);
                                               triStream.Append(v[TRI_STRIP[i*3+1]]);
                                               triStream.Append(v[TRI_STRIP[i*3+2]]);    

                                               triStream.RestartStrip();
                                             }
                                           }

              // ----------------------------------------------------
                                           float4 myFragmentShader(v2f IN) : COLOR
                                           {
                 //return float4(1.0,0.0,0.0,1.0);
                                             return IN.col;// * _LightColor0;
                                           }

                                           ENDCG
                                         }
                                       } 
                                     }