using UnityEngine;
using System;
using Object = UnityEngine.Object;
#pragma warning disable 0414

[System.Serializable]
public sealed class HexWorldTile
{
   
    [System.Serializable]
    public struct HexWorldEdge
    {
        ///
        [SerializeField] private Vector3 PointA;
        [SerializeField] private Vector3 PointB;

        /// <summary>
        /// Defines the edges of the tiles.
        /// </summary>
        /// <param name="PointA">Point one</param>
        /// <param name="PointB">Point two</param>
        public HexWorldEdge(Vector3 PointA, Vector3 PointB)
        {
            this.PointA = PointA;
            this.PointB = PointB;
        }
        /// <summary>
        /// Given 2 points, sets the edges.
        /// </summary>
        /// <param name="PointA">Point one</param>
        /// <param name="PointB">Point two</param>
        public void Create(Vector3 PointA, Vector3 PointB)
        {
            this.PointA = PointA;
            this.PointB = PointB;
        }
        /// <summary>
        /// Returns the points
        /// </summary>
        /// <returns>2 element array. First point and second point</returns>
        public Vector3[] GetPoints()
        {
            return new[] {PointA , PointB};
        }
        /// <summary>
        /// Checks equality of 2 edges.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool Equals(HexWorldEdge edge)
        {
            Vector3[] other_points = edge.GetPoints();
            Vector3 otherA = other_points[0], otherB=other_points[1];

            if (otherA.Equals(PointA) && otherB.Equals(PointB))
                return true;
            if (otherB.Equals(PointA) && otherA.Equals(PointB))
                return true;
            return false;
        }
    }


    [SerializeField] public Vector3 center { get; set; }
    [SerializeField] public Vector3[] _corners { get; set; }
    [SerializeField] public float radius { get; set; }
    [SerializeField] public int mID_X { get; set; }
    [SerializeField] public int mID_Y{ get; set; }
    [SerializeField] public Quaternion tileRotation { get; set; }
    [SerializeField] public Object @tileReference { get; set; }
    [SerializeField] public bool isFull { get; set; }

    [NonSerialized] public HexWorldChunk _ownerChunk;
    [NonSerialized] public GameObject tileGameObject;


    /// <summary>
    /// Default tile constructor.
    /// </summary>
    /// <param name="_ownerChunk"></param>
    /// <param name="mID_X"></param>
    /// <param name="mID_Y"></param>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    public HexWorldTile(HexWorldChunk _ownerChunk,int mID_X, int mID_Y, Vector3 center,float radius)
    {
        this.radius = radius;
        this.center = center;
        this._ownerChunk = _ownerChunk;
        this.mID_Y = mID_Y;
        this.mID_X = mID_X;
        _corners = CreateCorners(center,radius);
    }
    #region Behaviours
    /// <summary>
    /// Creates corners of the tile with given center and radius. 
    /// </summary>
    /// <param name="center">Center of the tile</param>
    /// <param name="radius">Radius of the tile</param>
    /// <returns></returns>
    private Vector3[] CreateCorners(Vector3 center,float radius)
    {
        Vector3[] crs = new Vector3[6];
        crs[0] = center - new Vector3(radius*Mathf.Sqrt(3)/2,0,.5f*radius);
        crs[1] = center - new Vector3(0, 0, radius);
        crs[2] = center - new Vector3(-radius * Mathf.Sqrt(3) / 2, 0, .5f * radius);
        crs[3] = center + new Vector3(+radius * Mathf.Sqrt(3) / 2, 0, .5f * radius);
        crs[4] = center + new Vector3(0, 0, radius);
        crs[5] = center - new Vector3(radius * Mathf.Sqrt(3) / 2, 0, -.5f * radius);
        return crs;
    }
    /// <summary>
    /// Creates edges by connecting the corners.
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    private HexWorldEdge[] CreateEdges(Vector3[] corners)
    {
        HexWorldEdge[] edges = new HexWorldEdge[6];
        for (int i = 0; i < 6; i++)
        {
            Vector3 pt1 = corners[i % 6];
            Vector3 pt2 = corners[(i + 1) % 6];
            edges[i] = new HexWorldEdge(pt1, pt2);
        }
        return edges;
    }
    /// <summary>
    /// Rotates tile tile's game object by given <paramref name="rotation"/> value
    /// </summary>
    /// <param name="rotation">Degrees to rotate</param>
    /// <param name="rotationType">Rotation axis</param>
    public void Rotate(float rotation,Enums.RotationType rotationType)
    {
        if (tileGameObject == null)
            return;
        switch (rotationType)
        {
            case Enums.RotationType.X:
                tileGameObject.transform.Rotate(Vector3.right, rotation);
                break;
            case Enums.RotationType.Y:
                tileGameObject.transform.Rotate(Vector3.up, rotation);
                break;
            case Enums.RotationType.Z:
                tileGameObject.transform.Rotate(Vector3.forward, rotation);
                break;

        }

        tileRotation = tileGameObject.transform.rotation;
    }
    /// <summary>
    /// Creates and places the prefab gameobject on the tile.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="rotation"></param>
    /// <param name="rotationType"></param>
    /// <returns></returns>
    public GameObject PlacePrefab(HexWorldPrefab prefab,float rotation,Enums.RotationType rotationType)
    {
        RemovePrefab();
        
        GameObject go = Object.Instantiate(prefab.GetGameObject());
        go.transform.position = center;
        tileGameObject = go;

        Rotate(rotation, rotationType);
        @tileReference = prefab.GetObject();
        isFull = true;

        tileGameObject.transform.parent = _ownerChunk.chunkTileObject.transform;
        return tileGameObject;

    }
    /// <summary>
    /// Removes the game object of tile thus empties it.
    /// </summary>
    public void RemovePrefab()
    {
        isFull = false;
        @tileReference = null;
        if (tileGameObject)
            Object.DestroyImmediate(tileGameObject);
        else
            tileGameObject = null;

    }
    /// <summary>
    /// Renews the loaded tile by creating the gameobject of the tile.
    /// </summary>
    /// <param name="owner">Parent chunk of the tile.</param>
    public void RenewTile(HexWorldChunk owner)
    {
        _ownerChunk = owner;
        if (!isFull)
            return;

        GameObject go = GameObject.Instantiate(@tileReference as GameObject);
        go.transform.position = center;
        go.transform.rotation = tileRotation;
        tileGameObject = go;

        tileGameObject.transform.parent = _ownerChunk.chunkTileObject.transform;
    }
    #endregion
    

  
}
