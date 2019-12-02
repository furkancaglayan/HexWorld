using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapSize = Enums.MapSize;


public class _EditorHexWorld : EditorWindow
{
    #region Fields
        #region Editor
        private readonly Color _editorColor = Color.white;
        private Color _colorSetOne = new Color(255F / 255F, 255F / 255F, 148F / 255F);
        private Color _colorSetTwo = new Color(.388f, .658f, 1);
        private Color _windowColor = new Color(241F / 255F, 235F / 255F, 235F / 255F);
        private Vector2 _editorScrollVal;

        #endregion
        #region Map
        private Material _gridMaterial;
        private Map _map;
    private GameObject mapObject;
        private MapSize _mapSize = MapSize.Small;
        private float _hexSize = 1F;
    #endregion
    #endregion
    #region Configuration

    private static EditorConfiguration _configuration;
    #endregion


    #region Built-in Functions
    [MenuItem("HexWorld/Map Generator", priority = -1)]
    public static void Init()
    {
        _EditorUtils.AddTag("HexWorld");
        _EditorHexWorld window = (_EditorHexWorld)GetWindow(typeof(_EditorHexWorld));
        _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));

        window.autoRepaintOnSceneChange = true;
        window.titleContent = new GUIContent("HexWorld", _configuration.hexWorldLogo);
        window.Show();
    }

    private void OnEnable()
    {
    }
    private void OnGUI()
    {
        #region Styles


        GUIStyle currentStyle = new GUIStyle(GUI.skin.window)
        {
            padding = new RectOffset(0, 0, -5, 0)
        };
        GUIStyle fontStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            padding = new RectOffset(0, 0, -5, 0)
        };
        GUIStyle txtStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            fontSize = 10,
            richText = true
        };
        GUIStyle headingStyle = new GUIStyle(EditorStyles.miniBoldLabel)
        {
            fontSize = 14,
            fontStyle = FontStyle.BoldAndItalic,
        };

        GUIStyle prefabStyle = new GUIStyle(GUI.skin.window)
        {
            imagePosition = ImagePosition.ImageAbove,
            padding = new RectOffset(0, 0, 40, 0),
            font = EditorStyles.miniBoldFont,
            fontStyle = FontStyle.Italic,
            fontSize = 12
        };
        GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbarButton)
        {
            imagePosition = ImagePosition.ImageLeft,
            //padding = new RectOffset(0, 0, 40, 0)
        };

        #endregion
      
        _editorScrollVal = GUILayout.BeginScrollView(_editorScrollVal);
        #region MapCreation
        GUILayout.BeginVertical();
        
        GUI.color = _windowColor;


        GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(140), GUILayout.Width(position.width));
        GUILayout.Label("Create Maps", fontStyle);
        GUILayout.Space(10);
        GUI.color = new Color(255F / 255F, 211F / 255F, 89F / 255F, .8F);


        GUI.color = new Color(_windowColor.r, _windowColor.g, _windowColor.b, .88f);

        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Map Size", txtStyle, GUILayout.Width(80));
        _mapSize = (Enums.MapSize)EditorGUILayout.EnumPopup(_mapSize, GUILayout.Width(position.width - 120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Hex Radius", txtStyle, GUILayout.Width(80));
        _hexSize = EditorGUILayout.FloatField(_hexSize, EditorStyles.helpBox, GUILayout.Width(position.width - 120));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal(GUILayout.Width(position.width - 20));
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Color One", txtStyle, GUILayout.Width(80));
        _colorSetOne = EditorGUILayout.ColorField(new GUIContent(""), _colorSetOne);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Color Two", txtStyle, GUILayout.Width(80));
        _colorSetTwo = EditorGUILayout.ColorField(new GUIContent(""), _colorSetTwo, GUILayout.Width(120));
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Background", txtStyle, GUILayout.Width(80));
        _windowColor = EditorGUILayout.ColorField(_windowColor, GUILayout.Width(position.width - 120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Grid Material", txtStyle, GUILayout.Width(80));
        _gridMaterial = (Material)EditorGUILayout.ObjectField(_gridMaterial, typeof(Material), false,
            GUILayout.Width(position.width - 120));
        GUILayout.Space(10);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        GUI.color = _colorSetOne;
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        if (GUILayout.Button("Create Map", EditorStyles.toolbarButton, GUILayout.Width(position.width - 40)))
            CreateMap(_mapSize, _hexSize, _gridMaterial);
        if (GUILayout.Button("Delete Map", EditorStyles.toolbarButton, GUILayout.Width(position.width - 40)))
        {
            if (_map != null)
            {
                if (_map.gameObject != null && !_map.IsEmpty())
                {
                    int chosen = EditorUtility.DisplayDialogComplex("Delete Map", "Are you sure you want to delete map?", "Yes",
                        "No", "Save the map");
                    if (chosen == 1)
                        return;
                    if (chosen == 2)
                        //SaveMapData("Assets/" + MapsDirectory, MapName, _map);
                        return;
                }
            }

            DeleteMap();

        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(20);


        GUILayout.EndVertical();


        #endregion
        GUILayout.EndScrollView();
    }
    #endregion


    #region Helper Functions
    private void CreateMap(MapSize mapSize, float hexSize, Material mat)
    {
        DeleteMap();
        _map = Factory.create_map(mapSize, hexSize, mat);
        mapObject = _map.gameObject;
    }

    private void DeleteMap()
    {

        DestroyImmediate(mapObject);
        _map = null;
        mapObject = null;
    }
    #endregion
}
