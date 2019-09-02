using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0168 

[Serializable]
public class TileList 
{

    [SerializeField] private List<TileContainer> tileContainers;
    public TileList(int containerCount)
    {
        tileContainers = new List<TileContainer>();
        for (int i = 0; i < containerCount; i++)
            AddContainer();
    }
    public HexWorldTile Get(int x, int y)
    {
        try
        {
            
            return tileContainers[y].GetTile(x);
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
    
    public HexWorldTile Add(int i, HexWorldTile tile)
    {
        tileContainers[i].AddTile(tile);
        return tile;
        
    }

    
    public void AddContainer()
    {
       
        tileContainers.Add(new TileContainer());
    }

    public List<TileContainer> GetContainers()
    {
        return tileContainers;
    }
}
[Serializable]
public class TileContainer 
{
    [SerializeField] private List<HexWorldTile> hexWorldTiles;

    public TileContainer()
    {
        hexWorldTiles = new List<HexWorldTile>();
    }

    public HexWorldTile GetTile(int j)
    {
        return hexWorldTiles[j];
    }

    public void AddTile(HexWorldTile tile)
    {
        hexWorldTiles.Add(tile);
    }
    public List<HexWorldTile> GetTileList()
    {
        return hexWorldTiles;
    }


}