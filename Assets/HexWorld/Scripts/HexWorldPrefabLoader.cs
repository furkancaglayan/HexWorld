using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using prefab=Enums.PrefabType;

[CreateAssetMenu(fileName ="Data")]
public class HexWorldPrefabLoader:ScriptableObject
{
    public string path = "Assets/HexWorld/Prefabs/Tiles";
    public string ecosystemName = "Forest";
    public prefab prefabType = prefab.BOTH;

    public HexWorldPrefabSet hexWorldPrefabSet;

}
