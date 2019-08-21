using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Factory  {


    public static HexWorldTile create_tile(HexWorldChunk chunk,int idx, int idy, Vector3 center, float hexRadius)
    {
            return new HexWorldTile(chunk, idx, idy, center, hexRadius);
    }
    public static HexWorldMapData create_map(Enums.MapSize mapSize, float hexRad, Material mat)
    {

        return new HexWorldMapData(mapSize, hexRad, mat);
    }
    public static HexWorldMapData create_map(HexWorldStaticData staticData, Material mat)
    {

        return new HexWorldMapData(staticData, mat);
    }

    public static HexWorldChunk create_chunk(int chunkSize, int id_x, int id_y, Vector3 left_down_corner, float hex_radius)
    {
        return new HexWorldChunk(chunkSize, id_x, id_y, left_down_corner, hex_radius);
    }
    public static HexWorldFolder create_datafolder(string path, Texture2D texture)
    {
        return new HexWorldFolder(path, texture);
    }
    public static HexWorldPrefab create_prefab(string path)
    {
        return new HexWorldPrefab(path);
    }
    public static HexWorldPrefabSet create_dataset(string path)
    {
        return new HexWorldPrefabSet(path);
    }
}
