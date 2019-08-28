using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HexWorldFolder
{
    public string folderPath;
    public string tooltip;

    public Texture2D texture;
    public GUIContent content;

    public List<HexWorldPrefab> prefabs;

    public HexWorldFolder(string folderPath,Texture2D texture)
    {
        this.folderPath = folderPath;
        this.texture = texture;

        string shortName = CreateShortenedFolderName(this.folderPath, 6, out tooltip);
        content = CreateContent(shortName, this.texture, tooltip);

        prefabs = CreatePrefabs(this.folderPath, this.texture);

    }



    public int Size()
    {
        return prefabs.Count;
    }
    private GUIContent CreateContent(string name,Texture2D texture,string tooltip)
    {
        return new GUIContent(name, texture, tooltip);
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
    /// Create a shortened name for a given folder folderPath. For example
    /// 'Plain Tiles' is 11 character long. return name will be
    /// 'Plain Ti..' , thus will be shortened.
    /// </summary>
    /// <param name="folderPath">folder folderPath to separate and shorten</param>
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
