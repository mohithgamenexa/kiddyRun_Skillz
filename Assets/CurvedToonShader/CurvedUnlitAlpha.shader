Shader "Unlit/CurvedUnlitAlpha"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
        _Color ("Main Color", Color) = (.5,.5,.5,1)
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
			// make fog work
	#pragma multi_compile_fog

	#include "CurvedCode.cginc"

			ENDCG
		}
	}
}
