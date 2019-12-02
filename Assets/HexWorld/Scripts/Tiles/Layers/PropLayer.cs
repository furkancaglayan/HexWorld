using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PropLayer : ILayer
{
    [SerializeField] public Prop[] props;
}
