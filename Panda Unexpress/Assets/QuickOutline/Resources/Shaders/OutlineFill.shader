Shader "Custom/Outline Fill URP VR" {
    Properties {
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", Range(0, 10)) = 2
    }

    SubShader {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent+110"
            "RenderType" = "Transparent"
            "DisableBatching" = "True" // Required for accurate object-space math
        }

        Pass {
            Name "Fill"
            Cull Off
            ZTest [_ZTest]
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            Stencil {
                Ref 1
                Comp NotEqual
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Ensure Unity handles VR stereo instancing correctly
            #pragma multi_compile_instancing 
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float3 smoothNormalOS : TEXCOORD3; // Your baked smooth normals
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _OutlineColor;
                float _OutlineWidth;
            CBUFFER_END

            Varyings vert(Attributes input) {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Use smooth normals if they exist, otherwise fallback to standard normals
                float3 normalOS = any(input.smoothNormalOS) ? input.smoothNormalOS : input.normalOS;
                
                // Extrude the mesh in Object Space (Uniform for both VR eyes)
                // We divide by 100 to keep the inspector slider values manageable
                input.positionOS.xyz += normalOS * (_OutlineWidth / 500.0);
                
                // Convert to Clip Space for the final render
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = _OutlineColor;

                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(input);
                return input.color;
            }
            ENDHLSL
        }
    }
}