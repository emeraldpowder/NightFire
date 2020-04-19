Shader "Custom/Character" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Noise ("Noise (RGB)", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1,1)
    _Step1 ("Step 1", float) = 0.155
    _Step2 ("Step 2", float) = 0.150
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd vertex:vert

sampler2D _MainTex;
sampler2D _Noise;
float4 _Color;

float _Step1;
float _Step2;

struct Input {
    float2 uv_MainTex;
    float3 objPos;
};
 
void vert (inout appdata_full v, out Input o) {
    UNITY_INITIALIZE_OUTPUT(Input,o);
    o.objPos = v.vertex;
}

void surf (Input IN, inout SurfaceOutput o) {
    float2 uv = IN.objPos.xy;
    uv.x += _Time*200- IN.objPos.z;
    uv.y += -_Time*300 + IN.objPos.z;
    uv/=50;
    
    
    float2 uv2 = IN.objPos.xy;
    uv2.x += -_Time*40 - IN.objPos.z;
    uv2.y += _Time*60 + IN.objPos.z ;
    uv2/=10;
    
    float mul = 1 - smoothstep(_Step2, _Step1, IN.uv_MainTex.x);
    
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Emission = mul * tex2D(_Noise, uv) /* tex2D(_Noise, uv2)*/;
    o.Albedo = lerp(c.rgb,_Color, o.Emission);
    o.Alpha = c.a;
    //o.Emission *= 0.5;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}