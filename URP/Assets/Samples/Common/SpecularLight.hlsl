#ifndef SPECULAR_LIGHT_HLSL
#define SPECULAR_LIGHT_HLSL

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" 

void SpecularLight_float(
    float3 specular,
    float smoothness,
    float3 lightDir,
    float3 lightColor,
    float3 worldNormal,
    float3 worldViewDir,
    out float3 Out)
{
    #if SHADERGRAPH_PREVIEW

    Out = 0;

    #else

    //smoothness = exp2(10 * smoothness + 1);
    worldNormal = normalize(worldNormal);
    worldViewDir = SafeNormalize(worldViewDir);
    lightDir = normalize(lightDir);
    
    Out = LightingSpecular(lightColor, lightDir, worldNormal, worldViewDir, float4(specular, 0), smoothness);
    
    #endif
}

void SpecularLight_half(
    half3 specular,
    half smoothness,
    half3 lightDir,
    half3 lightColor,
    half3 worldNormal,
    half3 worldViewDir,
    out half3 Out)
{
    #if SHADERGRAPH_PREVIEW

    Out = 0;
    
    #else

    //smoothness = exp2(10 * smoothness + 1);
    worldNormal = normalize(worldNormal);
    worldViewDir = SafeNormalize(worldViewDir);
    lightDir = normalize(lightDir);

    // float3 halfVec = SafeNormalize(float3(lightDir) + float3(worldViewDir));
    // half NdotH = half(saturate(dot(worldNormal, halfVec)));
    // half modifier = pow(NdotH, smoothness);
    // half3 specularReflection = specular.rgb * modifier;
    // Out = lightColor * specularReflection;
    
    Out = 1;//LightingSpecular(lightColor, lightDir, worldNormal, worldViewDir, half4(specular, 0), smoothness);
    
    #endif
}

#endif
