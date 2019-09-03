
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BrushType=Enums.BrushType;
using RotationType = Enums.RotationType;
public static class HexWorldBrush
{
  
    public static GameObject ApplyStroke(HexWorldTile tile,
                                         HexWorldPrefab selectedPrefab,
                                         HexWorldChunk chunk,
                                         HexWorldMap map,
                                         BrushType brushType,
                                         int brushRadius,
                                         bool randomRotation,
                                         bool inheritRotation,
                                         RotationType rotationType
                                  )
    {
        if (tile == null)
            return null;


        GameObject returningTile = null;
        List<HexWorldTile> lst = map.GetTilesInRadius(tile,brushRadius);
        

        float rotation = 0;
        int r = 0;
        if (randomRotation)
            r = Random.Range(0, 6);
        rotation = r * 60;


        switch (brushType)
        {
            case BrushType.Place:
                foreach (var hex in lst)
                {
                    if(inheritRotation)
                        hex.PlacePrefab(selectedPrefab, chunk, rotation, rotationType);
                    else
                    {
                        if (randomRotation)
                        {
                            r = Random.Range(0, 6);
                            rotation = r * 60;
                        }
                        hex.PlacePrefab(selectedPrefab, chunk, rotation, rotationType);
                    }
                }
                returningTile = tile.gameObject;
                break;
            case BrushType.Delete:
                foreach (var hex in lst)
                    hex.RemovePrefab();
                break;
            case BrushType.Select:
                returningTile = tile.gameObject;
                break;
            case BrushType.RotateRight:
                foreach (var hex in lst)
                    hex.Rotate(60, rotationType);
                break;
            case BrushType.RotateLeft:
                foreach (var hex in lst)
                    hex.Rotate(-60, rotationType);
                break;
        }

        return returningTile;

    }

    public static void ApplySimpleStroke(BrushType brushType, HexWorldTile tile, HexWorldChunk chunk, HexWorldPrefab selectedPrefab,
        bool randomRotation, RotationType rotationType)
    {
        float rotation = 0;
        int r = 0;
        if (randomRotation)
            r = Random.Range(0, 6);
        rotation = r * 60;


        switch (brushType)
        {
            case BrushType.Place:
                tile.PlacePrefab(selectedPrefab, chunk, rotation, rotationType);
                break;
            case BrushType.Delete:
                tile.RemovePrefab();
                break;
            case BrushType.RotateRight:
                tile.Rotate(60, rotationType);
                break;
            case BrushType.RotateLeft:
                tile.Rotate(-60, rotationType);
                break;
        }
    }
}
