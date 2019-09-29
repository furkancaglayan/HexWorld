using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{

    [Serializable]
    public class PropLayer : ILayer
    {
        [SerializeField] public Prop[] props;
    }

}