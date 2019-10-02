using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{

    public static class Factory
    {


        public static Tile create_tile(Chunk chunk, int idx, int idy, Vector3 center, float radius)
        {
            Tile tile = new Tile(idx, idy, center)
            {
                layerWrapper = new LayerWrapper()
            };
            tile.CreateCorners(center,radius);
            tile.CreateAddress(chunk);

            return tile;
        }
        public static Map create_map(Enums.MapSize mapSize, float hexRad, Material mat)
        {
            Map map= new Map(mapSize, hexRad);
            int chunk_capacity = 20;

            map.DetermineChunkSize((int)mapSize, chunk_capacity);
            map.Create2DChunkArray(chunk_capacity, hexRad, (int)mapSize);
            map.CreateObjectName();
            map.CreateSceneReferences(mat, chunk_capacity);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearProgressBar();
#endif
            return map;

        }
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
        public static Prop CreateProp(string path){
            Prop prop=new Prop(path);
            prop.LoadObject();
            return prop;
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
            Chunk chunk=new Chunk(chunkSize, id_x, id_y,  hex_radius){

            };
            chunk.CreateCorners(left_down_corner, chunkSize);
            return chunk;
        }


    }

}