using UnityEditor;
using UnityEngine;


namespace HexWorld
{

    [CustomEditor(typeof(CombinedTileSet))]
    public class _EditorCombinedTileSet : Editor
    {
        CombinedTileSet _instance;
        #region Fields
        private float labelWidth = 120;
        private int _selectedFolder = 0;
        private int propCount = 0;

        private bool showPrefabs = true;
        private bool showDefault = false;
        #endregion

        public void OnEnable()
        {
            _instance = (CombinedTileSet)target;
            propCount = _instance.GetPropCount();
        }

        public override void OnInspectorGUI()
        {

            GUIStyle labels = new GUIStyle(EditorStyles.miniLabel)
            {
                fontSize = 12,
                richText = true,
                margin = new RectOffset(0, 0, -2, 0)
            };
            GUI.color = Color.white;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("TileSet name:", labels, GUILayout.Width(labelWidth));
            GUILayout.Label(_instance.name, labels);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Ecosystem:", labels, GUILayout.Width(labelWidth));
            GUILayout.Label(_instance.ecosystem, labels);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Prefab Count:", labels, GUILayout.Width(labelWidth));
            GUILayout.Label(propCount.ToString(), labels);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Show Prefabs:", labels, GUILayout.Width(labelWidth));
            showPrefabs = GUILayout.Toggle(showPrefabs, "");
            GUILayout.EndHorizontal();

            if (showPrefabs)
                ShowProps();

            showDefault = EditorGUILayout.Foldout(showDefault, "Show Default Inspector", true);
            if (showDefault)
                DrawDefaultInspector();

            GUILayout.EndVertical();
        }

        private void ShowProps()
        {
            GUIStyle prefabStyle = new GUIStyle(GUI.skin.window)
            {
                imagePosition = ImagePosition.ImageAbove,
                padding = new RectOffset(0, 0, 40, 0),
                font = EditorStyles.miniBoldFont,
                fontStyle = FontStyle.Italic,
                fontSize = 12
            };
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(600), GUILayout.MinWidth(400),
                GUILayout.MinHeight(600));
            if (_instance.folders != null)
            {
                int rows = 4;

                string[] folderContents = _instance.GetFolderNames();
                _selectedFolder = GUILayout.Toolbar(_selectedFolder, folderContents, EditorStyles.toolbarButton);

                for (int i = 0; i < _instance.folders[_selectedFolder].props.Count; i++)
                {
                    if (i % rows == 0)
                    {
                        if (i != 0)
                            GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                    }


                    string _name = _instance.folders[_selectedFolder].props[i].@object.name;
                    Texture2D texture = AssetPreview.GetAssetPreview(_instance.folders[_selectedFolder].props[i].@object);
                    GUIContent content = new GUIContent(_name, texture);


                    GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400 / rows));
                    GUILayout.Label(content.image, EditorStyles.helpBox);
                    GUILayout.Label(content.text, EditorStyles.boldLabel);

                    EditorGUILayout.Separator();
                    GUILayout.Label("Appearance Rate %99");

                    Rect rect = GUILayoutUtility.GetRect(140, 20);
                    rect.width = 140;
                    rect.height = 20;
                    EditorGUI.ProgressBar(rect, .3f, "Rate");

                    if (GUILayout.Button("Delete", EditorStyles.toolbarButton, GUILayout.Width(rect.width)))
                    {
                        _instance.folders[_selectedFolder].DeleteProp(i);
                        propCount = _instance.GetPropCount();
                    }

                    GUILayout.Space(3);
                    GUILayout.EndVertical();

                    if (i == _instance.folders[_selectedFolder].props.Count - 1)
                        GUILayout.EndHorizontal();

                }

            }


            GUILayout.EndVertical();
        }
    }

}