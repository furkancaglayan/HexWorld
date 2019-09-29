using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{

    [Serializable]
    public class PrefabLayer : ILayer
    {
        [SerializeField] public Prop prefab;
    }

}