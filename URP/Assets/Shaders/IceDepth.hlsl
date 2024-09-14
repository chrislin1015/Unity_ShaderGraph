#ifndef ICEDEPTH_INCLUDE
#define ICEDEPTH_INCLUDE

void IceDepth_float(
    in UnityTexture2D mainTex,
    in float2 uv,
    in UnitySamplerState ss,
    in float samples,
    in float offset,
    in float3 worldPosition,
    in float lerpFactor,
    in int lod,
    out float4 outColor)
{
    float4 color = 0;
    float uoff = 0;
    float voff = 0;

    for (int i = 0; i < samples; i++)
    {
        float2 tempUV = float2(uoff, voff);
        color += SAMPLE_TEXTURE2D_LOD(mainTex, ss, tempUV + uv, lod);
        uoff += offset * (_WorldSpaceCameraPos.x - worldPosition.x);
        voff += offset * (_WorldSpaceCameraPos.y - worldPosition.y);
    }

    float4 blendColor = (color / samples);
    outColor = lerp(SAMPLE_TEXTURE2D(mainTex, ss, uv), blendColor, lerpFactor);
}

void IceDepth_half(
    in UnityTexture2D mainTex,
    in half2 uv,
    in UnitySamplerState ss,
    in half samples,
    in half offset,
    in half3 worldPosition,
    in half lerpFactor,
    in int lod,
    out half4 outColor)
{
    half4 color = 0;
    half uoff = 0;
    half voff = 0;

    for (int i = 0; i < samples; i++)
    {
        half2 tempUV = half2(uoff, voff);
        color += SAMPLE_TEXTURE2D_LOD(mainTex, ss, tempUV + uv, lod);
        uoff += offset * (_WorldSpaceCameraPos.x - worldPosition.x);
        voff += offset * (_WorldSpaceCameraPos.y - worldPosition.y);
    }

    half4 blendColor = (color / samples);
    outColor = lerp(SAMPLE_TEXTURE2D(mainTex, ss, uv), blendColor, lerpFactor);
}

#endif
