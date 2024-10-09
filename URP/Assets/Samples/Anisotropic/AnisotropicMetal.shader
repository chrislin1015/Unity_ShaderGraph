Shader "Custom/AnisotropicShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}   // 基礎貼圖
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5   // 光滑度
        _Anisotropy ("Anisotropy", Range(-1.0, 1.0)) = 0.0   // 各向異性強度
        _SpecColor ("Specular Color", Color) = (1,1,1,1)   // 高光顏色
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Glossiness;
            float _Anisotropy;
            float4 _SpecColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float3 AnisotropicBRDF(float3 lightDir, float3 viewDir, float3 normal, float anisotropy, float glossiness)
            {
                float3 halfwayDir = normalize(lightDir + viewDir);

                // 使用法線與光線方向的點積來進行基本的鏡面反射
                float NdotH = saturate(dot(normal, halfwayDir));

                // 用來計算切線方向的點積，用來模擬各向異性效果
                float3 tangent = normalize(cross(normal, float3(0.0, 1.0, 0.0)));  // 使用簡單的切線方向
                float TdotH = saturate(dot(tangent, halfwayDir));

                // 調整反射的高光根據各向異性強度進行拉伸
                float anisotropicFactor = pow(TdotH, 1.0 - anisotropy);
                float specular = pow(NdotH, glossiness * 128.0 * anisotropicFactor);

                return _SpecColor.rgb * specular;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 取出基礎的貼圖顏色
                fixed4 albedo = tex2D(_MainTex, i.uv);

                // 簡單的方向性光源，假設光線從上方來
                float3 lightDir = normalize(float3(0.0, 1.0, 1.0));

                // 計算視角方向
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // 計算各向異性反射
                float3 specular = AnisotropicBRDF(lightDir, viewDir, i.worldNormal, _Anisotropy, _Glossiness);

                // 最終顏色 = 貼圖顏色 + 高光顏色
                return fixed4(albedo.rgb + specular, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
