using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class _EditorTileSetUtility 
{
    public static void CreateCombinedDataSet(string path,string name,string ecosystem, string savePath)
    {

        bool valid=_EditorUtility.CheckIfDirectoryIsValid(path, false);
        if (!valid)
            return;
        if(!savePath.Equals("Assets/"))
            if (!savePath.Substring(0, 6).Equals("Assets"))
            {
                EditorUtility.DisplayDialog("Invalid Save Path", "Please enter a valid save directory!", "Ok");
                return;
            }
        bool credentialsAreValid = _EditorUtility.IsStringValid(new []{name,ecosystem});
        if (!credentialsAreValid)
        {
            EditorUtility.DisplayDialog("Credentials aren't correct!", "You broke the system.Congrats! Please" +
                                                                       "try again with valid credentials", "OK");
            return;
        }

        if (!Directory.Exists(savePath))
        {
            int choice=EditorUtility.DisplayDialogComplex("Directory does not exist!","Directory '"+ savePath+"' " +
                                                                            "does not exist. Would you like to create it?"
                                                                            ,"Yes","No","Cancel");
            if (choice == 0)
                Directory.CreateDirectory(savePath);
            else
                return;
        }

        CombinedTileSet set = Factory.CreateCombinedTileSet(path);
        string fullSavePath = savePath.Trim('/')+"/"+name+".asset";
        AssetDatabase.CreateAsset(set, fullSavePath);
        set.name = name;
        set.ecosystem = ecosystem;
        EditorUtility.SetDirty(set);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Successfully created prefab set!", "Prefab set " +
            name+" has been successfully saved at Directory:"+ savePath.Trim('/'),"Ok");
    }
}
