using UnityEditor;
using UnityEngine;

namespace HexWorld
{

    [CustomEditor(typeof(LayeredTileSet))]
    public class _EditorLayeredTileSet : Editor
    {

        LayeredTileSet _instance;

        #region Fields
        private float labelWidth = 120;
        private int _selectedFolder = 0;
        private int propCount = 0;

        private bool showPrefabs = true;
        private bool showDefault = false;
        #endregion

        public void OnEnable()
        {
            _instance = (LayeredTileSet)target;
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
            GUIStyle labels = new GUIStyle(EditorStyles.miniBoldLabel)
            {
                fontSize = 14,
                richText = true,
                margin = new RectOffset(0, 0, -2, 0)
            };
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

            int rows = 4;
            //for each layer
            for (int i = 0; i < 4; i++)
            {
                PropFolder folder = _instance.layers[i];


                GUILayout.BeginVertical();

                GUILayout.Label(folder.name, labels);

                for (int j = 0; j < folder.props.Count; j++)
                {
                    if (j % rows == 0)
                    {

                        if (j != 0)
                            GUILayout.EndHorizontal();



                        GUILayout.BeginHorizontal();
                    }

                    if (j % 2 == 0)
                        GUI.color = Color.white;
                    else
                        GUI.color = new Color(.85f, .85f, .85f);
                    string _name = folder.props[j].@object.name;
                    Texture2D texture = AssetPreview.GetAssetPreview(folder.props[j].@object);
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
                        folder.DeleteProp(j);
                        propCount = _instance.GetPropCount();
                    }

                    GUILayout.Space(3);
                    GUILayout.EndVertical();

                    if (j == folder.props.Count - 1)
                        GUILayout.EndHorizontal();

                }


                GUILayout.EndVertical();


            }


            GUILayout.EndVertical();
        }
    }

}