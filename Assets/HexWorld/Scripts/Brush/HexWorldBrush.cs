using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class HexWorldBrush
{
    private static int brushRadius;

    //TODO:Change this to tile list later.
    public static void ApplyStroke(Enums.BrushType brushType, HexWorldTile tile, HexWorldPrefab selectedPrefab,
        bool randomRotation, Enums.RotationType rotationType, out GameObject gameObject)
    {
        gameObject = null;
        if (tile == null)
            return;

        float rotation = 0;
        int r = 0;
        if (randomRotation)
            r = Random.Range(0, 6);
        rotation = r * 60;


        switch (brushType)
        {
            case Enums.BrushType.Place:
                gameObject = tile.PlacePrefab(selectedPrefab, rotation, rotationType);
                break;
            case Enums.BrushType.Delete:
                tile.RemovePrefab();
                break;
            case Enums.BrushType.Select:
                gameObject = tile.gameObject;
                break;
            case Enums.BrushType.RotateRight:
                tile.Rotate(60, rotationType);
                break;
            case Enums.BrushType.RotateLeft:
                tile.Rotate(-60, rotationType);
                break;
        }
    }

    public static void ApplySimpleStroke(Enums.BrushType brushType, HexWorldTile tile, HexWorldPrefab selectedPrefab,
        bool randomRotation, Enums.RotationType rotationType)
    {
        float rotation = 0;
        int r = 0;
        if (randomRotation)
            r = Random.Range(0, 6);
        rotation = r * 60;


        switch (brushType)
        {
            case Enums.BrushType.Place:
                tile.PlacePrefab(selectedPrefab, rotation, rotationType);
                break;
            case Enums.BrushType.Delete:
                tile.RemovePrefab();
                break;
            case Enums.BrushType.RotateRight:
                tile.Rotate(60, rotationType);
                break;
            case Enums.BrushType.RotateLeft:
                tile.Rotate(-60, rotationType);
                break;
        }
    }

    public static void DrawBrush(HexWorldTile tile,Enums.BrushType brushType,float size)
    {
#if UNITY_EDITOR
        switch (brushType)
        {
            case Enums.BrushType.Place:
                Handles.color = Color.green;
                break;
            case Enums.BrushType.Delete:
                Handles.color = Color.red;
                break;
            case Enums.BrushType.Select:
                Handles.color = Color.blue;
                break;
            case Enums.BrushType.RotateRight:
                Handles.color = Color.black;
                break;
            case Enums.BrushType.RotateLeft:
                Handles.color = Color.black;
                break;
            default:
                Handles.color = Color.white;
                break;

        }
        DrawHexagon(tile,0);
        DrawHexagon(tile, size);
        for (int i = 0; i < 6; i++)
            Handles.DrawLine(tile.corners[i], tile.corners[i]+new Vector3(0,size,0));
#endif
    }

    private static void DrawHexagon(HexWorldTile tile, float size)
    {
#if UNITY_EDITOR
        Vector3[] arr = new Vector3[7];
        for (int i = 0; i < 6; i++)
            arr[i] = tile.corners[i]+new Vector3(0,size,0);
        arr[6]= tile.corners[0] + new Vector3(0, size, 0);
        Handles.DrawAAPolyLine(arr);
#endif
    }
}