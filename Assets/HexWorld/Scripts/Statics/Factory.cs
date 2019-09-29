using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{

    public static class Factory
    {


        public static Tile create_tile(Chunk chunk, int idx, int idy, Vector3 center, float hexRadius)
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
        public static LayeredTileSet CreateLayeredTileSet(string name, string ecosystem, string[] paths)
        {
            LayeredTileSet set = (LayeredTileSet)ScriptableObject.CreateInstance(typeof(LayeredTileSet));
            set.name = name;
            set.ecosystem = ecosystem;
            set.CreateLayers(paths);
            return set;
        }
        public static CombinedTileSet CreateCombinedTileSet(string name, string ecosystem, string path)
        {
            CombinedTileSet set = (CombinedTileSet)ScriptableObject.CreateInstance(typeof(CombinedTileSet));
            set.ecosystem = ecosystem;
            set.name = name;
            set.LoadPrefabs(path);
            return set;
        }
        public static Prop CreateProp(string path)
        {
            return new Prop(path);
        }
        public static PropFolder CreatePropFolder(string name, string path)
        {
            PropFolder folder = new PropFolder(name);
            folder.Create(path);
            return folder;
        }
        public static PropFolder CreatePropFolder(string name)
        {
            PropFolder folder = new PropFolder(name);
            return folder;
        }
        public static Chunk create_chunk(int chunkSize, int id_x, int id_y, Vector3 left_down_corner, float hex_radius)
        {
            return new Chunk(chunkSize, id_x, id_y, left_down_corner, hex_radius);
        }


    }

}