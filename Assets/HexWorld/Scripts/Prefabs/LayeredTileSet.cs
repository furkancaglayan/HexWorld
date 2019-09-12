using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LayeredTileSet : TileSet
{


    [SerializeField] public PropFolder tileLayer;
    [SerializeField] public PropFolder firstLayer;
    [SerializeField] public PropFolder secondLayer;
    [SerializeField] public PropFolder thirdLayer;


    [SerializeField] public TileUpgrade tileUpgrade;

    public void CreateLayers()
    {
        tileLayer = Factory.CreatePropFolder("Tile Layer");
        firstLayer = Factory.CreatePropFolder("First Layer");
        secondLayer = Factory.CreatePropFolder("Second Layer");
        thirdLayer = Factory.CreatePropFolder("Third Layer");
    }
    public override int GetPropCount()
    {
        throw new System.NotImplementedException();
    }
}
