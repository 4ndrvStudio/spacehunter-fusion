Shader "Custom/RenderFeature/KawaseBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex_TexelSize ("MainTex Texel Size", Vector) = (1, 1, 0, 0)
        _Offset ("Offset", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Offset -1, -1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0
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
            float4 _MainTex_TexelSize;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
{
    float2 res = _MainTex_TexelSize.xy;
    float offset = _Offset;

    fixed4 col;
    col.rgb = tex2D(_MainTex, i.uv).rgb;
    col.rgb += tex2D(_MainTex, i.uv + float2(offset, offset) * res).rgb;
    col.rgb += tex2D(_MainTex, i.uv + float2(offset, -offset) * res).rgb;
    col.rgb += tex2D(_MainTex, i.uv + float2(-offset, offset) * res).rgb;
    col.rgb += tex2D(_MainTex, i.uv + float2(-offset, -offset) * res).rgb;
    col.rgb /= 5.0f;

    return col;
}
            ENDCG
        }
    }
}