// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Modified by WeAthFolD
// inspiration: https://www.shadertoy.com/view/lsK3Dm

Shader "Sprites/Sun"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _PerlinTex ("PerlinTex", 2D) = "white" {}
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SunFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            sampler2D _NoiseTex, _PerlinTex;

            float pi = 3.1415926;

            float perlin(float2 coord) {
                return tex2D(_PerlinTex, coord).r;
            }

            fixed4 SunFrag(v2f IN) : SV_Target
            {
                fixed4 colA = fixed4(1.0, 0.0, 0.0, 1.0);
                fixed4 colB = fixed4(2.0, 1.5, 0.8, 1.0);

                float2 uv0 = 2.0 * (IN.texcoord - 0.5);
                float angle = 0.79;
                float2x2 rot = float2x2(sin(angle), -cos(angle), cos(angle), sin(angle));

                float2 uv1 = mul(uv0, rot);
                float2 uv2 = mul(rot, uv0);
                float3 enlarge = 2.0 - frac(float3(0.0, 0.333, 0.667) + _Time.z * 0.5);

                float alpha = atan2(uv0.y, uv0.x);
                float dr = perlin(float2(alpha / 3.141, 0.3 * _Time.y)) * 0.3;

                float r = dot(uv0, uv0) * (1 +  dr);
                float p = (pow(r, 3.0) + 0.3);

                uv0 *= p;
                uv1 *= p;
                uv2 *= p;

                float fire = dot(float3(
                    tex2D(_NoiseTex, uv0 * enlarge.x).x,
                    tex2D(_NoiseTex, uv1 * enlarge.y).y,
                    tex2D(_NoiseTex, uv2 * enlarge.z).z
                ), smoothstep(float3(0.5, 0.5, 0.5), float3(0.0, 0.0, 0.0),  abs(frac(enlarge) - 0.5) ));

                return lerp(colA, colB, fire) - r * r * 1.75;
            }
            /*
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec4 colA = vec4(1.0, 0.0, 0.0, 1.0);
    vec4 colB = vec4(2.0, 1.5, 0.8, 1.0);
	vec2 uv0 = ((fragCoord.xy - iResolution.xy * .5) / iResolution.y) * 2.0;
    float angle = 0.79;
    mat2 rot = mat2(
        sin(angle), -cos(angle),
        cos(angle), sin(angle)
    );
    vec2 uv1 = uv0 * rot;
    vec2 uv2 = rot * uv0;
    vec3 enlarge = 2. - fract(vec3(0., 0.333, 0.667) + iTime*0.5);
    
    float r = dot(uv0,uv0);
    float p = (pow(r, 3.) + 0.3);
    uv0 *= p;
    uv1 *= p;
    uv2 *= p;
    float fire = dot(vec3(
        texture(iChannel0, uv0 * enlarge.x).x,
        texture(iChannel0, uv1 * enlarge.y).y,
        texture(iChannel0, uv2 * enlarge.z).z
    ), smoothstep(vec3(0.5), vec3(0.0), abs(fract(enlarge)-0.5)));
    fragColor = mix(colA, colB, fire) - r*r * 1.75;
}
            */
        ENDCG
        }
    }
}
