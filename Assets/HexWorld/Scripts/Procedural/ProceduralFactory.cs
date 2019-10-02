using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{
    public static class ProceduralFactory
    {
        public static ProceduralMap CreateProceduralMap(Enums.MapSize mapSize, float hexRad, Material mat)
        {
            ProceduralMap map = new ProceduralMap(mapSize, hexRad);
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

        public static ProceduralChunk CreateProceduralChunk(int chunkSize, int id_x, int id_y, Vector3 left_down_corner, float hex_radius)
        {
            ProceduralChunk chunk = new ProceduralChunk(chunkSize, id_x, id_y, hex_radius)
            {

            };
            chunk.CreateCorners(left_down_corner, chunkSize);
            return chunk;
        }
        public static ProceduralProp CreateProceduralProp(string path)
        {
            ProceduralProp prop = new ProceduralProp(path);
            prop.LoadObject();
            return prop;
        }


        public static ProceduralCombinedTileSet CreateProceduralCombinedTileSet(string name, string ecosystem, string path)
        {
            ProceduralCombinedTileSet set = (ProceduralCombinedTileSet)ScriptableObject.CreateInstance(typeof(ProceduralCombinedTileSet));
            set.ecosystem = ecosystem;
            set.name = name;
            set.LoadPrefabs(path);
            return set;
        }
    }
}

