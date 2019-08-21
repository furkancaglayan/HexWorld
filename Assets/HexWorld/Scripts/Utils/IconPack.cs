using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class IconPack  {

    private static Texture2D hexworldLogo;
    private static Texture2D hexworldEditorLogo;
    private static Texture2D birchgamesLogo;
    private static Texture2D birchgamesppLogo;
    private static readonly string hexworldLogoPath = "Assets/HexWorld/Textures/hexworld_logo.png";
    private static readonly string hexworldEditorLogoPath = "Assets/HexWorld/Textures/editor_logo.png";
    private static readonly string birchgamesLogoPath = "Assets/HexWorld/Textures/birchgames_logo.png";
    private static readonly string birchgamesppLogoPath = "Assets/HexWorld/Textures/post_process_icon.png";

    public static void Load()
    {
#if UNITY_EDITOR
        if (!hexworldLogo)
            hexworldLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(hexworldLogoPath, typeof(Texture2D));
        if (!hexworldEditorLogo)
            hexworldEditorLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(hexworldEditorLogoPath, typeof(Texture2D));
        if (!birchgamesLogo)
            birchgamesLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(birchgamesLogoPath, typeof(Texture2D));
         if (!birchgamesppLogo)
            birchgamesppLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(birchgamesppLogoPath, typeof(Texture2D));
#endif
    }

    public static Texture2D GetBirchgamesLogo()
    {
        return birchgamesLogo;
    }
    public static Texture2D GetHexworldEditorLogo()
    {
        return hexworldEditorLogo;
    }
    public static Texture2D GetHexworldLogo()
    {
        return hexworldLogo;
    }
    public static Texture2D GetHexworldPPLogo()
    {
        return birchgamesppLogo;
    }

}
