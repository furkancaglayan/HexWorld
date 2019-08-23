using System;
using UnityEngine;
using Object = UnityEngine.Object;

#pragma warning disable 0219
[Serializable]
public class HexWorldMap
{
    [SerializeField] public Enums.MapSize mapSize;
    [SerializeField] public ChunkList chunkList;
    [SerializeField] public GameObject gameObject;

    [SerializeField] private float hexRadius;
    [SerializeField] private string mapName;
    


    public HexWorldMap(Enums.MapSize mapSize, float hexRadius, Material mat)
    {
        this.mapSize = mapSize;
        this.hexRadius = hexRadius;


        //find chunk size, constant chunk capacity is 20x20.
        int chunk_capacity = 20;
        int chunkCount = DetermineChunkSize((int)mapSize, chunk_capacity);

        //init chunks
        chunkList = Create2DChunkArray(chunkCount, chunk_capacity, hexRadius, (int)mapSize);

        mapName = CreateObjectName();
        gameObject = CreateSceneReferences( mat, mapName, chunkList, chunk_capacity);
    }

    public HexWorldMap(HexWorldStaticData staticData, Material mat)
    {
        var copy = Object.Instantiate(staticData);
        this.mapSize =copy.GetMapData().mapSize;
        this.hexRadius = copy.GetMapData().hexRadius;


        //find chunk size, constant chunk capacity is 20x20.
        int chunk_capacity = 20;

        //init chunks
        chunkList = copy.GetMapData().chunkList;

        mapName = copy.GetMapData().mapName;
        gameObject = CreateSceneReferences( mat, mapName, chunkList, chunk_capacity);
        Renew();


    }


    public HexWorldChunk FindChunkWithPos(Vector3 pos)
    {
        foreach (var lst in chunkList.GetContainers())
            foreach (var VARIABLE in lst.GetChunkList())
                if (VARIABLE.LocationInChunk(pos))
                    return VARIABLE;
        

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
    private GameObject CreateSceneReferences(Material mat, string map_name, ChunkList chunks,
        int chunk_capacity)
    {
        GameObject main = new GameObject(map_name);
        GameObject chunkObj = new GameObject("chunkList");
        chunkObj.transform.parent = main.transform;
        foreach (var lst in chunks.GetContainers())
        {
            foreach (var VARIABLE in lst.GetChunkList())
            {
                GameObject newChunk = VARIABLE.CreateSceneReference(chunk_capacity,  mat, hexRadius);
                newChunk.transform.parent = chunkObj.transform;
            }
            
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
    private ChunkList Create2DChunkArray(int chunk_count, int chunk_capacity, float hex_radius, int mapSize)
    {
        float chunk_short = Constants.SHORT_SIDE * chunk_capacity * hex_radius;
        float chunk_long = Constants.LONG_SIDE * chunk_capacity * hex_radius * 3 / 4;

        int cloneMapSize = mapSize * mapSize;
        ChunkList arr = new ChunkList();
      

        for (int i = 0; i < chunk_count; i++)
        {
            arr.AddContainer();
            for (int j = 0; j < chunk_count; j++)
            {
                if (cloneMapSize < 0)
                    break;

                Vector3 left_down_corner = new Vector3(chunk_short * i
                    , 0, chunk_long * j);
                //TODO:Add height feature
                HexWorldChunk hexWorldChunk = Factory.create_chunk(chunk_capacity, i, j, left_down_corner, hexRadius);
                hexWorldChunk.CreateTiles();
                arr.Add(i, hexWorldChunk);
                cloneMapSize -= chunk_capacity * chunk_capacity;
            }

            if (cloneMapSize < 0)
                break;
        }

        return arr;
    }

    /// <summary>
    /// Returns a unique map name.
    /// </summary>
    /// <returns></returns>
    private string CreateObjectName()
    {
        //TODO:Remove time and add id based name
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

    public void FillMap(HexWorldPrefab prefab, Enums.RotationType rotationType, bool randomRot)
    {
        foreach (var lst in chunkList.GetContainers())
        foreach (var VARIABLE in lst.GetChunkList())
                VARIABLE.Fill(prefab, rotationType, randomRot);
    }
    public void FillEmptyTiles(HexWorldPrefab prefab, Enums.RotationType rotationType, bool randomRot)
    {
        foreach (var lst in chunkList.GetContainers())
            foreach (var VARIABLE in lst.GetChunkList())
                VARIABLE.FillEmpty(prefab, rotationType, randomRot);
    }

    public bool isEmpty()
    {
        foreach (var lst in chunkList.GetContainers())
            foreach (var VARIABLE in lst.GetChunkList())
                if (!VARIABLE.isEmpty()) return false;
        return true;
    }
    
    public void Renew()
    {
        foreach (var lst in chunkList.GetContainers())
            foreach (var VARIABLE in lst.GetChunkList())
                    VARIABLE.Renew();
    }
}
