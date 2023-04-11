Shader "Tanknarok/ForcefieldHologram" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
		 _MaxDistance ("Max Distance", Range (0, 100)) = 4
    _EmissionIntensity ("Player Proximity Intensity", Range(0, 2.0)) = 1.2

    _MinStrength ("Min Strength", Range (0, 1)) = 0
    _FallOffStrength ("Fall Off Strength", Range (0, 4)) = 1.1
    [HideInInspector]_FallOffYOffset("Fall Off Y Offset", Range (0, 1)) = 0
    _IntersectionBoost ("Intersection Boost", Range (0, 1)) = 1

    _PositionPLAYER1("Position PLAYER 1", Vector) = (0,0,0,0)
    _PositionPLAYER2("Position PLAYER 2", Vector) = (0,0,0,0)
    _PositionPLAYER3("Position PLAYER 3", Vector) = (0,0,0,0)
    _PositionPLAYER4("Position PLAYER 4", Vector) = (0,0,0,0)

    [Toggle] _PLAYER2Toggle("PLAYER 2 Toggle", Float) = 0
    [Toggle] _PLAYER1Toggle("PLAYER 1 Toggle", Float) = 0
    [Toggle] _PLAYER3Toggle("PLAYER 3 Toggle", Float) = 0
    [Toggle] _PLAYER4Toggle("PLAYER 4 Toggle", Float) = 0

    [HideInInspector]_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
}

SubShader {
    Tags {
        "RenderType"="Transparent"
        "Queue"="Transparent"
        "IgnoreProjector"="True"
    }

    Pass {
        Name "URP"
        Tags { "LightMode"="ForwardBase" }

        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #include "UnityCG.cginc"
        #include "UnityStandardCore.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float4 posWorld : TEXCOORD1;
            float4 projPos : TEXCOORD2;
        };

        float4x4 unity_ObjectToWorld;
        float4x4 unity_WorldToObject;

        sampler2D _MainTex;
        float4 _MainTex_ST;
        float4 _TintColor;
        float _MaxDistance;
        float _EmissionIntensity;
        float _MinStrength;
        float _FallOffStrength;
        float _FallOffYOffset;
        float _IntersectionBoost;
        float4 _PositionPLAYER1;
        float4 _PositionPLAYER2;
        float4 _PositionPLAYER3;
        float4 _PositionPLAYER4;
        float _PLAYER2Toggle;
        float _PLAYER1Toggle;
        float _PLAYER3Toggle;
        float _PLAYER4Toggle;
        float _Cutoff;
			// calculate distance to each player
			float dist1 = length(i.posWorld.xyz - _PositionPLAYER1.xyz);
			float dist2 = length(i.posWorld.xyz - _PositionPLAYER2.xyz);
			float dist3 = length(i.posWorld.xyz - _PositionPLAYER3.xyz);
			float dist4 = length(i.posWorld.xyz - _PositionPLAYER4.xyz);

			// check if player is close enough to forcefield to affect it
			float strength1 = dist1 <= _MaxDistance ? (1 - (dist1 / _MaxDistance)) : 0;
			float strength2 = dist2 <= _MaxDistance ? (1 - (dist2 / _MaxDistance)) : 0;
			float strength3 = dist3 <= _MaxDistance ? (1 - (dist3 / _MaxDistance)) : 0;
			float strength4 = dist4 <= _MaxDistance ? (1 - (dist4 / _MaxDistance)) : 0;

			// apply player toggle
			strength1 *= _PLAYER1Toggle;
			strength2 *= _PLAYER2Toggle;
			strength3 *= _PLAYER3Toggle;
			strength4 *= _PLAYER4Toggle;

			// calculate final strength based on fall off
			float strength = max(max(max(strength1, strength2), strength3), strength4);
			float fallOff = _FallOffStrength * pow(max(0, i.posWorld.y - _FallOffYOffset), 2);
			strength = max(0, strength - fallOff);

			// apply intersection boost
			float4 col = strength > 0 ? tex * _TintColor : float4(0, 0, 0, 0);
			col.a = step(_Cutoff, max(max(max(strength1, strength2), strength3), strength4)) * _IntersectBoost;

			// apply emission
			col.rgb *= _EmissionIntensity;

			return col;
		}
		ENDCG
	}
}
FallBack "Diffuse"