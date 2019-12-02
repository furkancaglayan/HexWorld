using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexWorldPrefab 
{

    private string fullPath;
    private string shortName;
    private string toolTip;
    private Texture2D texture;

    private Object objectReference;

    private GUIContent prefabContent;

    public HexWorldPrefab(string fullPath)
    {

        this.fullPath = fullPath;
        toolTip = this.fullPath;
        toolTip=toolTip.Replace('\\','/');

        objectReference = LoadObject(fullPath);
        shortName = CreateShortName(objectReference);
#if UNITY_EDITOR
        texture = AssetPreview.GetAssetPreview(objectReference);
#endif
        prefabContent = new GUIContent(shortName, texture);
    }
    public GUIContent GetContent()
    {
        return prefabContent;
    }

    private string CreateShortName(Object obj)
    {
        return obj.name;
    }

    private Object LoadObject(string path)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
#else
        return null;
#endif
    }

    public GameObject GetGameObject()
    {
        return objectReference as GameObject;
    }
    public Object GetObject()
    {
        return objectReference;
    }

    public Texture2D GetTexture()
    {
        return texture;

    }
}
