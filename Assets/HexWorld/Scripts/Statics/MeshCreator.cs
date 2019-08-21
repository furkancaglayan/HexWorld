using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public static class MeshCreator  {

    public static GameObject CreateMesh(HexWorldChunk chunk, Material mat,out GameObject mesh)
    {
        GameObject @object = chunk.GetChunkObject();
        GameObject tiles = CreateTileMeshes(chunk,mat);
        tiles.transform.parent = @object.transform;


        mesh = tiles;
        return @object;
    }

    private static GameObject CreateTileMeshes(HexWorldChunk chunk,Material mat)
    {
        TileList tiles = chunk.GetTiles();
        HexWorldTile current_tile = null;
        Vector3[] currentCorners = null;


        int capacity = chunk.GetChunkCapacity();

        

        GameObject @object = new GameObject("Mesh");
        MeshFilter @meshFilter = @object.AddComponent<MeshFilter>();
        MeshRenderer @meshRenderer = @object.AddComponent<MeshRenderer>();
        List<int> @indices = new List<int>();
        List<Vector3> @vertices = new List<Vector3>();

        meshRenderer.materials = new[] { mat };
        for (int i = 0; i < capacity; i++)
        {
            for (int j = 0; j < capacity; j++)
            {

                current_tile = tiles.Get(j, i);
                if (current_tile==null)
                    continue;
                currentCorners = current_tile.GetCorners();
                

                for (int k = 0; k < 6; k++)
                {
                    Vector3 p = currentCorners[k];
                    @vertices.Add(p);
                    @indices.Add((i*capacity+j)*6 + k);
                    if (k < 5)
                        @indices.Add((i * capacity + j) * 6 + k + 1);
                    else
                        @indices.Add((i * capacity + j) * 6);
                }




            }   
        }
        Mesh @mesh = new Mesh();
        @meshFilter.mesh = @mesh;
        @mesh.vertices = vertices.ToArray();

        @mesh.SetIndices(@indices.ToArray(), MeshTopology.Lines, 0);
        @mesh.RecalculateBounds();

        Graphics.DrawMeshNow(@mesh, Vector3.zero, Quaternion.identity);


        return @object;


    }

    


}
