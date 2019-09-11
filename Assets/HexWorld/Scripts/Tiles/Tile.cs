using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;
#pragma warning disable 0414 
#pragma warning disable 0168 
[Serializable]
public class Tile
{
    


    [SerializeField] public Vector3 center;
    [SerializeField] public int idX, idY;
    [SerializeField] public Vector3[] corners;
    [SerializeField] public TileAddress[] neighbors = new TileAddress[6];


    [NonSerialized] public TileAddress address;
    [NonSerialized] public Map map;


    [SerializeField] private bool _isFull;
    [SerializeField] public LayerWrapper layerWrapper;
    public bool IsEmpty => !_isFull;





    public Tile(Chunk chunk, int idX, int idY, Vector3 center,float radius)
    {
        this.center = center;
        this.idY = idY;
        this.idX = idX;
        corners = CreateCorners(center,radius);
        address = CreateAddress(chunk);
        

    }
    #region PUBLIC
    /// <summary>
    /// Checks the neighboring tiles and add them to the list
    /// if they are available.
    /// </summary>
    /// <param name="map"></param>
    public void UpdateNeighbors(Map map)
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
    private void SetNeighbor(TileAddress neighborAddress, int neighborId, Map map)
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
    private TileAddress CreateAddress(Chunk chunk)
    {
        return new TileAddress(chunk.idX, chunk.idY, idX, idY);
    }
    #endregion

}
