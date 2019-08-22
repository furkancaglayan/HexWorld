using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TileList 
{

    [SerializeField] private List<TileContainer> tileContainers;
    public TileList()
    {
        tileContainers = new List<TileContainer>();
    }
    public HexWorldTile Get(int i, int j)
    {
        return tileContainers[i].GetTile(j);
    }
    public void Add(int i, HexWorldTile chunk)
    {
        tileContainers[i].AddTile(chunk);
        
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
[System.Serializable]
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