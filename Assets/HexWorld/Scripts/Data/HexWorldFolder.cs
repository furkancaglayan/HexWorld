using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HexWorldFolder
{

    private string path;
    private string shortName;

    private List<HexWorldPrefab> prefabs;
    private Texture2D contentTexture;

    private GUIContent folderContent;
    public HexWorldFolder(string path,Texture2D contentTexture)
    {
        this.path = path;
        this.contentTexture = contentTexture;

        string tooltip;
        shortName = CreateShortenedFolderName(this.path, 6, out tooltip);
        folderContent = CreateContent(shortName, this.contentTexture, tooltip);

        prefabs = CreatePrefabs(this.path, this.contentTexture);

    }

    public GUIContent GetFolderContent()
    {
        return folderContent;
    }

    public int Size()
    {
        return prefabs.Count;
    }
    private GUIContent CreateContent(string name,Texture2D texture,string tooltip)
    {
        return new GUIContent(name, texture, tooltip);
    }

    public List<HexWorldPrefab> GetPrefabs()
    {
        return prefabs;
    }

    private List<HexWorldPrefab> CreatePrefabs(string folderPath,Texture2D texture)
    {
        string[] files = Directory.GetFiles(folderPath);
        List<HexWorldPrefab> hexWorldPrefabs = new List<HexWorldPrefab>();
        foreach (var VARIABLE in files)
            if (!VARIABLE.Contains(".meta")&& VARIABLE.Contains(".prefab"))
                    hexWorldPrefabs.Add(Factory.create_prefab(VARIABLE));

        return hexWorldPrefabs;

    }
    /// <summary>
    /// Create a shortened name for a given folder path. For example
    /// 'Plain Tiles' is 11 character long. return name will be
    /// 'Plain Ti..' , thus will be shortened.
    /// </summary>
    /// <param name="folderPath">folder path to separate and shorten</param>
    /// <param name="separator">separator to use-> \\ for paths</param>
    /// <param name="tooltip">out tooltip variable->full name of the folder</param>
    /// <returns></returns>
    private string CreateShortenedFolderName(string folderPath,int length,out string tooltip)
    {   string[] splitPath= folderPath.Split('\\');

        string[] split = splitPath[splitPath.Length-1].Split('/');
        string folder = split[split.Length - 1];
        tooltip = folder;

        if (folder.Length > length)
            folder = folder.Substring(0, length - 2) + "..";
        return folder;
    }
}
