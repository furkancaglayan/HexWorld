using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class HexWorldPrefab 
{

    public string path;
    public string shortName;
    public string toolTip;
    public Texture texture;

    public Object @object;
    public GUIContent content;

    public GameObject GameObject => @object as GameObject;

    public HexWorldPrefab(string path)
    {

        this.path = path;
        toolTip = this.path;
        toolTip=toolTip.Replace('\\','/');

        @object = LoadObject(path);
        shortName = @object.name;
#if UNITY_EDITOR
        texture = AssetPreview.GetAssetPreview(@object);
#endif
        content = new GUIContent(shortName, texture);
    }


    private Object LoadObject(string path)
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
#else
        return null;
#endif
    }



 
}
