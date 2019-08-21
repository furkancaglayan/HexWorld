using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0414 

[System.Serializable]
public class HexWorldTile
{
    [System.Serializable]
    public struct HexWorldEdge
    {
        [SerializeField] private Vector3 PointA;
        [SerializeField] private Vector3 PointB;

        public HexWorldEdge(Vector3 PointA, Vector3 PointB)
        {
            this.PointA = PointA;
            this.PointB = PointB;
        }

        public void Create(Vector3 PointA, Vector3 PointB)
        {
            this.PointA = PointA;
            this.PointB = PointB;
        }

        public Vector3[] GetPoints()
        {
            return new[] {PointA , PointB};
        }

        public bool Equals(HexWorldEdge other)
        {
            Vector3[] other_points = other.GetPoints();
            Vector3 otherA = other_points[0], otherB=other_points[1];

            if (otherA.Equals(PointA) && otherB.Equals(PointB))
                return true;
            if (otherB.Equals(PointA) && otherA.Equals(PointB))
                return true;
            return false;
        }
    }


    [SerializeField] private Vector3 center;
    [SerializeField] private float radius;
    [SerializeField] private int mID_X, mID_Y;

    [System.NonSerialized] private GameObject tileGameObject;
    [SerializeField] private Quaternion tileRotation = Quaternion.identity;

    //use this for saving
    [SerializeField] private Object @tileReference;

    [SerializeField] private bool isFull;


    //[SerializeField] private NeighborInfo _tileNeighborInfo;
    [System.NonSerialized]private HexWorldChunk _ownerChunk;


    //[SerializeField] private HexWorldEdge[] _tileEdges;


    [SerializeField] private Vector3[] _corners;

    public HexWorldTile(HexWorldChunk _ownerChunk,int mID_X, int mID_Y, Vector3 center,float radius)
    {
        this.radius = radius;
        this.center = center;
        this._ownerChunk = _ownerChunk;
        this.mID_Y = mID_Y;
        this.mID_X = mID_X;
        //create address
        //create corners
        _corners = CreateCorners(center,radius);
        //create edges
        //_tileEdges = CreateEdges(_corners);
    }

  

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

    private HexWorldEdge[] CreateEdges(Vector3[] corners)
    {
        HexWorldEdge[] edges = new HexWorldEdge[6];
        for (int i = 0; i < 6; i++)
        {
            Vector3 pt1 = corners[i % 6];
            Vector3 pt2 = corners[(i+1) % 6];
            edges[i] = new HexWorldEdge(pt1,pt2);
        }
        return edges;
    }

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
    public GameObject PlacePrefab(HexWorldPrefab prefab,float rotation,Enums.RotationType rotationType)
    {
        RemovePrefab();
        
        GameObject go = Object.Instantiate(prefab.GetGameObject());
        go.transform.position = center;
        tileGameObject = go;

        Rotate(rotation, rotationType);
        @tileReference = prefab.GetObject();
        isFull = true;

        tileGameObject.transform.parent = _ownerChunk.GetChunkTileObject().transform;
        return tileGameObject;

    }

    public void RemovePrefab()
    {
        isFull = false;
        @tileReference = null;
        if (tileGameObject)
            Object.DestroyImmediate(tileGameObject);
        else
            tileGameObject = null;

    }
    #region Getter-Setter
    /*public HexWorldEdge[] GetEdges()
    {
        return _tileEdges;
    }*/


    public bool isEmpty()
    {
        return !isFull;
    }
    public Vector3 GetCenter()
    {
        return center;
    }

    public Vector3[] GetCorners()
    {
        return _corners;
    }

    public GameObject GetGameObject()
    {
        return tileGameObject;
    }
    #endregion

    public void Renew(HexWorldChunk owner)
    {
        _ownerChunk = owner;
        if (!isFull)
            return;

        GameObject go = GameObject.Instantiate(@tileReference as GameObject);
        go.transform.position = center;
        go.transform.rotation = tileRotation;
        tileGameObject = go;

        tileGameObject.transform.parent = _ownerChunk.GetChunkTileObject().transform;
    }
}
