using System;
using UnityEngine;
using Object=UnityEngine.Object;
#pragma warning disable 0414 

[Serializable]
public class HexWorldTile
{
    [Serializable]
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


    [SerializeField] public Vector3 center;
    [SerializeField] public float radius;
    [SerializeField] public int idX, idY;

    [NonSerialized] public GameObject gameObject;
    [SerializeField] private Quaternion rotation = Quaternion.identity;

    [SerializeField] private Object @object;

    [SerializeField] public bool isFull;


    [NonSerialized]private HexWorldChunk chunk;




    [SerializeField] public Vector3[] corners;

    public HexWorldTile(HexWorldChunk chunk,int idX, int idY, Vector3 center,float radius)
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

        this.rotation = gameObject.transform.rotation;
    }
    public GameObject PlacePrefab(HexWorldPrefab prefab,float rotation,Enums.RotationType rotationType)
    {
        RemovePrefab();
        
        GameObject go = Object.Instantiate(prefab.GetGameObject());
        go.transform.position = center;
        gameObject = go;

        Rotate(rotation, rotationType);
        @object = prefab.GetObject();
        isFull = true;

        gameObject.transform.parent = chunk.gameObject.transform;
        return gameObject;

    }

    public void RemovePrefab()
    {
        isFull = false;
        @object = null;
        if (gameObject)
            Object.DestroyImmediate(gameObject);
        else
            gameObject = null;

    }
   

    public void Renew(HexWorldChunk owner)
    {
        chunk = owner;
        if (!isFull)
            return;

        GameObject go = GameObject.Instantiate(@object as GameObject);
        go.transform.position = center;
        go.transform.rotation = rotation;
        gameObject = go;

        gameObject.transform.parent = chunk.tileObject.transform;
    }
}
