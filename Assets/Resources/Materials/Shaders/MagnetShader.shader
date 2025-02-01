Shader "Custom/GlowingShieldShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (0,1,1,1)
        _GlowStrength ("Glow Strength", Float) = 1.0
        _Frequency ("Frequency", Float) = 10.0
        _Speed ("Speed", Float) = 1.0
        _Transparency ("Transparency", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _GlowColor;
            float _GlowStrength;
            float _Frequency;
            float _Speed;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = length(uv - 0.5) * 2.0;

                // Creating the glow effect using a sine wave
                float glow = sin(_Time.y * _Speed + dist * _Frequency) * 0.5 + 0.5;
                glow = pow(glow, _GlowStrength);

                // Combining glow with main texture color
                fixed4 tex = tex2D(_MainTex, uv);
                fixed4 finalColor = _Color * tex + _GlowColor * glow;

                // Setting transparency
                finalColor.a = _Transparency;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
