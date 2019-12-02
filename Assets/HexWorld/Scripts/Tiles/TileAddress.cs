using System;
using UnityEngine;

[Serializable]
public class TileAddress
{
    [SerializeField] public int chunkIdX;
    [SerializeField] public int chunkIdY;

    [SerializeField] public int idX;
    [SerializeField] public int idY;

    public TileAddress(int _chunkIdX, int _chunkIdY, int _idX, int _idY)
    {
        this.chunkIdX = _chunkIdX;
        this.chunkIdY = _chunkIdY;
        this.idX = _idX;
        this.idY = _idY;
    }

    




}