using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public class CombinedTileSet : TileSet
{
    [SerializeField] public List<PropFolder> folders;
    public void LoadPrefabs(string path)
    {
        folders = CreateDataFolders(path);
    }
    private List<PropFolder> CreateDataFolders(string root)
    {
        string[] folders = Directory.GetDirectories(root);
        List<PropFolder> folderList = new List<PropFolder>();
        foreach (var variable in folders)
            if(RuntimeUtility.GetFileCountInFolder(variable,".prefab")!=0)
                folderList.Add(Factory.CreatePropFolder(variable,variable));
        if (RuntimeUtility.GetFileCountInFolder(root, ".prefab") != 0)
            folderList.Add(Factory.CreatePropFolder(root,root));
        return folderList;
    }


    public string[] GetFolderNames()
    {
        string[] names = new string[folders.Count];
        int index = 0;
        foreach (var VARIABLE in folders)
            names[index++] = VARIABLE.path.Split('/')[VARIABLE.path.Split('/').Length - 1];
        return names;
    }

    public override int GetPropCount()
    {
        int count = 0;
        foreach (var VARIABLE in folders)
            count += VARIABLE.props.Count;
        return count;
    }

    public override GUIContent[] GetFolderContents()
    {
        List<GUIContent> contents = new List<GUIContent>();
        for (int i = 0; i < folders.Count; i++)
            contents.Add(new GUIContent(folders[i].name));
        return contents.ToArray();
    }
}

