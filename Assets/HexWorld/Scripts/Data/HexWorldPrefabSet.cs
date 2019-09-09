using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#pragma warning disable 0168 
#pragma warning disable 0219

[Serializable]
public class HexWorldPrefabSet
{
    [SerializeField] public List<HexWorldFolder> folders;
    public string path;

    public HexWorldPrefabSet(string root)
    {
        path = root;
    }

    public bool validIndices(int folderInd, int prefabInd)
    {
        try
        {
            HexWorldPrefab h = Get(folderInd, prefabInd);
            return true;
        }
        catch (ArgumentOutOfRangeException e)
        {
            return false;
        }
        catch (NullReferenceException e)
        {
            return false;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }

    }
    public bool Create()
    {
        bool failed = FailTest();
        if (failed)
            return false;

        folders = CreateDataFolders(path, IconPack.GetHexworldEditorLogo());

        return true;
    }

    private bool FailTest()
    {
        //if passes the test, return false
        if (path.Length < 6)
        {
            //no "Assets"
            Utils.ShowDialog("Directory Exception", "Invalid Directory Path", "Ok");
            return true;
        }

        return !Utils.CheckIfDirectoryIsValid(path,false);
    }

    private List<HexWorldFolder> CreateDataFolders(string root, Texture2D texture)
    {
        string[] folders = Directory.GetDirectories(root);
        List<HexWorldFolder> folderList = new List<HexWorldFolder>();
        foreach (var variable in folders)
            folderList.Add(Factory.create_datafolder(variable, texture));
        folderList.Add(Factory.create_datafolder(root, texture));
        return folderList;
    }

    public GUIContent[] GetFolderContents()
    {
        int size = folders.Count;
        GUIContent[] folderContents = new GUIContent[size];
        for (int i = 0; i < size; i++)
            folderContents[i] = folders[i].content;
        return folderContents;
    }
    public GUIContent[][] GetPrefabContents()
    {
        int size = folders.Count;
        GUIContent[][] prefabContents = new GUIContent[size][];
        for (int i = 0; i < size; i++)
        {
            HexWorldFolder folder = folders[i];
            int prefabCount = folder.Size();

            prefabContents[i] = new GUIContent[prefabCount];
            for (int j = 0; j < prefabCount; j++)
                prefabContents[i][j] = folder.prefabs[j].GetContent();

        }
        return prefabContents;
    }

    public HexWorldPrefab Get(int folder, int prefab)
    {
        return folders[folder].prefabs[prefab];
    }
  
}