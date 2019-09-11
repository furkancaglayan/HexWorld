using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Prop
{

    [SerializeField] public string path;
    [SerializeField] public Object @object;

    public Prop(string path)
    {
        this.path = path;
        @object = LoadObject(path);
    }

    private Object LoadObject(string objPath)
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath(objPath, typeof(Object));
#else 
        return null;
#endif
    }
}
