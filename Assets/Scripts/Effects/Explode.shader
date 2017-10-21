Shader "Custom/Explode" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Progress("Progress", Range(0, 1)) = 0.0
	}
	SubShader {
		Tags { 
			"Queue"="Transparent"
			"RenderType"="Transparent" 
		}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert addshadow alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		half _Progress;

		void vert(inout appdata_full v) {
			v.vertex.xyz += v.normal * _Progress;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			o.Alpha = (1 - _Progress);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
