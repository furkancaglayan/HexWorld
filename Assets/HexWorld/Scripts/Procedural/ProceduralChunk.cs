using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{
    [Serializable]
    public class ProceduralChunk : Chunk
    {
        public ProceduralChunk(int chunk_size, int id_x, int id_y, float gridRadius) : base(chunk_size, id_x, id_y, gridRadius)
        {

        }
    }
}
