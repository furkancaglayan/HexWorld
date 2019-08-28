using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

#pragma warning disable 0168 

public static class Utils  {




    public static void ShowDialog(string title, string message, string ok)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog(title,message,ok);
#endif
    }

    /// <summary>
    /// Returns the file number in a folder(not subfolders) with the given extension.
    /// Meta files are excluded.
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    private static int GetFileCountInFolder(string folderPath, string extension)
    {
        string[] rootFilePaths = Directory.GetFiles(folderPath);

        int objectCount = 0;
        for (int i = 0; i < rootFilePaths.Length; i++)
        {
            if (rootFilePaths[i].Contains(".meta") || !rootFilePaths[i].Contains(extension))
                continue;
            objectCount++;
        }

        return objectCount;

    }

    
 

 

    public static bool CheckIfDirectoryIsValid(string path,bool showFinalDialog)
    {

        string title = "";
        string message = "";
        bool retVal = false;

        try
        {
            Directory.GetFiles(path);
            retVal = true;
        }
        catch (DirectoryNotFoundException e)
        {
            title = "Directory Exception";
            message = e.Message + "\n" + e.HelpLink;
            retVal= false;
        }
        catch (UnauthorizedAccessException e)
        {
            title = "Directory Exception";
            message = "Invalid Directory Path";
            retVal = false;
        }
        catch (ArgumentException e)
        {
            title = "Directory Exception";
            message = e.Message + "\n" + e.HelpLink;
            retVal = false;

        }
        catch (IOException e)
        {
            title = "Directory Exception";
            message = "Invalid Directory Path";
            retVal = false;
            
        }
        if (!retVal)
            ShowDialog(title, message, "Ok");
        else if(showFinalDialog)
            ShowDialog("Valid", "Directory seems to be valid.", "Ok");

        return retVal;

    }


    public static string CreateName(int length)
    {
        string alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        string alphanumericUpper = alphanumeric.ToUpper();

        alphanumeric += alphanumericUpper;
        string name = "";
        for (int i = 0; i < length; i++)
        {
            int randomInt = Random.Range(0, alphanumeric.Length);
            name += alphanumeric[randomInt].ToString();
        }

        return "HexWorld_"+name;

    }

   

    public static Camera CreateCamera()
    {
        Camera current = Camera.main;
        if (current == null)
            current = (Camera)Object.FindObjectOfType(typeof(Camera));
        if (current == null)
        {
            GameObject newGO = new GameObject("HexWorld_Camera", typeof(Camera));
            current = newGO.GetComponent<Camera>();
        }
        current.name = "[HexWorld_Camera]";
        current.tag = "MainCamera";
        return current;

    }


    public static void ClearEffectsAndLights()
    {
        GameObject lights=GameObject.Find("[HexWorld_Lights]");
        Object.DestroyImmediate(lights);

        GameObject effects = GameObject.Find("[HexWorld_Effects]");
        Object.DestroyImmediate(effects);
    }

    public static void AddCameraController(float minHeight, float maxHeight, float rotSpeed, float Speed, float ScrollSensitivity)
    {
        Camera camera = CreateCamera();
        CameraController controller = camera.GetComponent<CameraController>();
        if(controller==null)
            controller = camera.gameObject.AddComponent<CameraController>();
        controller.Set_Opt(minHeight, maxHeight, rotSpeed, Speed, ScrollSensitivity);
#if UNITY_EDITOR
        Selection.activeGameObject = camera.gameObject;
#endif

    }
}
