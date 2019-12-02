using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public CombinedDataSet set;
    void Start()
    {
        set = (CombinedDataSet)ScriptableObject.CreateInstance(typeof(CombinedDataSet));
        set.LoadPrefabs("Assets/HexWorld/Prefabs/Tiles",false);
        //folder.Create("Assets/HexWorld/Prefabs/Tiles/Forest Tiles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
