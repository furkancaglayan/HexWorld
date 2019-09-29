using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexWorld
{
    public class _EditorModelViewer : EditorWindow
    {

        private static EditorConfiguration _configuration;

        private Editor modelView;
        private static Object content;
        public static void Init(Object contentToShow)
        {

            content = contentToShow;
            _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));
            _EditorModelViewer window = (_EditorModelViewer)GetWindow(typeof(_EditorModelViewer));
            window.minSize.Set(512, 512);
            window.maxSize.Set(512, 512);

            window.titleContent = new GUIContent("Model Viewer", _configuration.birchGamesLogo);
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
}
