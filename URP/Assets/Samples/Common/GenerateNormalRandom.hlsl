#ifndef GENERATE_NORMAL_RANDOM_HLSL
#define GENERATE_NORMAL_RANDOM_HLSL

float RandomFloat(float2 seed)
{
    return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453) + 0.00001;
}

float RandomHalf(half2 seed)
{
    return frac(sin(dot(seed, half2(12.9898, 78.233))) * 43758.5453) + 0.00001;
}

void GenerateNormalRandom_float(float2 seed, float min, float max, out float3 Out)
{
    float u1 = RandomFloat(seed);
    float u2 = RandomFloat(seed + float2(132.54, 464.32));

    float radius = sqrt(-2.0f * log(u1)) * 4.0;
    float theta = 2.0f * 3.1415926f * u2;

    float x = radius * cos(theta);
    float y = radius * sin(theta);

    float2 result;
    result.x = x * (max - min) + min;
    result.y = y * (max - min) + min;

    Out = float3(result.xy, 1);
}

void GenerateNormalRandom_half(half2 seed, half min, half max, out half3 Out)
{
    half u1 = RandomFloat(seed);
    half u2 = RandomFloat(seed + half2(132.54, 464.32));

    half radius = sqrt(-2.0f * log(u1)) * 4.0;
    half theta = 2.0f * 3.1415926f * u2;

    half x = radius * cos(theta);
    half y = radius * sin(theta);

    half2 result;
    result.x = x * (max - min) + min;
    result.y = y * (max - min) + min;

    Out = half3(result.xy, 1);
}

#endif