using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LayerWrapper
{
    [SerializeField] public TileUpgrade upgrade;
    [SerializeField] public ILayer[] layers = new ILayer[4];
}
