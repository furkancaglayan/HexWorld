using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#pragma warning disable 0219
#pragma warning disable 0168 
[Serializable]
public class Map 
{
    [SerializeField] public Enums.MapSize mapSize;
    [SerializeField] public Chunk[] chunkList;
    [SerializeField] public float hexRadius;
    [SerializeField] private string _mapName;

    [NonSerialized] public int chunkCount;
    [NonSerialized] public GameObject gameObject;


    public Map(Enums.MapSize mapSize, float hexRadius, Material mat)
    {
        this.mapSize = mapSize;
        this.hexRadius = hexRadius;


        int chunk_capacity = 20;
        chunkCount = DetermineChunkSize((int)mapSize, chunk_capacity);

        Create2DChunkArray(chunkCount, chunk_capacity, hexRadius, (int)mapSize);
        this._mapName = CreateObjectName();
        gameObject = CreateSceneReferences( mat, _mapName, chunkList, chunk_capacity);
    }
 
    /// <summary>
    /// Given a vector3 <paramref name="pos"/>, find the Chunk object that includes it.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Chunk FindChunkWithPos(Vector3 pos)
    {
        foreach (var chunk in chunkList)
            if (chunk.IsLocationInChunk(pos))
                return chunk;
        

        return null;
    }
    /// <summary>
    /// Determines the chunk count of the map. For example for a 40x40 map
    /// this method will return 4 if the chunk capacity is 20
    /// (40x40)/(20x20)=4
    /// </summary>
    /// <param name="mapSize"></param>
    /// <param name="chunk_capacity">How many tiles there should be in a chunk.
    /// if chunk_capacity is 20, then chunk has 20x20 tiles.</param>
    /// <returns></returns>
    private int DetermineChunkSize(int mSize, int chunk_capacity)
    {
        //map is mapSize x mapSize
        int length = Mathf.CeilToInt((float)mSize / chunk_capacity);
        return length;
    }
    /// <summary>
    /// Creates the scene references of the map. This includes
    /// main map gameobject and chunk references.
    /// </summary>
    /// <param name="drawChunk"></param>
    /// <param name="mat"></param>
    /// <param name="map_name"></param>
    /// <param name="chunks"></param>
    /// <param name="chunk_capacity">How many tiles there should be in a chunk.
    /// if chunk_capacity is 20, then chunk has 20x20 tiles.</param>
    /// <returns></returns>
    private GameObject CreateSceneReferences(Material mat, string map_name, Chunk[] lst,
        int chunk_capacity)
    {
        GameObject main = new GameObject(map_name);
        GameObject chunkObj = new GameObject("chunkList");
        chunkObj.transform.parent = main.transform;
        foreach (var chunk in lst)
        {
            GameObject newChunk = chunk.CreateSceneReference(chunk_capacity,  mat, hexRadius);
            newChunk.transform.parent = chunkObj.transform;
            
        }

        return main;
    }
    /// <summary>
    /// Creates 2d array of chunks. Defines their corners, centers and colliders.
    /// </summary>
    /// <param name="chunk_count"></param>
    /// <param name="chunk_capacity"></param>
    /// <param name="hex_radius"></param>
    /// <param name="mapSize"></param>
    /// <returns></returns>
    private void Create2DChunkArray(int chunk_count, int chunk_capacity, float hex_radius, int mapSize)
    {
        float chunk_short = Constants.SHORT_SIDE * chunk_capacity * hex_radius;
        float chunk_long = Constants.LONG_SIDE * chunk_capacity * hex_radius * 3 / 4;

        int cloneMapSize = mapSize * mapSize;
        chunkList = new Chunk[chunk_count * chunk_count];

        for (int i = 0; i < chunk_count; i++)
        {
            for (int j = 0; j < chunk_count; j++)
            {
                Vector3 left_down_corner = new Vector3(chunk_short * i
                    , 0, chunk_long * j);
                //TODO:Add height feature
                Chunk chunk = Factory.create_chunk(chunk_capacity, i, j, left_down_corner, hexRadius);
                chunk.CreateTiles(this);
                AddChunk(i,j, chunk);
            }

        }
    }
    /// <summary>
    /// Returns a unique map name.
    /// </summary>
    /// <returns></returns>
    private string CreateObjectName()
    {
        string time = System.DateTime.UtcNow.ToLocalTime().ToLongTimeString();
        string name = "[HexWorld_" + time + "_" + mapSize.ToString() + "]";

        return name;
    }
    /// <summary>
    /// Creates map corners using mapSize and hexRadius. Dependent on these variables
    /// since there are no parameters. This method is called in HexWorldEditor.
    /// </summary>
    /// <returns></returns>
    public Vector3[] DefineCorners()
    {
        Vector3[] corners = new Vector3[4];

        float @long = (int)mapSize * hexRadius * 3 / 2 + hexRadius * Constants.LONG_SIDE / 4;
        float @short = ((int)mapSize * hexRadius + hexRadius / 2) * Constants.SHORT_SIDE;

        corners[0] = Vector3.zero;
        corners[1] = corners[0] + new Vector3(@short, 0, 0);
        corners[2] = corners[1] + new Vector3(0, 0, @long);
        corners[3] = corners[2] - new Vector3(@short, 0, 0);

        return corners;
    }
    public TileAddress GetNeighbor(Tile tile, int neighborId)
    {
        TileAddress tileAddress = tile.address;

        int chunkX = tileAddress.chunkIdX;
        int chunkY = tileAddress.chunkIdY;

        Chunk chunk = GetChunk(chunkX,chunkY);

        int tileX = tileAddress.idX;
        int tileY = tileAddress.idY;

        try
        {
            if (neighborId == 5)
            {

                if (tileX == 0)
                {
                    if (chunkX == 0)
                        return null;
                    Chunk left = GetChunk(chunkX - 1,chunkY );
                    return left.GetTile(19, tileY).address;
                }
                return chunk.GetTile(tileX - 1,tileY ).address;

            }
            if (neighborId == 2)
            {

                if (tileX == 19)
                {

                    Chunk right = GetChunk(chunkX + 1, chunkY);
                    return right.GetTile(0, tileY).address;
                }
                return chunk.GetTile(tileX + 1, tileY).address;

            }
            if (neighborId == 0)
            {
                if (tileY == 0 && chunkY == 0)
                    return null;

                if (tileX == 0)
                {

                    if (tileY != 0)
                    {
                        Chunk left = GetChunk(chunkX - 1, chunkY); ;
                        if (tileY%2==0)
                            return left.GetTile(19, tileY - 1).address;
                        return chunk.GetTile(tileX, tileY - 1).address;

                    }
                    Chunk leftDown = GetChunk(chunkX - 1, chunkY-1);
                    return leftDown.GetTile(19, 19).address;


                }
                else
                {
                    if (tileY == 0)
                    {
                        Chunk down = GetChunk(chunkX , chunkY - 1);
                        return down.GetTile(tileX-1, 19).address;
                    }
                    if(tileY%2==0)
                        return chunk.GetTile(tileX-1, tileY-1).address;
                    return chunk.GetTile(tileX, tileY - 1).address;
                }


            }
            if (neighborId == 1)
            {
                

                if (tileX == 19)
                {
                    

                    if (tileY != 0)
                    {
                        Chunk right = GetChunk(chunkX + 1, chunkY);
                        
                        if (tileY % 2 != 0)
                            return right.GetTile(0, tileY - 1).address;
                        return chunk.GetTile(tileX, tileY - 1).address;

                    }
                    Chunk down = GetChunk(chunkX, chunkY - 1);
                    return down.GetTile(19, 19).address;


                }
                else
                {
                    if (tileY == 0)
                    {
                        Chunk down = GetChunk(chunkX, chunkY - 1);
                        return down.GetTile(tileX, 19).address;
                    }
                    if (tileY % 2 == 0)
                        return chunk.GetTile(tileX, tileY - 1).address;
                    return chunk.GetTile(tileX+1, tileY - 1).address;
                }


            }
            if (neighborId == 4)
            {
                if (tileY == 19 && chunkY == chunkCount-1)
                    return null;

                if (tileX == 0)
                {
                    

                    if (tileY != 19)
                    {
                        Chunk left = GetChunk(chunkX - 1, chunkY); ;
                        if (tileY % 2 == 0)
                            return left.GetTile(19, tileY + 1).address;
                        return chunk.GetTile(tileX, tileY + 1).address;

                    }
                    Chunk top = GetChunk(chunkX, chunkY + 1);
                    return top.GetTile(0, 0).address;


                }
                else
                {
                    if (tileY == 19)
                    {
                        Chunk top = GetChunk(chunkX, chunkY +1);
                        return top.GetTile(tileX , 0).address;
                    }
                    if (tileY % 2 == 0)
                        return chunk.GetTile(tileX - 1, tileY + 1).address;
                    return chunk.GetTile(tileX, tileY + 1).address;
                }


            }
            if (neighborId == 3)
            {
                if (tileY == 19 && chunkY == chunkCount - 1)
                    return null;

                if (tileX == 19)
                {
                    

                    if (tileY != 19)
                    {
                        Chunk right = GetChunk(chunkX + 1, chunkY); ;
                        if (tileY % 2 != 0)
                            return right.GetTile(0, tileY + 1).address;
                        return chunk.GetTile(tileX, tileY + 1).address;

                    }
                    Chunk topR = GetChunk(chunkX+1, chunkY + 1);
                    return topR.GetTile(0, 0).address;


                }
                else
                {
                    if (tileY == 19)
                    {
                        Chunk top = GetChunk(chunkX, chunkY + 1);
                        return top.GetTile(tileX+1, 0).address;
                    }
                    if (tileY % 2 == 0)
                        return chunk.GetTile(tileX , tileY + 1).address;
                    return chunk.GetTile(tileX+1, tileY + 1).address;
                }


            }
            return null;
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
    public Tile GetTile(TileAddress tileAddress)
    {
        int chunkX = tileAddress.chunkIdX;
        int chunkY = tileAddress.chunkIdY;

        int tileX = tileAddress.idX;
        int tileY = tileAddress.idY;

        return GetChunk(chunkX,chunkY).GetTile(tileX, tileY);
    }
   

    public bool IsEmpty()
    {
        foreach (var chunk in chunkList)
            if (!chunk.IsEmpty()) return false;
        return true;
    }
   

    public List<Tile> GetTilesInRadius(Tile centerTile, int radius)
    {
        
        if (radius == 1)
            return new List<Tile>{centerTile};
        if(radius == 2)
        {
            List<Tile> tiles = new List<Tile>
            {
                centerTile
            };
            for (int i = 0; i < 6; i++)
            {
                TileAddress address = centerTile.neighbors[i];
                if (address != null)
                    tiles.Add(GetTile(address));
            }
            return tiles;
        }
        List<Tile> lst = new List<Tile>(GetTilesInRadius(centerTile, radius-1));
        List<Tile> lstCopy = new List<Tile>(lst);
        foreach (var tile in lst)
        {
            for (int i = 0; i < 6; i++)
            {
                TileAddress address = tile.neighbors[i];
                if(address!=null)
                    if(!lst.Contains(GetTile(address)))
                        lstCopy.Add(GetTile(address));
            }
        }

        return lstCopy;
    }
    public Chunk GetChunk(int x, int y)
    {
        try
        {
            return chunkList[chunkCount * x + y];
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

    public void AddChunk(int i, int j, Chunk chunk) => chunkList[i * chunkCount + j] = chunk;

}
