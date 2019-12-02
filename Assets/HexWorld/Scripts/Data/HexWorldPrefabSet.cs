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
    #region PUBLIC
    public bool ValidIndices(int folderInd, int prefabInd)
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
    /// <summary>
    /// Creates the data set.
    /// </summary>
    /// <returns></returns>
    public bool Create()
    {
        bool FailTest()
        {
            //if passes the test, return false
            if (path.Length < 6)
            {
                //no "Assets"
                Utils.ShowDialog("Directory Exception", "Invalid Directory Path", "Ok");
                return true;
            }

            return !Utils.CheckIfDirectoryIsValid(path, false);
        }
        bool failed = FailTest();
        if (failed)
            return false;

        folders = CreateDataFolders(path, IconPack.GetHexworldEditorLogo());

        return true;
    }



    /// <summary>
    /// Returns a 1d array of GUIContents. It's made of folder contents.
    /// </summary>
    /// <returns></returns>
    public GUIContent[] GetFolderContents()
    {
        int size = folders.Count;
        GUIContent[] folderContents = new GUIContent[size];
        for (int i = 0; i < size; i++)
            folderContents[i] = folders[i].content;
        return folderContents;
    }
    /// <summary>
    /// Creates and returns a 2d array of GUIContents.
    /// Rows are prefabs and columns are folders. It can be used to make a selection grid of prefabs.
    /// </summary>
    /// <returns></returns>
    public GUIContent[][] GetPrefabContents()
    {
        int size = folders.Count;
        GUIContent[][] prefabContents = new GUIContent[size][];
        for (int i = 0; i < size; i++)
        {
            HexWorldFolder folder = folders[i];
            int prefabCount = folder.Size;

            prefabContents[i] = new GUIContent[prefabCount];
            for (int j = 0; j < prefabCount; j++)
                prefabContents[i][j] = folder.prefabs[j].content;

        }
        return prefabContents;
    }

    /// <summary>
    /// Get a prefab by its <paramref name="folder"/> and <paramref name="prefab"/> index.
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public HexWorldPrefab Get(int folder, int prefab)
    {
        return folders[folder].prefabs[prefab];
    }
    /// <summary>
    /// Get a folder by its <paramref name="folder"/> index.
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    public HexWorldFolder Get(int folder)
    {
        return folders[folder];
    }
    #endregion
    #region PRIVATE
    /// <summary>
    /// Creates Data folders.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="texture"></param>
    /// <returns></returns>
    private List<HexWorldFolder> CreateDataFolders(string root, Texture2D texture)
    {
        string[] folders = Directory.GetDirectories(root);
        List<HexWorldFolder> folderList = new List<HexWorldFolder>();
        foreach (var variable in folders)
            folderList.Add(Factory.create_datafolder(variable, texture));
        folderList.Add(Factory.create_datafolder(root, texture));
        return folderList;
    }
    #endregion
}