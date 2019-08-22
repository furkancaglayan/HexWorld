using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ChunkList 
{

    [SerializeField]private List<ChunkContainer> chunkContainer;
    public ChunkList()
    {
        chunkContainer = new List<ChunkContainer>();
    }
    public HexWorldChunk Get(int i, int j)
    {
        return chunkContainer[i].GetChunk(j);
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
[System.Serializable]
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
    }
    public List<HexWorldChunk> GetChunkList()
    {
        return hexWorldChunks;
    }


}
