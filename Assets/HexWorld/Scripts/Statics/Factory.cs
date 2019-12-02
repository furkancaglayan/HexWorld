using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Factory  {


    public static Tile create_tile(Chunk chunk,int idx, int idy, Vector3 center, float hexRadius)
    {
        Tile tile = new Tile(chunk, idx, idy, center, hexRadius)
        {
            layerWrapper = new LayerWrapper()
        };

        return tile;
    }
    public static Map create_map(Enums.MapSize mapSize, float hexRad, Material mat)
    {

        return new Map(mapSize, hexRad, mat);
    }
    /*public static Map create_map(HexWorldSerialized serialized, Material mat)
    {

        return new Map(serialized, mat);
    }*/

    public static Prop CreateProp(string path)
    {
        return new Prop(path);
    }
    public static PropFolder CreatePropFolder(string path)
    {
        PropFolder folder = new PropFolder();
        folder.Create(path);
        return folder;
    }
    public static PropFolder CreatePropFolder()
    {
        PropFolder folder = new PropFolder();
        return folder;
    }
    public static Chunk create_chunk(int chunkSize, int id_x, int id_y, Vector3 left_down_corner, float hex_radius)
    {
        return new Chunk(chunkSize, id_x, id_y, left_down_corner, hex_radius);
    }
  
}
