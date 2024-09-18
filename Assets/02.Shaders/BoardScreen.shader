Shader "Custom/BoardScreen"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _LEDTex ("LED Texture", 2D) = "white" {}
        _OffsetX ("Offset X", Float) = 0.0
        _OffsetY ("Offset Y", Float) = 0.0
        _Tiling ("Tiling", Float) = 1.0
        _Brightness ("Brightness", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uvMain : TEXCOORD0;
                float2 uvLED : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uvMain : TEXCOORD0;
                float2 uvLED : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_LEDTex);
            SAMPLER(sampler_LEDTex);

            float _OffsetX;
            float _OffsetY;
            float _Tiling;
            float _Brightness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uvMain = IN.uvMain;
                OUT.uvLED = IN.uvLED;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 mainUV = float2(_OffsetX, _OffsetY) + floor(IN.uvMain * _Tiling) / _Tiling;
                float4 mainTexColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, mainUV);
                float4 ledTexColor = SAMPLE_TEXTURE2D(_LEDTex, sampler_LEDTex, IN.uvLED * _Tiling);

                float4 resultColor = mainTexColor * ledTexColor * _Brightness;
                resultColor.a = mainTexColor.a;

                return resultColor;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}