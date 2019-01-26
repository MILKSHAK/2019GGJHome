// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Modified by WeAthFolD
// inspiration: https://www.shadertoy.com/view/lsK3Dm

Shader "Sprites/EnergyBar"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Glow ("Glow", Color) = (1,1,1,1)
        _MaxRadians ("MaxRadians", Float) = 0.8
        _Progress ("Progress", Float) = 1.0
        _MinRad ("MinRad", Float) = 1.0
        _MaxRad ("MaxRad", Float) = 1.0

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

            float pi = 3.1415926;

            float _MaxRadians, _Progress;
            float _MinRad, _MaxRad;
            float4 _Glow;

            fixed4 SunFrag(v2f IN) : SV_Target
            {
                float2 uv0 = 2.0 * (IN.texcoord - 0.5);
                float alpha = atan2(uv0.y, uv0.x);
                alpha = alpha + 3.14159 / 2;

                float r = dot(uv0, uv0);

                float thresh = _MaxRadians * _Progress;
                float dthresh = max(abs(alpha) - thresh, 0);
                float dr = max(0, max(_MinRad - r, r - _MaxRad));
                float d = max(dthresh, dr);
                float glow = min(1, sqrt(d) / 0.5);
                float a2 = d < 0.01 ? 1 : 1 - glow;

                fixed4 c = SampleSpriteTexture(IN.texcoord) * lerp(IN.color, _Glow, glow);
                c.a *= a2;

                c.rgb *= c.a;
                return c;
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
