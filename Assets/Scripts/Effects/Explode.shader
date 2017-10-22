// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Explode" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Progress("Progress", Range(0, 1)) = 0.0
		_Spread("Spread", Float) = 1
		_Gravity("Gravity", Float) = 10
		_FloorHeight("Floor Height", Float) = 0.0
	}
	SubShader {
		Tags { 
			"Queue"="Transparent"
			"RenderType"="Transparent" 
			"IgnoreProjector" = "True"
		}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "UnityCG.cginc"

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		half _Progress;
		float _Spread;
		float _Gravity;
		float _FloorHeight;

		void vert(inout appdata_full v) {
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

			float progress = max(0, _Progress + (v.color.x / 1) * (_Progress - 1));

			//hardcore quadratic equation here
			float ny = v.normal.y;
			float maxSpreadProgress = ((ny * _Spread) + sqrt(ny * ny * _Spread * _Spread + 4 * _Gravity * (worldPos.y - _FloorHeight))) / (2 * _Gravity);

			v.vertex.xyz += v.normal * min(maxSpreadProgress, progress) * _Spread;

			worldPos = mul(unity_ObjectToWorld, v.vertex);
			worldPos.y -= progress * progress * _Gravity;
			worldPos.y = max(_FloorHeight, worldPos.y);

			v.vertex = mul(unity_WorldToObject, worldPos);
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			o.Alpha = 1 - _Progress;
		}
		ENDCG
				
	}
	FallBack "Diffuse"
}
