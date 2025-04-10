// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Unlit UV Rotation in vertex with Texture Position and Size"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _Rotation ("Rotation", Range(0,360)) = 0.0
        _TexPos ("Texture Position", Vector) = (0,0,0,0)
        _TexSize ("Texture Size", Vector) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
           
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Rotation;
            float4 _TexPos;
            float4 _TexSize;
           
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
 
                // rotating UV
                const float Deg2Rad = (UNITY_PI * 2.0) / 360.0;
 
                float rotationRadians = _Rotation * Deg2Rad; // convert degrees to radians
                float s = sin(rotationRadians); // sin and cos take radians, not degrees
                float c = cos(rotationRadians);
 
                float2x2 rotationMatrix = float2x2( c, -s, s, c); // construct simple rotation matrix
 
                v.uv -= 0.5; // offset UV so we rotate around 0.5 and not 0.0
                v.uv = mul(rotationMatrix, v.uv); // apply rotation matrix
                v.uv += 0.5; // offset UV again so UVs are in the correct location
 
                // apply texture position and size
                float2 texCoord = v.uv.xy * _TexSize.xy + _TexPos.xy;
                o.uv = TRANSFORM_TEX(texCoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
           
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);              
                return col;
            }
            ENDCG
        }
    }
}