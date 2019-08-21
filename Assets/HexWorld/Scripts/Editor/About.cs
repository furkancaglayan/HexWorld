using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class About : EditorWindow {

    private static Texture2D birchgamesLogo;
    private static readonly string birchgamesLogoPath = "Assets/HexWorld/Textures/birchgames_logo.png";
    [MenuItem("Window/HexWorld/About Us", priority =1)]
    public static void Init()
    {

        birchgamesLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(birchgamesLogoPath, typeof(Texture2D));
        About window = (About)GetWindow(typeof(About));
        window.minSize.Set(250, 600);
        window.autoRepaintOnSceneChange = true;
        window.titleContent = new GUIContent("About Us", birchgamesLogo);
        window.Show(false);
    }
    public static void OnEnable()
    {

        birchgamesLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(birchgamesLogoPath, typeof(Texture2D));
    }

  
    private void OnGUI()
    {
        GUIStyle uIStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            richText=true,
            fontSize=11
        };
        GUIStyle urlStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            richText = true,
            fontSize = 11
            
        };
        GUIStyle logoStyle = new GUIStyle(GUI.skin.window)
        {
            padding = new RectOffset(0, 0, 8, 0)
        };
        GUILayout.BeginVertical();

        GUI.color = new Color(255F / 255F, 211F / 255F, 89F / 255F, .8F);
        GUILayout.BeginHorizontal(logoStyle, GUILayout.Width(position.width), GUILayout.Height(128));
        GUILayout.Space(position.width / 2 - 148);

        GUILayout.Label(IconPack.GetBirchgamesLogo(), GUILayout.Width(128), GUILayout.Height(128));
        GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
        GUILayout.EndHorizontal();
        GUILayout.Label("To find more information about HexWorld, visit <color=green>birchgames.com</color>", uIStyle);
        GUILayout.EndVertical();
        Rect rect = EditorGUILayout.GetControlRect();
        if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
            Application.OpenURL("www.birchgames.com");
        if (rect.Contains(Event.current.mousePosition))
        {
            GUI.Label(rect, "<color=blue>birchgames.com</color>", urlStyle);
        }
        else
        {
            GUI.Label(rect, "<color=green>birchgames.com</color>", urlStyle);
        }
       
        Repaint();
    }
}
