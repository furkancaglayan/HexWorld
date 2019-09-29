using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexWorld
{
    using DataType = Enums.DataType;
    public class _EditorTileSetGenerator : EditorWindow
    {
        #region Init
        private static EditorConfiguration _configuration;
        [MenuItem("HexWorld/TileSet Generator", priority = 2)]
        static void Init()
        {
            _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));
            _EditorTileSetGenerator window = (_EditorTileSetGenerator)GetWindow(typeof(_EditorTileSetGenerator));
            window.autoRepaintOnSceneChange = true;
            window.titleContent = new GUIContent("TileSet Generator", _configuration.birchGamesLogo);
            window.Show(false);

        }

        private void OnEnable()
        {
            if (_configuration == null)
                _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));
        }
        #endregion
        #region Fields
        private string _tilesetName = "TileSet1";
        private string _tilesetEcosystem = "Awesome Ecosystem";
        private Vector2 _mainScrollView;
        private DataType dataType = DataType.Layered;
        private float labelWidth = 90;
        private Color _color2 = Color.yellow;
        private Color _color1 = Color.green;

        private float FieldWidth => position.width - labelWidth - 40;
        private float SecondFieldWidth => (position.width - 36) / 2;

        #endregion
        #region CombinedFields
        private string _path = _configuration.prefabsDirectory;
        private string _savePath = _configuration.tileSetSaveDirectory;
        #endregion

        #region LayeredFields

        private string[] layerNames = { "Tiling Layer", "First Layer", "Second Layer", "Third Layer" };

        private string[] layerPaths =
        {
        "Assets/HexWorld/Prefabs/Tiles/",
        "Assets/HexWorld/Prefabs/Tiles/",
        "Assets/HexWorld/Prefabs/Tiles/",
        "Assets/HexWorld/Prefabs/Tiles/"
    };
        #endregion
        #region ONGUI
        private void OnGUI()
        {
            #region Styles
            GUIStyle labelstyle = new GUIStyle(EditorStyles.miniBoldLabel)
            {
                fontSize = 10,
                padding = new RectOffset(0, 0, -18, 0)
            };
            GUIStyle textstyle = new GUIStyle(EditorStyles.miniLabel)
            {
                fontSize = 10,
                wordWrap = true,
                //margin = new RectOffset(0, 0, 2, 0),
            };
            GUIStyle textfield = new GUIStyle(EditorStyles.helpBox)
            {
                margin = new RectOffset(0, 0, 2, 0),
            };
            #endregion


            _mainScrollView = GUILayout.BeginScrollView(_mainScrollView, GUI.skin.window);
            GUILayout.Label("HexWorld TileSet Generator", labelstyle);
            GUILayout.BeginVertical();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("TileSet Name:", textstyle, GUILayout.Width(labelWidth));
            _tilesetName = GUILayout.TextField(_tilesetName, textfield, GUILayout.Width(FieldWidth));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Save Directory:", textstyle, GUILayout.Width(labelWidth));
            _savePath = GUILayout.TextField(_savePath, textfield, GUILayout.Width(FieldWidth));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Ecosystem:", textstyle, GUILayout.Width(labelWidth));
            _tilesetEcosystem = GUILayout.TextField(_tilesetEcosystem, textfield, GUILayout.Width(FieldWidth));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("TileSet Type:", textstyle, GUILayout.Width(labelWidth));
            dataType = (DataType)EditorGUILayout.EnumPopup(dataType, GUILayout.Width(FieldWidth));
            GUILayout.EndHorizontal();

            Rect lastRect = GUILayoutUtility.GetLastRect();
            Handles.DrawLine(new Vector3(lastRect.position.x, lastRect.position.y + 20), new Vector3(position.width - 10, lastRect.y + 20));



            GUILayout.Space(10);
            #region CombinedDatasetGUI
            if (dataType == DataType.Combined)
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical();
                GUILayout.Space(10);
                GUILayout.Label("Combined TileSet", labelstyle);


                GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(position.width - 30));
                GUILayout.Space(10);
                GUILayout.Label("Combined TileSet allows you to import already-made, game ready tiles." +
                        " Recommended to use if your tile assets are created and merged in an external modeling software. " +
                        "Combined TileSet will include first subfolders of the root directory besides" +
                        " root directory prefabs.", textstyle);
                GUILayout.EndHorizontal();
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("TileSet Path:", textstyle, GUILayout.Width(labelWidth));
                _path = GUILayout.TextField(_path, textfield, GUILayout.Width(FieldWidth - 50));
                GUILayout.Space(10);
                GUI.color = _color2;
                if (GUILayout.Button(new GUIContent("F", "Open Folder Panel"), EditorStyles.toolbarButton, GUILayout.Width(40)))
                    _path = OpenFolder("Choose Folder");
                GUI.color = Color.white;
                GUILayout.EndHorizontal();



                GUILayout.Space(4);
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUI.color = _color1;
                if (GUILayout.Button("Create TileSet", EditorStyles.toolbarButton, GUILayout.Width(SecondFieldWidth)))
                    _EditorTileSetUtility.CreateCombinedDataSet(_path, _tilesetName, _tilesetEcosystem, _savePath
                        );
                GUI.color = Color.white;




                if (GUILayout.Button("Open Prefab Generator", EditorStyles.toolbarButton, GUILayout.Width(SecondFieldWidth)))
                    _EditorPrefabGeneratorWindow.Init();
                GUILayout.EndHorizontal();




                GUILayout.EndVertical();
            }
            #endregion

            #region LayeredDatasetGUI

            if (dataType == DataType.Layered)
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical();
                GUILayout.Space(10);
                GUILayout.Label("Layered TileSet", labelstyle);

                GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(position.width - 30));
                GUILayout.Space(10);
                GUILayout.Label("Layered TileSet allows you to define prefabs for 4 different layers. " +
                        "First Layer is for tiles, and other tiles are for other props. ", textstyle);
                GUILayout.EndHorizontal();
                GUILayout.Space(10);



                for (int i = 0; i < 4; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.Label(layerNames[i] + " Path:", textstyle, GUILayout.Width(labelWidth));
                    layerPaths[i] = GUILayout.TextField(layerPaths[i], textfield, GUILayout.Width(FieldWidth - 50));
                    GUILayout.Space(10);
                    GUI.color = _color2;
                    if (GUILayout.Button(new GUIContent("F", "Open Folder Panel"), EditorStyles.toolbarButton, GUILayout.Width(40)))
                        layerPaths[i]=OpenFolder("Choose Folder");
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();
                }







                GUILayout.Space(4);
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUI.color = _color1;

                GUILayout.BeginHorizontal();
                GUILayout.Space(position.width / 2 - SecondFieldWidth / 2 - 20);
                if (GUILayout.Button("Create TileSet", EditorStyles.toolbarButton, GUILayout.Width(SecondFieldWidth)))
                    _EditorTileSetUtility.CreateLayeredTileSet(_tilesetName, _tilesetEcosystem, _savePath, layerPaths);
                GUILayout.EndHorizontal();

                GUI.color = Color.white;

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            #endregion

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        #endregion

        private string OpenFolder(string title) 
        {
            string path=EditorUtility.OpenFolderPanel(title, "Assets", "");
            string[] directories=path.Split('/');

            string finalPath="";
            bool canAdd=false;
            foreach (var item in directories)
            {
                if(item.Equals("Assets")){
                    canAdd=true;
                }
                if(canAdd){
                    finalPath+=item+"/";
                }
            }
            finalPath=finalPath.TrimEnd('/');
            if(finalPath==null||finalPath=="")
                return "Assets";
            return finalPath;
        }
    }
}


