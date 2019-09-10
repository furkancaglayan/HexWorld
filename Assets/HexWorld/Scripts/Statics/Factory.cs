using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Factory  {


    public static HexWorldTile create_tile(HexWorldChunk chunk,int idx, int idy, Vector3 center, float hexRadius)
    {
        HexWorldTile tile= new HexWorldTile(chunk, idx, idy, center, hexRadius);

        tile.infrastructure = new Infrastructure();

        return tile;
    }
    public static HexWorldMap create_map(Enums.MapSize mapSize, float hexRad, Material mat)
    {

        return new HexWorldMap(mapSize, hexRad, mat);
    }
    public static HexWorldMap create_map(HexWorldSerialized serialized, Material mat)
    {

        return new HexWorldMap(serialized, mat);
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
