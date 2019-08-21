using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#pragma warning disable 0168 
#pragma warning disable 0219

public class HexWorldPrefabSet
{
    private List<HexWorldFolder> DataFolders;
    private string rootPath;

    public HexWorldPrefabSet(string root)
    {
        rootPath = root;
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
        
    }
    public bool Create()
    {
        bool failed = FailTest();
        if (failed)
            return false;

        DataFolders = CreateDataFolders(rootPath, IconPack.GetHexworldEditorLogo());

        return true;
    }

    private bool FailTest()
    {
        //if passes the test, return false
        if (rootPath.Length < 6)
        {
            //no "Assets"
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Directory Exception", "Invalid Directory Path", "Ok");
#endif
            return true;
        }

        return !Utils.CheckIfDirectoryIsValid(rootPath,false);
    }

    private List<HexWorldFolder> CreateDataFolders(string root, Texture2D texture)
    {
        string[] folders = Directory.GetDirectories(root);
        List<HexWorldFolder> folderList = new List<HexWorldFolder>();
        foreach (var VARIABLE in folders)
        {
            folderList.Add(Factory.create_datafolder(VARIABLE, texture));


        }

        return folderList;
    }

    public GUIContent[] GetFolderContents()
    {
        int size = DataFolders.Count;
        GUIContent[] folderContents = new GUIContent[size];
        for (int i = 0; i < size; i++)
            folderContents[i] = DataFolders[i].GetFolderContent();
        return folderContents;
    }
    public GUIContent[][] GetPrefabContents()
    {
        int size = DataFolders.Count;
        GUIContent[][] prefabContents = new GUIContent[size][];
        for (int i = 0; i < size; i++)
        {
            HexWorldFolder folder = DataFolders[i];
            int prefabCount = folder.Size();

            prefabContents[i] = new GUIContent[prefabCount];
            for (int j = 0; j < prefabCount; j++)
                prefabContents[i][j] = folder.GetPrefabs()[j].GetContent();

        }
        return prefabContents;
    }

    public HexWorldPrefab Get(int folder, int prefab)
    {
        return DataFolders[folder].GetPrefabs()[prefab];
    }
    //silinebilir
    public GameObject[][] GetPrefabGameObjects()
    {
        int size = DataFolders.Count;
        GameObject[][] prefabs = new GameObject[size][];
        for (int i = 0; i < size; i++)
        {
            HexWorldFolder folder = DataFolders[i];
            int prefabCount = folder.Size();

            prefabs[i] = new GameObject[prefabCount];
            for (int j = 0; j < prefabCount; j++)
                prefabs[i][j] = folder.GetPrefabs()[j].GetGameObject();

        }
        return prefabs;
    }
}