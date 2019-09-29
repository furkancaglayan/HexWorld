using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HexWorld
{

    [Serializable]
    public class Prop
    {

        [SerializeField] public string path;
        [SerializeField] public Object @object;

        public Prop(string path)
        {
            string correctedPath = path.Replace("\\", "/");
            this.path = correctedPath;
            @object = LoadObject(correctedPath);
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

}