#ifndef MAIN_LIGHT_HLSL
#define MAIN_LIGHT_HLSL

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 

void MainLight_float(
    float3 worldPos,
    out float3 lightDir,
    out float3 lightColor,
    out float lightAttenuation,
    out float shadowAttenuation)
{
    #if SHADERGRAPH_PREVIEW
    lightDir = float3(0.5, 0.5, 0.0);
    lightColor = float3(1.0, 1.0, 1.0);
    lightAttenuation = 1.0;
    shadowAttenuation = 1.0;
    #else
    #if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(worldPos);
    float4 shadowCoord = ComputeScreenPos(clipPos);
    #else
    float4 shadowCoord = TransformWorldToShadowCoord(worldPos);
    #endif
    Light mainLight = GetMainLight(shadowCoord);
    lightDir = mainLight.direction;
    lightColor = mainLight.color;
    lightAttenuation = mainLight.distanceAttenuation;
    shadowAttenuation = mainLight.shadowAttenuation;
    #endif  
}

void MainLight_half(
    half3 worldPos,
    out half3 lightDir,
    out half3 lightColor,
    out half lightAttenuation,
    out half shadowAttenuation)
{
    #if SHADERGRAPH_PREVIEW
    
    lightDir = half3(0.5, 0.5, 0.0);
    lightColor = half3(1.0, 1.0, 1.0);
    lightAttenuation = 1.0;
    shadowAttenuation = 1.0;
    
    #else

    #if SHADOWS_SCREEN
    
    half4 clipPos = TransformWorldToHClip(worldPos);
    half4 shadowCoord = ComputeScreenPos(clipPos);
    
    #else
    
    half4 shadowCoord = TransformWorldToShadowCoord(worldPos);
    
    #endif
    
    Light mainLight = GetMainLight(shadowCoord);
    lightDir = mainLight.direction;
    lightColor = mainLight.color;
    lightAttenuation = mainLight.distanceAttenuation;
    shadowAttenuation = mainLight.shadowAttenuation;
    
    #endif  
}

#endif  