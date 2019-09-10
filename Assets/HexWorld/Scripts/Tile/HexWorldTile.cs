using System;
using UnityEngine;
using Object=UnityEngine.Object;
#pragma warning disable 0414 
#pragma warning disable 0168 
[Serializable]
public class HexWorldTile
{
    


    [SerializeField] public Vector3 center;
    [SerializeField] public float radius;
    [SerializeField] public int idX, idY;
    [SerializeField] public Vector3[] corners;
    [SerializeField] public TileAddress[] neighbors;


    [NonSerialized] public GameObject gameObject;
    [NonSerialized] public TileAddress address;
    [NonSerialized] public HexWorldChunk chunk;


    [SerializeField] private bool _isFull;
    [SerializeField] private Object _object;
    [SerializeField] private Quaternion _rotation = Quaternion.identity;

    [SerializeField] public Infrastructure infrastructure;
    public bool IsEmpty => !_isFull;



    public HexWorldTile(HexWorldChunk chunk, int idX, int idY, Vector3 center,float radius)
    {
        this.radius = radius;
        this.center = center;
        this.chunk = chunk;
        this.idY = idY;
        this.idX = idX;
        //create address
        //create corners
        corners = CreateCorners(center,radius);
        //create edges
        //_tileEdges = CreateEdges(corners);
        neighbors = new TileAddress[6];
        address = CreateAddress();

    }
    #region PUBLIC

    /// <summary>
    /// Rotates the hexagon by given <paramref name="rotation"/> around _rotation axis <paramref name="rotationType"/>
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="rotationType"></param>
    public void Rotate(float rotation,Enums.RotationType rotationType)
    {
        if (gameObject == null)
            return;
        switch (rotationType)
        {
            case Enums.RotationType.X:
                gameObject.transform.Rotate(Vector3.right, rotation);
                break;
            case Enums.RotationType.Y:
                gameObject.transform.Rotate(Vector3.up, rotation);
                break;
            case Enums.RotationType.Z:
                gameObject.transform.Rotate(Vector3.forward, rotation);
                break;

        }

        this._rotation = gameObject.transform.rotation;
    }
    /// <summary>
    /// Places the given <paramref name="prefab"/> on the tile
    /// </summary>
    /// <param name="prefab">prefab</param>
    /// <param name="owner"></param>
    /// <param name="rotation"></param>
    /// <param name="rotationType"></param>
    /// <returns></returns>
    public GameObject PlacePrefab(HexWorldPrefab prefab,HexWorldChunk owner,float rotation,Enums.RotationType rotationType)
    {
        RemovePrefab();
        SetOwnerChunk(owner);

     



        GameObject go = Object.Instantiate(prefab.GameObject);
        go.transform.position = center;
        gameObject = go;

        Rotate(rotation, rotationType);
        _object = prefab.GameObject;
        _isFull = true;

        gameObject.transform.parent = chunk.tileObject.transform;
        return gameObject;

    }
    /// <summary>
    /// Removes the prefab from the tile.
    /// </summary>
    public void RemovePrefab()
    {
        _isFull = false;
        _object = null;
        if (gameObject)
            Object.DestroyImmediate(gameObject);
        else
            gameObject = null;

    }

    /// <summary>
    /// Sets the given <paramref name="owner"/> as the tiles owner.
    /// </summary>
    /// <param name="owner"></param>
    public void SetOwnerChunk(HexWorldChunk owner) => chunk = owner;
    /// <summary>
    /// Renews the tile by placing the _object again.
    /// </summary>
    public void Renew()
    {
        
        if (!_isFull)
            return;
        GameObject go = Object.Instantiate(_object as GameObject);
        go.transform.position = center;
        go.transform.rotation = _rotation;
        gameObject = go;

        gameObject.transform.parent = chunk.tileObject.transform;
    }
    /// <summary>
    /// Checks the neighboring tiles and add them to the list
    /// if they are available.
    /// </summary>
    /// <param name="map"></param>
    public void UpdateNeighbors(HexWorldMap map)
    {
       //left neighbor
       for (int i = 0; i < 6; i++)
       {
           TileAddress tileAddress= map.GetNeighbor(this, i);
           if(tileAddress != null)
               SetNeighbor(tileAddress, i, map);
       }
        
    }


    public override string ToString() => "idX : " + idX.ToString() + " - idY : " + idY.ToString();
    #endregion
    #region PRIVATE

    /// <summary>
    /// Given a <paramref name="neighborAddress"/>, sets it this tiles neighbor.
    /// </summary>
    /// <param name="neighborAddress"></param>
    /// <param name="neighborId"></param>
    /// <param name="map"></param>
    private void SetNeighbor(TileAddress neighborAddress, int neighborId, HexWorldMap map)
    {
        try
        {
            neighbors[neighborId] = neighborAddress;
            map.GetTile(neighborAddress).neighbors[(neighborId + 3) % 6] = address;
        }
        catch (ArgumentException e)
        {
        }
        catch (NullReferenceException e)
        {
        }
    }
    /// <summary>
    /// Given a <paramref name="center"/> and a <paramref name="radius"/>, creates the corners of the hexagon.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns>vec3 array of corners</returns>
    private Vector3[] CreateCorners(Vector3 center, float radius)
    {
        Vector3[] crs = new Vector3[6];
        crs[0] = center - new Vector3(radius * Mathf.Sqrt(3) / 2, 0, .5f * radius);
        crs[1] = center - new Vector3(0, 0, radius);
        crs[2] = center - new Vector3(-radius * Mathf.Sqrt(3) / 2, 0, .5f * radius);
        crs[3] = center + new Vector3(+radius * Mathf.Sqrt(3) / 2, 0, .5f * radius);
        crs[4] = center + new Vector3(0, 0, radius);
        crs[5] = center - new Vector3(radius * Mathf.Sqrt(3) / 2, 0, -.5f * radius);
        return crs;
    }
    /// <summary>
    /// Creates the address of the tile.
    /// </summary>
    /// <returns></returns>
    private TileAddress CreateAddress()
    {
        return new TileAddress(chunk.idX, chunk.idY, idX, idY);
    }
    #endregion

}
