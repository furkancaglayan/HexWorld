using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif

namespace HexWorld
{

    public static class _EditorPostProcessingUtility
    {
#if UNITY_POST_PROCESSING_STACK_V2
    public static void AddEffect(HexWorldEffect effect)
    {
        if (effect == null)
        {
            EditorUtility.DisplayDialog("Null Exception", "Effect is empty.", "Ok");
            return;
        }

        Camera camera = RuntimeUtility.CreateCamera();
        RuntimeUtility.ClearEffectsAndLights();
        PostProcessLayer layer = camera.GetComponent<PostProcessLayer>();
        if (layer == null)
            layer = camera.gameObject.AddComponent<PostProcessLayer>();
        AddLayer("PostProcessing");

        layer.antialiasingMode = effect.AA;
        layer.volumeTrigger = camera.transform;
        layer.volumeLayer = LayerMask.GetMask("PostProcessing");
        //now volume

        GameObject newGO = new GameObject("[HexWorld_Effects]", typeof(PostProcessVolume));
        PostProcessVolume volume = newGO.GetComponent<PostProcessVolume>();

        volume.isGlobal = true;
        volume.profile = effect.profile;
        newGO.layer = 8;

        //add lightning effects

        RenderSettings.ambientLight = effect.ambientColor;
        RenderSettings.ambientIntensity = effect.ambientIntensity;
        RenderSettings.reflectionIntensity = effect.reflectionIntensity;
        RenderSettings.ambientEquatorColor = effect.equatorColor;
        RenderSettings.ambientGroundColor = effect.groundColor;
        RenderSettings.ambientSkyColor = effect.skyColor;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
        RenderSettings.ambientMode = AmbientMode.Trilight;


        //add lights
        if (effect.lights.Count == 0)
            return;
        GameObject lightObj = new GameObject("[HexWorld_Lights]");
        for (int i = 0; i < effect.lights.Count; i++)
            effect.AddLight(effect.lights[i], lightObj.transform);

    }
#endif

    }

}