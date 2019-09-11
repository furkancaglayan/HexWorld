using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class _EditorSaveLoadUtility 
{
    public static void SaveMap(string path, string mapName, Map map)
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


        HexWorldSerialized static_data = ScriptableObject.CreateInstance<HexWorldSerialized>();
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

    public static void SavePrefab(string gameObjectName, string mapsDirectory, Map map)
    {
        bool valid = _EditorUtility.CheckIfDirectoryIsValid("Assets/" + mapsDirectory, false);
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
        asset = PrefabUtility.CreatePrefab(path, copy);

#endif
        Object.DestroyImmediate(copy);
        if (asset != null)
            Selection.activeObject = asset;
    }
}
