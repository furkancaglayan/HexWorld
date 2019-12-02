using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public CombinedTileSet set;
    void Start()
    {
        set = (CombinedTileSet)ScriptableObject.CreateInstance(typeof(CombinedTileSet));
        set.LoadPrefabs("Assets/HexWorld/Prefabs/Tiles");
        //folder.Create("Assets/HexWorld/Prefabs/Tiles/Forest Tiles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
