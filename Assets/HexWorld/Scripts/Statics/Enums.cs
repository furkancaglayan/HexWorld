using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums  {

 
    public enum RotationType
    {
        X,
        Y,
        Z
    }
    public enum MapSize
    {
        
        Small=20,
        Medium=40,
        Large=60,
        Larger=80,
        Huge=100,
        GIGANTIC =1000
    }

    public enum BrushType
    {
        Place=0,
        Delete=1,
        Select=2,
        RotateRight=3,
        RotateLeft=4,
    }

    public enum ColorScheme
    {
        HOT,
        COLD,
        SUNNY,
        DARK
    }

    public enum PrefabType
    {
        PREFAB,
        TILE,
        BOTH
    }
}
