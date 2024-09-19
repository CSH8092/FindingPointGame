//
//  OutlineFill.shader
//  QuickOutline
//
//  Created by Chris Nolet on 2/21/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

Shader "Custom/Outline Fill" {
  Properties {
    [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
    _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
    _OutlineWidth("Outline Width", Range(0, 10)) = 2
  }

  SubShader {
    Tags {
      "Queue" = "Transparent+110"
      "RenderType" = "Transparent"
      "DisableBatching" = "True"
    }

    Pass {
      Name "Outline"
      Cull Front  // Cull front faces to render the outline behind the object
      ZTest [_ZTest]
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB

      CGPROGRAM
      #include "UnityCG.cginc"

      #pragma vertex vert
      #pragma fragment frag

      struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f {
        float4 position : SV_POSITION;
        fixed4 color : COLOR;
        UNITY_VERTEX_OUTPUT_STEREO
      };

      uniform fixed4 _OutlineColor;
      uniform float _OutlineWidth;

      v2f vert(appdata v) {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

        // Calculate normal in view space
        float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
        float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, worldNormal));

        // Push vertices along the view-space normal to create the outline
        float3 offset = viewNormal * _OutlineWidth * 0.01;
        o.position = UnityObjectToClipPos(v.vertex + float4(offset, 0));

        o.color = _OutlineColor;
        return o;
      }

      fixed4 frag(v2f i) : SV_Target {
        return i.color;
      }

      ENDCG
    }
  }
}
