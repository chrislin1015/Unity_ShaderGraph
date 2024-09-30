#ifndef ADDITIONAL_LIGHTS_HLSL
#define ADDITIONAL_LIGHTS_HLSL

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 

void AdditionalLights_float(
    float3 specular,
    float smoothness,
    float3 worldPosition,
    float3 worldNormal,
    float3 worldViewDir,
    out float3 OutDiffuse,
    out float3 OutSpecular)
{
    float3 diffuseColor = 0;
    float3 specularColor = 0;

    #ifndef SHADERGRAPH_PREVIEW

    //smoothness = exp2(10 * smoothness + 1);
    worldNormal = normalize(worldNormal);
    worldViewDir = normalize(worldViewDir);
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++)
    {
        Light light = GetAdditionalLight(i, worldPosition);
        float3 attenuationLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert(attenuationLightColor, light.direction, worldNormal);
        specularColor += LightingSpecular(
            attenuationLightColor, light.direction, worldNormal, worldViewDir, float4(specular, 1), smoothness);
    }
    
    #endif

    OutDiffuse = diffuseColor;
    OutSpecular = specularColor;
}

void AdditionalLights_half(
    half3 specular,
    half smoothness,
    half3 worldPosition,
    half3 worldNormal,
    half3 worldViewDir,
    out half3 OutDiffuse,
    out half3 OutSpecular)
{
    half3 diffuseColor = 0;
    half3 specularColor = 0;

    #ifndef SHADERGRAPH_PREVIEW

    //smoothness = exp2(10 * smoothness + 1);
    worldNormal = normalize(worldNormal);
    worldViewDir = normalize(worldViewDir);
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++)
    {
        Light light = GetAdditionalLight(i, worldPosition);
        half3 attenuationLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert(attenuationLightColor, light.direction, worldNormal);
        specularColor += LightingSpecular(
            attenuationLightColor, light.direction, worldNormal, worldViewDir, half4(specular, 1), smoothness);
    }
    
    #endif

    OutDiffuse = diffuseColor;
    OutSpecular = specularColor;
}

#endif