// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

struct v2f
{
	float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
	float4 color : TEXCOORD2;
	float4 vertex : SV_POSITION;
    float3 cubenormal : TEXCOORD3;

};

sampler2D _MainTex;
samplerCUBE _ToonShade;
float4 _MainTex_ST;
float _CurveStrength;
float4 _Color;


v2f vert(appdata v)
{
	v2f o;

	float _Horizon = 100.0f;
	float _FadeDist = 50.0f;

	o.vertex = UnityObjectToClipPos(v.vertex);   
	float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z+0.02f);
	o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);   
    o.vertex.y -= _CurveStrength * dist * dist * _ProjectionParams.x;
	UNITY_TRANSFER_FOG(o, o.vertex);
	return o;
}

fixed4 frag(v2f i) : SV_Target
{
	// sample the texture
	fixed4 col = _Color * tex2D(_MainTex, i.uv);
    fixed4 cube = texCUBE(_ToonShade, i.cubenormal);
    fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a);
//fixed4 col = tex2D(_MainTex, i.uv) * i.color;
// apply fog
//UNITY_APPLY_FOG(i.fogCoord, col);
    UNITY_APPLY_FOG(i.fogCoord, c);
    return c;
}