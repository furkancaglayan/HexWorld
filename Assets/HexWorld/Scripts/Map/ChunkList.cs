using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#pragma warning disable 0168 
[Serializable]
public class ChunkList 
{


    [SerializeField]public HexWorldChunk[] list;
    [SerializeField] public int containerSize;
    public ChunkList(int containerSize)
    {
        this.containerSize = containerSize;
        list = new HexWorldChunk[containerSize * containerSize];
    }
    public HexWorldChunk Get(int x, int y)
    {
        try
        {
            return list[containerSize * x + y];
        }
        catch (ArgumentOutOfRangeException e)
        {
            return null;
        }
        catch (IndexOutOfRangeException e)
        {
            return null;
        }
    }

    public void Add(int i, int j, HexWorldChunk chunk) => list[i * containerSize + j] = chunk;
   

}
