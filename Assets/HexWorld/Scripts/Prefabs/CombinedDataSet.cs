using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public class CombinedDataSet : Dataset
{
    [SerializeField] public List<PropFolder> folders;

    public void LoadPrefabs(string path,bool singleFolder)
    {
        folders = singleFolder?CreateSingleDataFolder(path): CreateMultipleDataFolders(path);
    }
    private List<PropFolder> CreateMultipleDataFolders(string root)
    {
        string[] folders = Directory.GetDirectories(root);
        List<PropFolder> folderList = new List<PropFolder>();
        foreach (var variable in folders)
            if(RuntimeUtility.GetFileCountInFolder(variable,".prefab")!=0)
                folderList.Add(Factory.CreatePropFolder(variable));
        if (RuntimeUtility.GetFileCountInFolder(root, ".prefab") != 0)
            folderList.Add(Factory.CreatePropFolder(root));
        return folderList;
    }

    private List<PropFolder> CreateSingleDataFolder(string root)
    {
        string[] files = Directory.GetFiles(root);
        List<Prop> propList = new List<Prop>();
        foreach (var variable in files)
            propList.Add(Factory.CreateProp(variable));

        List<PropFolder> propFolders = new List<PropFolder>();
        PropFolder rootFolder = Factory.CreatePropFolder();
        //set props to the rootFolder;
        propFolders.Add(rootFolder);

        return propFolders;
    }

}
