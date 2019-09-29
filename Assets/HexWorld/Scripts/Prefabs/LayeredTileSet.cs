using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LayeredTileSet : TileSet
{

    [SerializeField] public PropFolder[] layers;

    public void CreateLayers(string[] paths)
    {
        PropFolder A = Factory.CreatePropFolder("Tile Layer");
        PropFolder B = Factory.CreatePropFolder("First Layer");
        PropFolder C = Factory.CreatePropFolder("Second Layer");
        PropFolder D = Factory.CreatePropFolder("Third Layer");

        layers = new[] {A, B, C, D};

        for (int i = 0; i < 4; i++)
            layers[i].Create(paths[i]);
    }
    public override int GetPropCount()
    {
        if (layers == null)
            return 0;
        int count = 0;
        foreach (var layer in layers)
            if (layer != null)
                count += layer.props.Count;
        return count;
    }


    public override GUIContent[] GetFolderContents()
    {
        return new []
        {
            new GUIContent("Tile Layer"),
            new GUIContent("First Layer"),
            new GUIContent("Second Layer"),
            new GUIContent("Third Layer")

        };
    }

    public override GUIContent[] GetFileContentsInFolder(int folderIndex)
    {
        if (folderIndex > 3)
            return null;
        return layers[folderIndex].GetContents();
    }
}
