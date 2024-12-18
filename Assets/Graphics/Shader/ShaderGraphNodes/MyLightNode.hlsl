#ifndef MYLIGHTNODE_INCLUDED
#define MYLIGHTNODE_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _SHADOWS_SOFT

#endif

void GetMainLightParams_float(float3 WorldPosition, out half3 Direction, out half3 Color, out float DistanceAttenuation,
                              out half ShadowAttenuation)
{
    #if defined(SHADERGRAPH_PREVIEW)
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    DistanceAttenuation = 1;
    ShadowAttenuation = 1;
    
    #else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAttenuation = mainLight.distanceAttenuation;
    ShadowAttenuation = MainLightRealtimeShadow(shadowCoord);

    #endif
}

void GetAdditionalLight_float(float3 WorldPosition, float3 Normal, out half3 Color)
{
    #ifdef SHADERGRAPH_PREVIEW
    Color = half3(0.5, 0.5, 0.5);
    
    #else
    uint lightCount = GetAdditionalLightsCount();

    for (uint lightIndex = 0u; lightIndex < lightCount; ++lightIndex)
    {
        //ライトの取得
        Light light = GetAdditionalLight(lightIndex, WorldPosition);
        //ライティングの計算
        half3 lightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        Color += LightingLambert(lightColor, light.direction, Normal);
    }

    #endif
}

void GetHalfVector_float(float3 ViewVector, out float3 HalfVector)
{
    #ifdef SHADERGRAPH_PREVIEW
    HalfVector = half3(0.5, 0.5, 0);
    #else
    HalfVector = normalize(_MainLightPosition + ViewVector);
    #endif
}

void GetReceiveShadow_float(float ShadowAlpha, float3 WorldPos, out half ShadowAttenuation)
{
    #ifdef SHADERGRAPH_PREVIEW
    ShadowAttenuation = 1.0;
    
    #else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    half shadow = mainLight.shadowAttenuation;
    int pixelLightCount = GetAdditionalLightsCount();

    for (int i = 0; i < pixelLightCount; i++)
    {
        Light AddLight0 = GetAdditionalLight(i, WorldPos, 1);
        half shadow0 = AddLight0.shadowAttenuation;
        shadow *= shadow0;
    }

    ShadowAttenuation = shadow * ShadowAlpha;

    #endif
}

#ifndef SHADERGRAPH_PREVIEW

float4 TransformHClipToNormalizedScreenPos(float4 positionCS)
{
    float4 o = positionCS * 0.5f;
    o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
    o.zw = positionCS.zw;
    return o / o.w;
}

half SampleOffsetDepth(float3 positionVS, float2 offset)
{
    // カメラとの距離やカメラのFOVで見た目上の輪郭の太さが変わらないように、オフセットをViewSpaceで計算する
    float3 samplePositionVS = float3(positionVS.xy + offset, positionVS.z);
    float4 samplePositionCS = TransformWViewToHClip(samplePositionVS);
    float4 samplePositionVP = TransformHClipToNormalizedScreenPos(samplePositionCS);
        
    float offsetDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, samplePositionVP).r;
    return offsetDepth;
}

#endif

void SobelFilter_float(float3 PositionWS, float Thickness, float SobelFilterThreshold, out float4 Out)
{
    #ifdef SHADERGRAPH_PREVIEW
    Out =  0.5;

    #else
    float3x3 sobel_x = float3x3(-1, 0, 1, -2, 0, 2, -1, 0, 1);
    float3x3 sobel_y = float3x3(-1, -2, -1, 0, 0, 0, 1, 2, 1);

    float edgeX = 0;
    float edgeY = 0;

    float3 positionVS = TransformWorldToView(PositionWS);

    UNITY_UNROLL
    for (int x = -1; x <= 1; x++)
    {
        UNITY_UNROLL
        for (int y = -1; y <= 1; y++)
        {
            float2 offset = float2(x,y) * Thickness;
            half depth = SampleOffsetDepth(positionVS, offset);
            depth = LinearEyeDepth(depth, _ZBufferParams);
                
            float intensity = depth;
            edgeX += intensity * sobel_x[x + 1][y + 1];
            edgeY += intensity * sobel_y[x + 1][y + 1];
        }
    }

    // エッジの強度を計算
    float edgeStrength = length(float2(edgeX, edgeY));
    edgeStrength = step(SobelFilterThreshold, edgeStrength);
    Out = float4(edgeStrength, edgeStrength, edgeStrength, 1);
    #endif
}

#endif
