using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0414 

[Serializable]
public class Chunk 
{
    [SerializeField] public int capacity;
    [SerializeField] public int idX;
    [SerializeField] public int idY;
    [SerializeField] public float gridRadius;
    [NonSerialized] public GameObject gameObject;
    [NonSerialized] public GameObject tileObject;
    [SerializeField] public Tile[] tiles;


    [SerializeField] private Vector3[] _chunkCorners;
    [NonSerialized] private Collider _collider;
    [NonSerialized] private GameObject _mesh;



    public Chunk(int chunk_size, int id_x, int id_y, Vector3 leftDownCorner, float gridRadius)
    {
        this.capacity = chunk_size;
        this.idX = id_x;
        this.idY = id_y;
        this.gridRadius = gridRadius;

        
        CreateCorners(leftDownCorner, capacity);


    }
    /// <summary>
    /// Given a chunk size and a reference corner, creates the chunk corners.
    /// </summary>
    /// <param name="left_down_corner"></param>
    /// <param name="chunkSize"></param>
    private void CreateCorners(Vector3 leftDownCorner,int chunkSize)
    {
        _chunkCorners = new Vector3[4];
        _chunkCorners[0] = leftDownCorner;
        _chunkCorners[1] = leftDownCorner +
                           new Vector3(
                               Constants.SHORT_SIDE * chunkSize * gridRadius + Constants.SHORT_SIDE * gridRadius / 2, 0,
                               0);
        _chunkCorners[2] = leftDownCorner +
                           new Vector3(
                               Constants.SHORT_SIDE * chunkSize * gridRadius + Constants.SHORT_SIDE * gridRadius / 2, 0,
                               Constants.LONG_SIDE * chunkSize * gridRadius * 3 / 4 + gridRadius / 2);
        _chunkCorners[3] = leftDownCorner +
                           new Vector3(0, 0, Constants.LONG_SIDE * chunkSize * gridRadius * 3 / 4 + gridRadius / 2);
    }
    /// <summary> 
    /// Creates scene representation of the Chunk.
    /// Creates a gameobject, draws tiles, creates _mesh and _collider.
    /// </summary>
    /// <param name="chunkCapacity"></param>
    /// <param name="mat"></param>
    /// <param name="hexRadius"></param>
    /// <returns></returns>
    public GameObject CreateSceneReference(int chunkCapacity, Material mat, float hexRadius)
    {
        gameObject = new GameObject("Chunk_" + idX.ToString() + idY.ToString())
        {
            tag = "HexWorld"
        };


        tileObject = new GameObject("Tiles");

        tileObject.transform.SetParent(gameObject.transform);

        gameObject.transform.position = GetChunkCenter();
        gameObject = RuntimeUtility.CreateMesh(this, mat,out _mesh);

        _collider = GenerateCollider(gameObject);

        


        return gameObject;
    }
    /// <summary>
    /// Create tiles for the chunk.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="corners"></param>
    /// <param name="hex_radius"></param>
    /// <returns>2d list of tiles.</returns>
    public void CreateTiles(Map map)
    {
        tiles = new Tile[capacity * capacity];

        for (int i = 0; i < capacity; i++)
        {
            
            for (int j = 0; j < capacity; j++)
            {
                Vector3 center;
                if (i % 2 == 0)
                    center = _chunkCorners[0] + new Vector3(
                                 Constants.SHORT_SIDE * gridRadius / 2 + j * Constants.SHORT_SIDE * gridRadius
                                 , 0, gridRadius * i * (1.5F)) + new Vector3(0, 0, gridRadius);
                else
                    center = _chunkCorners[0] + new Vector3(
                                 Constants.SHORT_SIDE * gridRadius + j * Constants.SHORT_SIDE * gridRadius
                                 , 0, gridRadius * i * (1.5F)) + new Vector3(0, 0, gridRadius);

                Tile tile = Factory.create_tile(this, j, i, center, gridRadius);
                tile= AddTile(j,i, tile);
            }
        }

        for (int i = 0; i < capacity; i++)
        {
            for (int j = 0; j < capacity; j++)
            {
                Tile t = GetTile(i,j);
                t.UpdateNeighbors(map);
            }
        }

    }
    /// <summary>
    /// Creates a box _collider and adds it to the "obj" which is also chunk. 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    private Collider GenerateCollider(GameObject obj)
    {
        BoxCollider boxCollider = obj.AddComponent<BoxCollider>();

        float x = _chunkCorners[1].x - _chunkCorners[0].x;
        float y = .1f;
        float z = _chunkCorners[2].z - _chunkCorners[0].z;

        boxCollider.center = new Vector3(idX * (x / 2 - (Constants.SHORT_SIDE / 2) * gridRadius), 0,
            idY * (z / 2 - (Constants.LONG_SIDE / 4) * gridRadius));
        boxCollider.size = new Vector3(x, y, z);
        return boxCollider;
    }
    /// <summary>
    /// Given a vector3 <paramref name="pos"/>, checks if this vector3 is in bounds of the chunk. 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Returns true if in bounds</returns>
    public bool IsLocationInChunk(Vector3 pos)
    {
        if (_chunkCorners[0].x < pos.x && _chunkCorners[1].x > pos.x)
            if (_chunkCorners[0].z < pos.z && _chunkCorners[2].z > pos.z)
                return true;
        return false;
    }
   
    /// <summary>
    /// Given a vector3 <paramref name="pos"/>, returns the closest tile.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Tile Position2Tile(Vector3 pos)
    {
        Vector3 center;
        float dist = 99999;
        Tile obj = null;
        foreach (var o in tiles)
        {
            center = o.center;
            float val = Vector3.Distance(center, pos);
            if (val < dist)
            {
                dist = val;
                obj = o;
            }
        }
        return obj;
    }
   
    /// <summary>
    /// Calculates the center of the chunk
    /// </summary>
    /// <returns>Center point of the chunk as vec3</returns>
    public Vector3 GetChunkCenter()
    {
        float x = (_chunkCorners[1].x - _chunkCorners[0].x) / 2;
        float z = (_chunkCorners[2].z - _chunkCorners[0].z) / 2;
        // Vector3 center = new Vector3(x * idX + x / 2 - idX * gridRadius, 0, z * idY + z / 2);

        Vector3 center = new Vector3((idX + 1) * x, 0, (idY + 1) * z);
        return center;
    }
    public bool IsEmpty()
    {
        foreach (var tile in tiles)
            if (!tile.IsEmpty)
                return false;
        return true;
    }
    public Tile GetTile(int x, int y)
    {
        try
        {

            return tiles[x * 20 + y];
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

    private Tile AddTile(int i, int j, Tile tile)
    {
        tiles[i * 20 + j] = tile;
        return tiles[i * 20 + j];
    }


}