using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using DataType=Enums.DataType;
using g = UnityEngine.GUILayout;
using ge = UnityEditor.EditorGUILayout;

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
        if(_configuration==null)
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
    private float SecondFieldWidth => (position.width - 36)/2;

    #endregion
    #region CombinedFields
    private string _path = _configuration.prefabsDirectory;
    private string _savePath = _configuration.saveDirectory;
    #endregion

    #region LayeredFields

    private string[] layerNames = {"Tiling Layer","First Layer","Second Layer","Third Layer" };

    private string[] layerPaths =
    {
        "Assets/HexWorld", "Assets/HexWorld", "Assets/HexWorld", "Assets/HexWorld"
    };
    #endregion
    #region ONGUI
    private void OnGUI()
    {
        #region Styles
        GUIStyle labelstyle = new GUIStyle(EditorStyles.miniBoldLabel)
        {
            fontSize = 10,
            padding=new RectOffset(0,0,-18,0)
        };
        GUIStyle textstyle = new GUIStyle(EditorStyles.miniLabel)
        {
            fontSize = 10,
            wordWrap=true,
            //margin = new RectOffset(0, 0, 2, 0),
        };
        GUIStyle textfield = new GUIStyle(EditorStyles.helpBox)
        {
            margin = new RectOffset(0, 0, 2, 0),
        };
        #endregion


        _mainScrollView = g.BeginScrollView(_mainScrollView,GUI.skin.window);
        g.Label("HexWorld TileSet Generator", labelstyle);
        g.BeginVertical();

        g.Space(10);

        g.BeginHorizontal();
        g.Space(10);
        g.Label("TileSet Name:", textstyle, g.Width(labelWidth));
        _tilesetName = g.TextField(_tilesetName, textfield, g.Width(FieldWidth));
        g.EndHorizontal();


        g.BeginHorizontal();
        g.Space(10);
        g.Label("Save Directory:", textstyle, g.Width(labelWidth));
        _savePath = g.TextField(_savePath, textfield, g.Width(FieldWidth));
        g.EndHorizontal();


        g.BeginHorizontal();
        g.Space(10);
        g.Label("Ecosystem:", textstyle, g.Width(labelWidth));
        _tilesetEcosystem = g.TextField(_tilesetEcosystem, textfield, g.Width(FieldWidth));
        g.EndHorizontal();

        g.BeginHorizontal();
        g.Space(10);
        g.Label("TileSet Type:", textstyle, g.Width(labelWidth));
        dataType = (DataType)ge.EnumPopup(dataType,  g.Width(FieldWidth));
        g.EndHorizontal();

        Rect lastRect=GUILayoutUtility.GetLastRect();
        Handles.DrawLine(new Vector3(lastRect.position.x, lastRect.position.y+20),new Vector3(position.width-10,lastRect.y+20));



        g.Space(10);
        #region CombinedDatasetGUI
        if (dataType == DataType.Combined)
        {
            g.Space(10);
            g.BeginVertical();
            g.Space(10);
            g.Label("Combined TileSet", labelstyle);


            g.BeginHorizontal(EditorStyles.helpBox, g.Width(position.width-30));
            g.Space(10);
            g.Label("Combined TileSet allows you to import already-made, game ready tiles." +
                    " Recommended to use if your tile assets are created and merged in an external modeling software. " +
                    "Combined TileSet will include first subfolders of the root directory besides" +
                    " root directory prefabs.", textstyle);
            g.EndHorizontal();
            g.Space(10);

            g.BeginHorizontal();
            g.Space(10);
            g.Label("TileSet Path:", textstyle, g.Width(labelWidth));
            _path = g.TextField(_path, textfield, g.Width(FieldWidth-50));
            g.Space(10);
            GUI.color = _color2;
            if (g.Button(new GUIContent("F", "Check if directory is valid!"), EditorStyles.toolbarButton, g.Width(40)))
                _EditorUtility.CheckIfDirectoryIsValid(_path,true);
            GUI.color = Color.white;
            g.EndHorizontal();



            g.Space(4);
            g.BeginHorizontal();
            g.Space(10);
            GUI.color = _color1;
            if (g.Button("Create TileSet", EditorStyles.toolbarButton, g.Width(SecondFieldWidth)))
                _EditorTileSetUtility.CreateCombinedDataSet(_path,_tilesetName,_tilesetEcosystem,_savePath
                    );
            GUI.color = Color.white;
                
                
               
               
            if (g.Button("Open Prefab Generator", EditorStyles.toolbarButton, g.Width(SecondFieldWidth)))
                _EditorPrefabGeneratorWindow.Init();
            g.EndHorizontal();




            g.EndVertical();
        }
        #endregion

        #region LayeredDatasetGUI

        if (dataType == DataType.Layered)
        {
            g.Space(10);
            g.BeginVertical();
            g.Space(10);
            g.Label("Layered TileSet", labelstyle);

            g.BeginHorizontal(EditorStyles.helpBox, g.Width(position.width - 30));
            g.Space(10);
            g.Label("Layered TileSet allows you to define prefabs for 4 different layers. " +
                    "First Layer is for tiles, and other tiles are for other props. ", textstyle);
            g.EndHorizontal();
            g.Space(10);



            for (int i = 0; i < 4; i++)
            {
                g.BeginHorizontal();
                g.Space(10);
                g.Label(layerNames[i]+" Path:", textstyle, g.Width(labelWidth));
                layerPaths[i] = g.TextField(layerPaths[i], textfield, g.Width(FieldWidth - 50));
                g.Space(10);
                GUI.color = _color2;
                if (g.Button(new GUIContent("F", "Check if directory is valid!"), EditorStyles.toolbarButton, g.Width(40)))
                    _EditorUtility.CheckIfDirectoryIsValid(layerPaths[i], true);
                GUI.color = Color.white;
                g.EndHorizontal();
            }







            g.Space(4);
            g.BeginHorizontal();
            g.Space(10);
            GUI.color = _color1;
            if (g.Button("Create TileSet", EditorStyles.toolbarButton, g.Width(SecondFieldWidth)))
                _EditorTileSetUtility.CreateLayeredTileSet(_tilesetName,_tilesetEcosystem,_savePath, layerPaths);
            GUI.color = Color.white;

            g.EndHorizontal();

            g.EndVertical();
        }
        #endregion

        g.EndVertical();
        g.EndScrollView();
    }
    #endregion

}
