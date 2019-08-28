using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif

public static class EditorUtils 
{
    public static void AddLayer(string layer)
    {
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
        if (asset != null)
        {
            var so = new SerializedObject(asset);
            var layers = so.FindProperty("layers");

            var numLayers = layers.arraySize;


            for (int i = 0; i < numLayers; i++)
            {
                var existingTag = layers.GetArrayElementAtIndex(i);
                if (existingTag.stringValue.Equals(layer)) return;
            }

            int emptySpace = 8;
            layers.InsertArrayElementAtIndex(emptySpace);
            layers.GetArrayElementAtIndex(emptySpace).stringValue = layer;
            so.ApplyModifiedProperties();
            so.Update();
        }

    }
#if UNITY_POST_PROCESSING_STACK_V2
    public static void AddEffect(HexWorldEffect effect)
    {
        if (effect == null)
        {
            EditorUtility.DisplayDialog("Null Exception", "Effect is empty.", "Ok");
            return;
        }

        Camera camera = Utils.CreateCamera();
        Utils.ClearEffectsAndLights();
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
    public static void FixPPDefineSymbolBug()
    {
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/ProjectSettings.asset");
        if (asset != null)
        {
            var so = new SerializedObject(asset);
            var properties = so.FindProperty("scriptingDefineSymbols");
            var numProp = properties.arraySize;

            if (numProp == 0)
                return;
            properties.DeleteArrayElementAtIndex(0);



            so.ApplyModifiedProperties();
            so.Update();
        }

    }

    public static void SavePrefab(string gameObjectName, string mapsDirectory, HexWorldMap map)
    {
        bool valid = Utils.CheckIfDirectoryIsValid("Assets/" + mapsDirectory, false);
        if (!valid)
            return;
        if (map == null)
        {
            EditorUtility.DisplayDialog("Null Exception", "Create a map first.", "Ok");
            return;
        }

        if (map.gameObject == null)
        {
            EditorUtility.DisplayDialog("Null Exception", "Map object is missing.", "Ok");
            return;
        }
        if (map.IsEmpty())
        {
            EditorUtility.DisplayDialog("Map is Empty", "Add some tiles first.", "Ok");
            return;
        }
        string fullPath = "Assets/" + mapsDirectory + "/" + gameObjectName + ".prefab";



        GameObject prefab = map.gameObject;

        GameObject copy = Object.Instantiate(prefab);
        try
        {
            Transform chunks = copy.transform.GetChild(0);
            for (int i = 0; i < chunks.childCount; i++)
            {
                Transform chunk = chunks.GetChild(i);
                GameObject mesh = chunk.GetChild(1).gameObject;
                if (mesh.name.Contains("Mesh"))
                    Object.DestroyImmediate(mesh);

                //also delete the collider
                Collider col = chunk.GetComponent<Collider>();
                Object.DestroyImmediate(col);


            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
        Object asset = null;
#if UNITY_2018_3_OR_NEWER
        asset = PrefabUtility.SaveAsPrefabAsset(copy, fullPath);
#else
        asset = PrefabUtility.CreatePrefab(fullPath, copy);

#endif
        Object.DestroyImmediate(copy);
        if (asset != null)
            Selection.activeObject = asset;
    }

    /// <summary>
    /// Adds the given tag parameter to project.
    /// </summary>
    /// <param name="tag"></param>
    public static void AddTag(string tag)
    {
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
        if (asset != null)
        {
            var so = new SerializedObject(asset);
            var tags = so.FindProperty("tags");

            var numTags = tags.arraySize;

            for (int i = 0; i < numTags; i++)
            {
                var existingTag = tags.GetArrayElementAtIndex(i);
                if (existingTag.stringValue == tag) return;
            }

            tags.InsertArrayElementAtIndex(numTags);
            tags.GetArrayElementAtIndex(numTags).stringValue = tag;
            so.ApplyModifiedProperties();
            so.Update();
        }

    }

    public static void SaveMap(string path, string mapName, HexWorldMap map)
    {

        string fullPath = path + "/" + mapName + ".asset";
        bool fileExists = File.Exists(fullPath);
        int chosen = 0;
        if (fileExists)
        {
            chosen = EditorUtility.DisplayDialogComplex("File Exists!", "'" + fullPath + "' already exists. Want to continue?",
                "Yes", "No", "Assign Random Name");
        }

        if (chosen == 1)
            return;


        HexWorldStaticData static_data = ScriptableObject.CreateInstance<HexWorldStaticData>();
        if (chosen == 2)
            mapName = Utils.CreateName(18);

        static_data.name = mapName;
        static_data.LoadData(map);
        fullPath = path + "/" + mapName + ".asset";


        AssetDatabase.CreateAsset(static_data, fullPath);
        EditorUtility.SetDirty(static_data);


        var fileInfo = new FileInfo(path + "/" + mapName + ".asset");
        static_data.SetSize((long)(fileInfo.Length / 1000F));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Save Successful!", "Map Name: " + mapName + "\n" +
                                                        "Path: " + path + "\n" +
                                                        "Size: " + (fileInfo.Length / 1000F) + " kb", "Ok");
    }
}
