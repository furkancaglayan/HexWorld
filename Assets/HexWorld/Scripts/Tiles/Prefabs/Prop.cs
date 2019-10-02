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
            this.path = path.Replace("\\", "/"); ;
             
        }

        public virtual void LoadObject()
        {
#if UNITY_EDITOR
            @object= UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Object));
#else
        @object null;
#endif
        }
    }

}