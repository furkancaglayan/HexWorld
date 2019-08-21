using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelViewer : EditorWindow {

    private static Texture2D birchgamesLogo;
    private static readonly string birchgamesLogoPath = "Assets/HexWorld/Textures/birchgames_logo.png";


    private Editor modelView;
    private static Object content;
    public static void Init(Object contentToShow)
    {
        content = contentToShow;
         birchgamesLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(birchgamesLogoPath, typeof(Texture2D));
        ModelViewer window = (ModelViewer)GetWindow(typeof(ModelViewer));
        window.minSize.Set(512, 512);
        window.maxSize.Set(512, 512);

        window.titleContent = new GUIContent("Model Viewer", birchgamesLogo);
        window.Show();
    }
    private void OnGUI()
    {
        position.Set(position.x, position.y, 512, 512);
        if (content != null)
        {
            if (modelView == null)
                modelView = Editor.CreateEditor(content);
            GUI.color = Color.gray;
            modelView.OnPreviewGUI(GUILayoutUtility.GetRect(512, 512), GUI.skin.box);
        }
    }
}
