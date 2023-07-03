Shader "Universal Render Pipeline/Custom/SimpleGrabPassBlur" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _BumpAmt  ("Distortion", Range (0,128)) = 10
        _MainTex ("Tint Color (RGB)", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
        _Size ("Size", Range(0, 20)) = 1
    }
 
    SubShader {
        Tags {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Opaque"
        }
 
        GrabPass { Tags { "LightMode" = "Always" } }
 
        Pass {
            Tags {
                "LightMode" = "Always"
                "RenderType" = "Opaque"
            }
 
            HLSLINCLUDE
             
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
             
            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
            };
             
            struct v2f {
                float4 vertex : SV_POSITION;
                float4 uvgrab : TEXCOORD0;
            };
             
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                return o;
            }
             
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _Size;
             
            fixed4 frag( v2f i ) : SV_Target {
                fixed4 sum = fixed4(0,0,0,0);
                #define GRABPIXEL(weight,kernelx) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx*_Size, i.uvgrab.y, i.uvgrab.z, i.uvgrab.w))) * weight
                sum += GRABPIXEL(0.05, -4.0);
                sum += GRABPIXEL(0.09, -3.0);
                sum += GRABPIXEL(0.12, -2.0);
                sum += GRABPIXEL(0.15, -1.0);
                sum += GRABPIXEL(0.18,  0.0);
                sum += GRABPIXEL(0.15, +1.0);
                sum += GRABPIXEL(0.12, +2.0);
                sum += GRABPIXEL(0.09, +3.0);
                sum += GRABPIXEL(0.05, +4.0);
                return sum;
            }
 
            ENDHLSL
        }
    }
}