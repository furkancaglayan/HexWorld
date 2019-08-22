using System;
using UnityEngine;
#pragma warning disable 0414 
[System.Serializable]
public sealed class HexWorldChunk
{
    [SerializeField] public int capacity { get;}
    [SerializeField] public int id_x { get;}
    [SerializeField] public int id_y { get;}
    [SerializeField] public Vector3[] chunkCorners { get; set; }
    [SerializeField] public TileList tileList { get; set; }
    [SerializeField] public float hex_radius { get;}

    [NonSerialized] public Collider chunkCollider;
    [NonSerialized] public GameObject chunkGameObject;
    [NonSerialized] public GameObject chunkTileObject;
    [NonSerialized] public GameObject meshGameObject;



public HexWorldChunk(int capacity, int id_x, int id_y, Vector3 left_down_corner, float hex_radius)
    {
        this.capacity = capacity;
        this.id_x = id_x;
        this.id_y = id_y;
        this.hex_radius = hex_radius;

        //create corners
        chunkCorners[0] = left_down_corner;
        chunkCorners[1] = left_down_corner +
                          new Vector3(
                              Constants.SHORT_SIDE * capacity * hex_radius + Constants.SHORT_SIDE * hex_radius / 2, 0,
                              0);
        chunkCorners[2] = left_down_corner +
                          new Vector3(
                              Constants.SHORT_SIDE * capacity * hex_radius + Constants.SHORT_SIDE * hex_radius / 2, 0,
                              Constants.LONG_SIDE * capacity * hex_radius * 3 / 4 + hex_radius / 2);
        chunkCorners[3] = left_down_corner +
                          new Vector3(0, 0, Constants.LONG_SIDE * capacity * hex_radius * 3 / 4 + hex_radius / 2);

       
    }
    /// <summary>
    /// Creates scene representation of the HexWorldChunk.
    /// Creates a gameobject, draws tiles, creates mesh and collider.
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="hexRadius"></param>
    /// <returns></returns>
    public GameObject CreateSceneReference(Material mat, float hexRadius)
    {
        chunkGameObject = new GameObject("Chunk_" + id_x.ToString() + id_y.ToString())
        {
            tag = "HexWorld"
        };


        chunkTileObject = new GameObject("Tiles");

        chunkTileObject.transform.SetParent(chunkGameObject.transform);

        chunkGameObject.transform.position = GetChunkCenter();
        chunkGameObject = MeshCreator.CreateMesh(this, mat,out meshGameObject);

        chunkCollider = GenerateCollider(chunkGameObject);

        


        return chunkGameObject;
    }

    /// <summary>
    /// Create tiles for the chunk.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="corners"></param>
    /// <param name="hex_radius"></param>
    /// <returns>2d list of tiles.</returns>
    public void CreateTiles()
    {
        TileList lst = new TileList();

        for (int i = 0; i < capacity; i++)
        {
            lst.AddContainer();
            for (int j = 0; j < capacity; j++)
            {
                Vector3 center;
                if (i % 2 == 0)
                    center = chunkCorners[0] + new Vector3(
                                 Constants.SHORT_SIDE * hex_radius / 2 + j * Constants.SHORT_SIDE * hex_radius
                                 , 0, hex_radius * i * (1.5F)) + new Vector3(0, 0, hex_radius);
                else
                    center = chunkCorners[0] + new Vector3(
                                 Constants.SHORT_SIDE * hex_radius + j * Constants.SHORT_SIDE * hex_radius
                                 , 0, hex_radius * i * (1.5F)) + new Vector3(0, 0, hex_radius);
                lst.Add(i,Factory.create_tile(this, j, i, center, hex_radius));
            }
        }

        tileList= lst;
    }

    /// <summary>
    /// Creates a box collider and adds it to the "obj" which is also chunk. 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    private Collider GenerateCollider(GameObject obj)
    {
        BoxCollider boxCollider = obj.AddComponent<BoxCollider>();

        float x = chunkCorners[1].x - chunkCorners[0].x;
        float y = .1f;
        float z = chunkCorners[2].z - chunkCorners[0].z;

        boxCollider.center = new Vector3(id_x * (x / 2 - (Constants.SHORT_SIDE / 2) * hex_radius), 0,
            id_y * (z / 2 - (Constants.LONG_SIDE / 4) * hex_radius));
        boxCollider.size = new Vector3(x, y, z);
        return boxCollider;
    }

    public bool LocationInChunk(Vector3 pos)
    {
        if (chunkCorners[0].x < pos.x && chunkCorners[1].x > pos.x)
            if (chunkCorners[0].z < pos.z && chunkCorners[2].z > pos.z)
                return true;
        return false;
    }

    public void Fill(HexWorldPrefab prefab, Enums.RotationType rotationType, bool randomRot)
    {
        foreach (var lst in tileList.GetContainers())
            foreach (var VARIABLE in lst.GetTileList())
                HexWorldBrush.ApplySimpleStroke(Enums.BrushType.Place, VARIABLE, prefab, randomRot, rotationType);
    }
    public void FillEmpty(HexWorldPrefab prefab, Enums.RotationType rotationType, bool randomRot)
    {
        foreach (var lst in tileList.GetContainers())
            foreach (var VARIABLE in lst.GetTileList())
                if (VARIABLE.isFull)
                    HexWorldBrush.ApplySimpleStroke(Enums.BrushType.Place, VARIABLE, prefab, randomRot, rotationType);
    }

    public HexWorldTile Position2Tile(Vector3 pos)
    {
        Vector3 center;
        float dist = 99999;
        HexWorldTile obj = null;
        foreach (var lst in tileList.GetContainers())
        {
            foreach (var o in lst.GetTileList())
            {
                center = o.center;
                float val = Vector3.Distance(center, pos);
                if (val < dist)
                {
                    dist = val;
                    obj = o;
                }
            }
        }
          



        return obj;
    }
    public bool isEmpty()
    {
        foreach (var lst in tileList.GetContainers())
            foreach (var VARIABLE in lst.GetTileList())
                if (VARIABLE.isFull)
                    return false;
        return true;
    }
    public Vector3 GetChunkCenter()
    {
        float x = (chunkCorners[1].x - chunkCorners[0].x) / 2;
        float z = (chunkCorners[2].z - chunkCorners[0].z) / 2;
        // Vector3 center = new Vector3(x * id_x + x / 2 - id_x * hex_radius, 0, z * id_y + z / 2);

        Vector3 center = new Vector3((id_x + 1) * x, 0, (id_y + 1) * z);
        return center;
    }

    public void RenewChunk()
    {
        foreach (var lst in tileList.GetContainers())
            foreach (var VARIABLE in lst.GetTileList())
                    VARIABLE.RenewTile(this);
    }
}