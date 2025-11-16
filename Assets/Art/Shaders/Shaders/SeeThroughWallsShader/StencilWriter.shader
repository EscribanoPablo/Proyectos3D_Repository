Shader "Hidden/StencilWriter"
{
    Properties{}

        SubShader
    {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry-1"
        }

        Pass
        {
            Name "StencilWrite"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite Off
            ZTest Always
            ColorMask 0

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
                Fail Keep
                ZFail Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionHCS : SV_POSITION; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                return o;
            }

            half4 frag(Varyings i) : SV_Target { return half4(0,0,0,0); }
            ENDHLSL
        }
    }

    /*Properties{}

    SubShader
    {
        Tags 
        {
            "RenderType" = "Opaque"
        }

        Pass 
        {
            ZWrite Off
        }
    }*/
}