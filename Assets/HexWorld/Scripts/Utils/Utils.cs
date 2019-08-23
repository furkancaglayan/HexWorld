using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if UNITY_POST_PROCESSING_STACK_V2 
using UnityEngine.Rendering.PostProcessing;
#endif
#pragma warning disable 0168 

public static class Utils  {

   
    /// <summary>
    /// Returns the file number in a folder(not subfolders) with the given extension.
    /// Meta files are excluded.
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    private static int GetFileCountInFolder(string folderPath, string extension)
    {
        string[] rootFilePaths = Directory.GetFiles(folderPath);

        int objectCount = 0;
        for (int i = 0; i < rootFilePaths.Length; i++)
        {
            if (rootFilePaths[i].Contains(".meta") || !rootFilePaths[i].Contains(extension))
                continue;
            objectCount++;
        }

        return objectCount;

    }
    /// <summary>
    /// Adds the given tag parameter to project.
    /// </summary>
    /// <param name="tag"></param>
    public static void AddTag(string tag)
    {
#if UNITY_EDITOR
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
#endif

    }
    public static void AddLayer(string layer)
    {
#if UNITY_EDITOR
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

#endif
    }

    public static void FixPPDefineSymbolBug()
    {
#if UNITY_EDITOR
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/ProjectSettings.asset");
        if (asset != null)
        {
            var so = new SerializedObject(asset);
            var properties = so.FindProperty("scriptingDefineSymbols");
            var numProp = properties.arraySize;

            if (numProp == 0)
                return;
            properties.DeleteArrayElementAtIndex(0);

            Debug.Log("Fİx");


            so.ApplyModifiedProperties();
            so.Update();
        }
#endif

    }

    public static void SaveMap(string path, string mapName, HexWorldMap map)
    {


#if UNITY_EDITOR
        string fullPath = path + "/" + mapName + ".asset";
        bool fileExists = System.IO.File.Exists(fullPath);
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


        var fileInfo = new System.IO.FileInfo(path + "/" + mapName + ".asset");
        static_data.SetSize((long)(fileInfo.Length / 1000F));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Save Successful!", "Map Name: " + mapName + "\n" +
                                                        "Path: " + path + "\n" +
                                                        "Size: " + (fileInfo.Length / 1000F)+" kb", "Ok");
#endif
    }

    public static bool CheckIfDirectoryIsValid(string path,bool showFinalDialog)
    {

        string title = "";
        string message = "";
        bool retVal = false;

        try
        {
            Directory.GetFiles(path);
            retVal = true;
        }
        catch (DirectoryNotFoundException e)
        {
            title = "Directory Exception";
            message = e.Message + "\n" + e.HelpLink;
            retVal= false;
        }
        catch (UnauthorizedAccessException e)
        {
            title = "Directory Exception";
            message = "Invalid Directory Path";
            retVal = false;
        }
        catch (ArgumentException e)
        {
            title = "Directory Exception";
            message = e.Message + "\n" + e.HelpLink;
            retVal = false;

        }
        catch (IOException e)
        {
            title = "Directory Exception";
            message = "Invalid Directory Path";
            retVal = false;
            
        }
#if UNITY_EDITOR
        if(!retVal)
            EditorUtility.DisplayDialog(title,message , "Ok");
        else if(showFinalDialog)
            EditorUtility.DisplayDialog("Valid","Directory seems to be valid." , "Ok");
#endif

        return retVal;

    }


    public static string CreateName(int length)
    {
        string alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        string alphanumericUpper = alphanumeric.ToUpper();

        alphanumeric += alphanumericUpper;
        string name = "";
        for (int i = 0; i < length; i++)
        {
            int randomInt = Random.Range(0, alphanumeric.Length);
            name += alphanumeric[randomInt].ToString();
        }

        return "Hexworld_"+name;

    }

    public static void SavePrefab(string gameObjectName, string mapsDirectory, HexWorldMap map)
    {
#if UNITY_EDITOR
        bool valid = CheckIfDirectoryIsValid("Assets/"+mapsDirectory,false);
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
        if (map.isEmpty())
        {
            EditorUtility.DisplayDialog("Map is Empty", "Add some tiles first.", "Ok");
            return;
        }
        string fullPath = "Assets/" + mapsDirectory + "/" + gameObjectName + ".prefab";



        GameObject prefab = map.gameObject;

        GameObject copy = GameObject.Instantiate(prefab);
        try
        {
            Transform chunks = copy.transform.GetChild(0);
            for (int i = 0; i < chunks.childCount; i++)
            {
                Transform chunk = chunks.GetChild(i);
                GameObject mesh = chunk.GetChild(1).gameObject;
                if(mesh.name.Contains("Mesh"))
                    GameObject.DestroyImmediate(mesh);

                //also delete the collider
                Collider col = chunk.GetComponent<Collider>();
                GameObject.DestroyImmediate(col);


            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
        Object asset = null;
#if UNITY_2018_3_OR_NEWER
        asset=PrefabUtility.SaveAsPrefabAsset(copy,fullPath);
#else
        asset = PrefabUtility.CreatePrefab(fullPath, copy);

#endif
        GameObject.DestroyImmediate(copy);
        if(asset!=null)
            Selection.activeObject = asset;
#endif
    }

    public static Camera CreateCamera()
    {
        Camera current = Camera.main;
        if (current == null)
            current = (Camera)GameObject.FindObjectOfType(typeof(Camera));
        if (current == null)
        {
            GameObject newGO = new GameObject("HexWorld_Camera", typeof(Camera));
            current = newGO.GetComponent<Camera>();
        }
        current.name = "[HexWorld_Camera]";
        current.tag = "MainCamera";
        return current;

    }

#if UNITY_POST_PROCESSING_STACK_V2
    public static void AddEffect(HexWorldEffect effect)
    {
        if (effect == null)
        {
            EditorUtility.DisplayDialog("Null Exception", "Effect is empty.", "Ok");
            return;
        }

        Camera camera = CreateCamera();
        ClearEffectsAndLights();
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

        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;


        //add lights
        if (effect.lights.Count == 0)
            return;
        GameObject lightObj = new GameObject("[HexWorld_Lights]");
        for (int i = 0; i < effect.lights.Count; i++)
            effect.AddLight(effect.lights[i], lightObj.transform);

    }
#endif
    private static void ClearEffectsAndLights()
    {
        GameObject lights=GameObject.Find("[HexWorld_Lights]");
        GameObject.DestroyImmediate(lights);

        GameObject effects = GameObject.Find("[HexWorld_Effects]");
        GameObject.DestroyImmediate(effects);
    }

    public static void AddCameraController(float minHeight, float maxHeight, float rotSpeed, float Speed, float ScrollSensitivity)
    {
        Camera camera = CreateCamera();
        CameraController controller = camera.GetComponent<CameraController>();
        if(controller==null)
            controller = camera.gameObject.AddComponent<CameraController>();
        controller.Set_Opt(minHeight, maxHeight, rotSpeed, Speed, ScrollSensitivity);
#if UNITY_EDITOR
        Selection.activeGameObject = camera.gameObject;
#endif

    }
}
