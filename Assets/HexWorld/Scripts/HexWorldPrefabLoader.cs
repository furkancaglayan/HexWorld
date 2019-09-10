using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Data")]
public class HexWorldPrefabLoader:ScriptableObject
{
    public string path = "Assets/HexWorld/Prefabs/Tiles";
    public string ecosystemName = "Forest";

    public HexWorldPrefabSet hexWorldPrefabSet;

}
