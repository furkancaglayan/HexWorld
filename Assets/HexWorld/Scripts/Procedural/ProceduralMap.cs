using System;
using System.Collections.Generic;
using UnityEngine;


namespace HexWorld
{
    [Serializable]
    public class ProceduralMap : Map
    {
        public ProceduralMap(Enums.MapSize mapSize, float hexRadius) : base(mapSize, hexRadius)
        {
        }

        /// <summary>
        /// Returns a unique map name.
        /// </summary>
        /// <returns></returns>
        public override void CreateObjectName()
        {
            string time = System.DateTime.UtcNow.ToLocalTime().ToLongTimeString();
            string name = "[HexWorld_Procedural_" + time + "_" + mapSize.ToString() + "]";

            _mapName = name;
        }
    }

}
