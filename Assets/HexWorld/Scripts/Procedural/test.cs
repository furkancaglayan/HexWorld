using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexWorld;

public class test : MonoBehaviour
{
    
    public Enums.MapSize mapSize;
    public Material material;
    public float hexRad;
    void Start()
    {
       ProceduralMap map=ProceduralFactory.CreateProceduralMap(mapSize,hexRad,material);
       Debug.Log("Map Created..!");
    }

    void Update()
    {
        
    }
}
