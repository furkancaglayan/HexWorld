using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class BrushEditor
{
    
    public static void DrawBrush(HexWorldTile tile,HexWorldMap map, Enums.BrushType brushType, float rad,int brushRadius)
    {
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

        List<HexWorldTile> tileLst = map.GetTilesInRadius(tile,brushRadius);
        for (int i = 0; i < tileLst.Count; i++)
        {
            if (tileLst[i] == null)
                continue;
            DrawHexagon(tileLst[i], 0);
            DrawHexagon(tileLst[i], rad);
            for (int j = 0; j < 6; j++)
                Handles.DrawLine(tileLst[i].corners[j], tileLst[i].corners[j] + new Vector3(0, rad, 0));
        }
       
    }
    public static void DrawBrush(HexWorldTile tile, Enums.BrushType brushType, float size,Color col)
    {
        Handles.color = col;
        DrawHexagon(tile, 0);
        DrawHexagon(tile, size);
        for (int i = 0; i < 6; i++)
            Handles.DrawLine(tile.corners[i], tile.corners[i] + new Vector3(0, size, 0));
    }

    private static void DrawHexagon(HexWorldTile tile, float size)
    {
        Vector3[] arr = new Vector3[7];
        for (int i = 0; i < 6; i++)
            arr[i] = tile.corners[i]+new Vector3(0,size,0);
        arr[6]= tile.corners[0] + new Vector3(0, size, 0);
        Handles.DrawAAPolyLine(arr);
    }
}