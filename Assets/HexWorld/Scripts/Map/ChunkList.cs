using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#pragma warning disable 0168 
[Serializable]
public class ChunkList 
{

    [SerializeField]private List<ChunkContainer> chunkContainer;
    public ChunkList(int containerSize)
    {
        chunkContainer = new List<ChunkContainer>();
        for (int i = 0; i < containerSize; i++)
            AddContainer();
    }
    public HexWorldChunk Get(int x, int y)
    {
        try
        {
            return chunkContainer[x].GetChunk(y);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return null;
        }
    }
    public void Add(int i, HexWorldChunk chunk)
    {
        chunkContainer[i].AddChunk(chunk);
    }
    public void AddContainer()
    {
        chunkContainer.Add(new ChunkContainer());
    }
   
    public List<ChunkContainer> GetContainers()
    {
        return chunkContainer;
    }

    public int GetTileCount()
    {

        return (int)Mathf.Pow( chunkContainer[0].GetChunkList().Count*20,2);
    }
}
[Serializable]
public class ChunkContainer
{
    [SerializeField] private List<HexWorldChunk> hexWorldChunks;

    public ChunkContainer()
    {
        hexWorldChunks = new List<HexWorldChunk>();
    }

    public HexWorldChunk GetChunk(int j)
    {
        return hexWorldChunks[j];
    }

    public void AddChunk(HexWorldChunk chunk)
    {
        hexWorldChunks.Add(chunk);
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
    public List<HexWorldChunk> GetChunkList()
    {
        return hexWorldChunks;
    }


}
