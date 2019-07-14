Shader "Unlit/shader1"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Wave("Wave", Range(0, 1)) = 1
		_Speed("Speed", Range(0, 100)) = 100
		_Magnitude("Magnitude", Range(0, 1)) = 0.06
		_Voxelize("Voxelize", Range(0, 1)) = 1
		_VoxelSize("Voxel Size", Float) = 0.1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;

			int _Wave;
			float _Speed;
			float _Magnitude;

			int _Voxelize;
			float _VoxelSize;
			
			
			v2f vert (appdata v)
			{
				v2f o;
				if (_Voxelize == 1) {
					v.vertex = round(v.vertex / _VoxelSize) * _VoxelSize;
				}
				o.vertex = UnityObjectToClipPos(v.vertex);
				if (_Wave == 1) {
					o.vertex.x = o.vertex.x + _Magnitude * sin(o.vertex.y + _Time.x*_Speed);
				}
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				return color;
			}
			ENDCG
		}
	}
}
