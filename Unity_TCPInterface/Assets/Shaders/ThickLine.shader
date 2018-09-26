// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ThickLine"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
         _Width ("Width", Range(0,1)) = 0.0125
    }
    SubShader
    {
        Tags {"RenderType" = "Transparent" "Queue" = "Transparent" } 
        //LOD 100
        //Cull off
        //ZWrite Off // don't write to depth buffer 
        //Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
        Pass
        {


         
            CGPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geo
           
            #include "UnityCG.cginc"
            //#include "Lighting.cginc"
 
            struct v2g
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };
 
            struct g2f
            {
                float4 pos: POSITION;
                float4 color : COLOR;
            };
 
            fixed4 _Color;
           
            v2g vert (float4 position : POSITION, float4 color : COLOR)
            {
                v2g o;
                o.vertex = position;
                o.color = color;
                return o;
            }

            float _Width;
 
            [maxvertexcount(4)]
            void geo(line v2g v[2], inout TriangleStream<g2f> ts)
            {
                float weight = _Width;
 
                float4 p1 = UnityObjectToClipPos(v[0].vertex);
                float4 p2 = UnityObjectToClipPos(v[1].vertex);
 
                float4 dir = normalize(p2 - p1);
                float4 perp = float4(dir.y, -dir.x, 0, 0);
 
                float4 v1_top = p1 + perp * weight;
                float4 v1_bot = p1 - perp * weight;
 
                float4 v2_top = p2 + perp * weight;
                float4 v2_bot = p2 - perp * weight;
 
                g2f o1;
                o1.pos = v1_top;
                o1.color = v[0].color;
                ts.Append(o1);
 
                g2f o2;
                o2.pos = v1_bot;
                o2.color = v[0].color;
                ts.Append(o2);
 
                g2f o3;
                o3.pos = v2_top;
                o3.color = v[1].color;
                ts.Append(o3);
 
                g2f o4;
                o4.pos = v2_bot;
                o4.color = v[1].color;
                ts.Append(o4);
            }
           
            fixed4 frag (g2f i) : SV_Target
            {
                fixed4 col = i.color;//*_LightColor0;
                //col.a = .1;
                return col;
            }
            ENDCG
        }
    }
}
 