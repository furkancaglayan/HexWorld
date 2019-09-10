using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0168 

[Serializable]
public class TileList 
{

    [SerializeField] public HexWorldTile[] list;
    public TileList(int containerCount)
    {
        list = new HexWorldTile[containerCount * containerCount];
       
    }
    public HexWorldTile Get(int x, int y)
    {
        try
        {

            return list[x * 20 + y];
        }
        catch (NullReferenceException e)
        {
            return null;
        }
        catch (ArgumentOutOfRangeException e)
        {
            return null;
        }
    }

    internal HexWorldTile Add(int i, int j, HexWorldTile hexWorldTile)
    {
        list[i * 20 + j] = hexWorldTile;
        return list[i * 20 + j];
    }
}
