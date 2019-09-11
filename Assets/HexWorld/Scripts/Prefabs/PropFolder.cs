using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PropFolder
{
    [SerializeField,HideInInspector] public string path;
    [SerializeField] public List<Prop> props;

    public PropFolder()
    {

    }

    public void Create(string path)
    {
        this.path = path;


        string[] files = Directory.GetFiles(path);
        props = new List<Prop>();
        foreach (var variable in files)
            if(!variable.Contains(".meta")&& variable.Contains(".prefab"))
                props.Add(Factory.CreateProp(variable));
    }

    public void SetProps(List<Prop> newProps)
    {
        this.props = newProps;
    }

}
