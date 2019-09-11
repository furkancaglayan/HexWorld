using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

public static class _EditorUtility 
{
    public static void AddLayer(string layer)
    {
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
        if (asset != null)
        {
            var so = new SerializedObject(asset);
            var layers = so.FindProperty("layers");

            var numLayers = layers.arraySize;


            for (int i = 0; i < numLayers; i++)
            {
                var existingTag = layers.GetArrayElementAtIndex(i);
                if (existingTag.stringValue.Equals(layer)) return;
            }

            int emptySpace = 8;
            layers.InsertArrayElementAtIndex(emptySpace);
            layers.GetArrayElementAtIndex(emptySpace).stringValue = layer;
            so.ApplyModifiedProperties();
            so.Update();
        }

    }

    /// <summary>
    /// Adds the given tag parameter to project.
    /// </summary>
    /// <param name="tag"></param>
    public static void AddTag(string tag)
    {
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
        if (asset != null)
        {
            var so = new SerializedObject(asset);
            var tags = so.FindProperty("tags");

            var numTags = tags.arraySize;

            for (int i = 0; i < numTags; i++)
            {
                var existingTag = tags.GetArrayElementAtIndex(i);
                if (existingTag.stringValue == tag) return;
            }

            tags.InsertArrayElementAtIndex(numTags);
            tags.GetArrayElementAtIndex(numTags).stringValue = tag;
            so.ApplyModifiedProperties();
            so.Update();
        }

    }

    public static bool CheckIfDirectoryIsValid(string path, bool showFinalDialog)
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
            retVal = false;
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
            Utils.ShowDialog(title, message, "Ok");
        else if (showFinalDialog)
            Utils.ShowDialog("Valid", "Directory seems to be valid.", "Ok");

        return retVal;

    }
}
