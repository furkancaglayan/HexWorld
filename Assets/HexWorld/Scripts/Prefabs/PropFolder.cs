using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HexWorld
{

    [Serializable]
    public class PropFolder : IContentHolder
    {
        [SerializeField] public string name;
        [SerializeField, HideInInspector] public string path;
        [SerializeField] public List<Prop> props;


        public PropFolder(string name) => this.name = name;

        public void Create(string path)
        {
            string correctedPath = path.Replace("\\", "/");
            this.path = correctedPath;

            string[] files = Directory.GetFiles(correctedPath);
            props = new List<Prop>();
            foreach (var variable in files)
                if (!variable.Contains(".meta") && variable.Contains(".prefab"))
                    props.Add(Factory.CreateProp(variable));
        }

        public void SetProps(List<Prop> newProps) => props = newProps;
        public void DeleteProp(int index) => props.RemoveAt(index);

        public GUIContent[] GetContents()
        {
            GUIContent[] contents = new GUIContent[props.Count];
            for (int i = 0; i < props.Count; i++)
            {
#if UNITY_EDITOR
                contents[i] = new GUIContent(props[i].@object.name, UnityEditor.AssetPreview.GetAssetPreview(props[i].@object));
#else
             contents[i] = new GUIContent(props[i].@object.name);
#endif
            }
            return contents;
        }
    }

}