using System;
using UnityEngine;

[Serializable]
public abstract class TileSet : ScriptableObject
{
    public new string name;
    public string ecosystem;
    public abstract int GetPropCount();

}
