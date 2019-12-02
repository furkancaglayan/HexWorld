using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object=UnityEngine.Object;

public static class _EditorPrefabUtility
{
    public static GameObject Combine(PlaceHolderTile placeHolder, string savePath, string saveName)
    {
        //TODO:check if valid
        string path = savePath + "/" + saveName + ".prefab";
        GameObject copy = (GameObject) Object.Instantiate(placeHolder.tile);
        copy.transform.position = Vector3.zero;

        GameObject tileUpgrade = (GameObject) Object.Instantiate(placeHolder.upgrade);
        tileUpgrade.transform.position = Vector3.zero;


        tileUpgrade.transform.SetParent(copy.transform);

        GameObject returnVal= PrefabUtility.SaveAsPrefabAsset(copy, path);
        Object.DestroyImmediate(copy);
        return returnVal;
    }
}
