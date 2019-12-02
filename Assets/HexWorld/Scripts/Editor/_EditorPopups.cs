using System.Collections;
using UnityEditor;
using UnityEngine;

public static class _EditorPopups
{
    public static void ShowMessage(string title,string message)
    {
        EditorUtility.DisplayDialog(title,message,"Ok");
    }
}
