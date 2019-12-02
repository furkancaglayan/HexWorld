using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif
#pragma warning disable 0168 
#pragma warning disable 0414 

public class _EditorMain : EditorWindow
{
    private float minHeight = 10, maxHeight = 100, rotSpeed = 60, Speed = 10, ScrollSensitivity = .3f;
    //private HexWorldEffect customEffect;
    private Enums.ColorScheme colorScheme = Enums.ColorScheme.COLD;
#region Brush

    private int brushRadius = 1;
    private bool inheritRotation = true;
    private string[] brushes = {"Place", "Delete", "Select", "Rotate Right", "Rotate Left"};

    private string[] brushIcons =
    {
        "Assets/HexWorld/Textures/ToolIcons/Place.png",
        "Assets/HexWorld/Textures/ToolIcons/Delete.png",
        "Assets/HexWorld/Textures/ToolIcons/Select.png",
        "Assets/HexWorld/Textures/ToolIcons/RotateRight.png",
        "Assets/HexWorld/Textures/ToolIcons/RotateLeft.png",
    };

    private string[] brushInfo =
    {
        "Place a Prefab(Shortcut: 3)",
        "Delete and Object from the map (Shortcut: 4)",
        "Select and Object from the map (See above-Shortcut: 5)",
        "Rotate an object by 60 degrees right (With respect to up axis of your object-Shortcut: 6)",
        "Rotate an object by 60 degrees left (With respect to up axis of your object-Shortcut: 7)",
        "Tools won't work if prefabs aren't loaded."
    };

    private bool toggle_help;
    private GUIContent[] brushContents;
    private int selectedBrush = 0;
    private Tile currentSelectedTile;

#endregion

#region Editor

    private readonly Color editorColor = Color.white;
    private Color colorSetOne = new Color(255F / 255F, 255F / 255F, 148F / 255F);
    private Color colorSetTwo = new Color(.388f, .658f, 1);
    private Color windowColor = new Color(241F / 255F, 235F / 255F, 235F / 255F);
    private Vector2 editorScroll;

#endregion

#region MapVariables

    private Enums.MapSize mapSize = Enums.MapSize.Small;
    private float HexSize = 1F;
    private Material gridMat;
    private bool randomRotation = true;
    private GameObject currentGameObject;
    private Enums.RotationType rotationType = Enums.RotationType.Y;
    private HexWorldSerialized _serialized;
    private int saveLoadToolbar = 0;

#endregion

#region MapData

    private Map _map;
    private GameObject mapObject;

    private bool isMapCreated
    {
        get { return _map != null; }
    }

#endregion

#region Prefabs

    private GUIContent[][] prefabContents;
    private GUIContent[] folderContents;

    private bool prefabsLoaded = false;
    private int selectedPrefabFolder = 0;
    private int selectedPrefab = 0;

    private Vector2 prefabsScroll;

#endregion

#region Foldouts

    private bool prefabSectionFoldout = true;
    private bool cameraSectionFoldout = true;
    private bool saveLoadSectionFoldout = true;

#endregion

#region DirectoriesAndSaveLoad

    private string PrefabsDirectory = "Assets/HexWorld/Prefabs/Tiles";
    private string MapsDirectory = "HexWorld/Data/Maps";
    private string MapPrefabDirectory = "HexWorld/Data/Maps";

    private string GameObjectName = "Hexworld_Prefab";
    private string MapName = "Hexworld_MAP";

#endregion

#region HelperFunctions

    private void SaveMapData(string savePath, string mapName, Map data)
    {
        bool valid = Utils.CheckIfDirectoryIsValid(savePath, false);
        if (!valid)
            return;

        if (data == null)
            EditorUtility.DisplayDialog("Map is null.", "Please create a map first.", "Ok");
        else
        {
            if (data.gameObject == null)
                EditorUtility.DisplayDialog("Map is empty.", "It can not be saved.", "Ok");
            else
            {
                if (data.IsEmpty())
                    EditorUtility.DisplayDialog("Map is empty.", "Add some tiles first.", "Ok");
                else
                    _EditorUtils.SaveMap(savePath, mapName, data);
            }
        }
    }
        
    /// <summary>
    /// Unloads Prefabs
    /// </summary>
    private void UnloadPrefabs()
    {
        prefabsLoaded = false;
        prefabContents = null;
    }

    private GUIContent[] CreateBrushContents(string[] names, string[] paths)
    {
        GUIContent[] cons = new GUIContent[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            Texture2D tex = (Texture2D) AssetDatabase.LoadAssetAtPath(paths[i], typeof(Texture2D));
            GUIContent content = new GUIContent(names[i], tex, names[i]);
            cons[i] = content;
        }

        return cons;
    }

    private void CreateMap(Enums.MapSize mapSize, float hexSize, Material mat)
    {
        DeleteMap();
        _map = Factory.create_map(mapSize, hexSize, mat);
        mapObject = _map.gameObject;
    }

    /*private void LoadMap(HexWorldSerialized serialized, Material mat)
    {
        DeleteMap();
        _map = Factory.create_map(serialized, mat);
        mapObject = _map.gameObject;
    }*/

    private void BrushToolsKeyControl()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            try
            {
                switch (Event.current.character)
                {
                    case '3':
                        selectedBrush = 0;
                        break;
                    case '4':
                        selectedBrush = 1;
                        break;
                    case '5':
                        selectedBrush = 2;
                        break;
                    case '6':
                        selectedBrush = 3;
                        break;
                    case '7':
                        selectedBrush = 4;
                        break;
                }
            }
            catch (Exception e)
            {
            }
        }
    }

    private void DeleteMap()
    {
        
        DestroyImmediate(mapObject);
        _map = null;
        mapObject = null;
    }

    /*private HexWorldPrefabSet LoadPrefabs(HexWorldPrefabLoader prefabLoader,string path)
    {
        if (prefabLoader == null)
        {
            prefabsLoaded = false;
            return null;
        }
        if (prefabLoader.hexWorldPrefabSet == null)
        {
            prefabsLoaded = false;
            return null;
        }

        hexWorldPrefabSet = prefabLoader.hexWorldPrefabSet;
        if (prefabsLoaded)
        {
            folderContents = hexWorldPrefabSet.GetFolderContents();
            prefabContents = hexWorldPrefabSet.GetPrefabContents();
        }

        selectedPrefabFolder = 0;
        prefabsLoaded = true;
        return hexWorldPrefabSet;
    }*/

    /// <summary>
    /// For the given MapData, draws to borders with Handles.
    /// </summary>
    /// <param name="hexWorldMap"></param>
    private void GUIDrawBordersAndArea(Map hexWorldMap)
    {
        if (hexWorldMap == null)
            return;
        if (!hexWorldMap.gameObject)
            return;

        Vector3[] corners = hexWorldMap.DefineCorners();

        Vector3 A = corners[0];
        Vector3 B = corners[1];
        Vector3 C = corners[2];
        Vector3 D = corners[3];
        Handles.color = new Color(1, 1, 1, .1F) ;
        //Handles.DrawAAConvexPolygon(corners);

        Handles.color = Color.red;
        Handles.DrawLine(A, B);
        Handles.DrawLine(C, D);
        Handles.color = Color.blue;
        Handles.DrawLine(D, A);
        Handles.DrawLine(B, C);
    }

#endregion

#region Builtin Functions

    [MenuItem("HexWorld/Map GeneratorOld", priority = -1)]
    public static void Init()
    {
        _EditorUtils.AddTag("HexWorld");
        _EditorMain window = (_EditorMain) GetWindow(typeof(_EditorMain));

        //IconPack.Load();
        window.minSize.Set(250, 600);
        window.autoRepaintOnSceneChange = true;
        //window.titleContent = new GUIContent("HexWorld", IconPack.GetHexworldEditorLogo());
        window.Show();
    }

    public void OnEnable()
    {
        //hexWorldPrefabSet = LoadPrefabs(PrefabsDirectory);
#if UNITY_2019_1_OR_NEWER
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
#endif
        brushContents = CreateBrushContents(brushes, brushIcons);
    }

    void OnDestroy()
    {
        if (_map != null)
        {
            if (_map.gameObject != null)
            {
                if (!_map.IsEmpty())
                {

                    int chosen = EditorUtility.DisplayDialogComplex("Exiting..", "Want to save map before leaving?",
                        "Yes, please", "No", "Come again?");
                    while (chosen == 2)
                        chosen = EditorUtility.DisplayDialogComplex("Exiting..", "Want to save map before leaving?",
                            "Yes, please", "No", "Come again?");
                    if (chosen == 0)
                        SaveMapData("Assets/" + MapsDirectory, MapName, _map);
                }

            }
        }

        DeleteMap();
#if UNITY_2019_1_OR_NEWER
         SceneView.duringSceneGui -= this.OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
#endif
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


#region MapCreation

        editorScroll = GUILayout.BeginScrollView(editorScroll);
        GUILayout.BeginVertical();
        //GUILayout.Space(20);
        GUI.color = windowColor;


        GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(140));
        GUILayout.Label("Create Maps", fontStyle);
        GUILayout.Space(10);
        GUI.color = new Color(255F / 255F, 211F / 255F, 89F / 255F, .8F);


        GUI.color = new Color(windowColor.r, windowColor.g, windowColor.b, .88f);

        GUILayout.Space(10);


        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Map Size", txtStyle, GUILayout.Width(80));
        mapSize = (Enums.MapSize) EditorGUILayout.EnumPopup(mapSize, GUILayout.Width(position.width - 120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Hex Radius", txtStyle, GUILayout.Width(80));
        HexSize = EditorGUILayout.FloatField(HexSize, EditorStyles.helpBox, GUILayout.Width(position.width - 120));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal(GUILayout.Width(position.width - 20));
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Color One", txtStyle, GUILayout.Width(80));
        colorSetOne = EditorGUILayout.ColorField(new GUIContent(""),colorSetOne);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Color Two", txtStyle, GUILayout.Width(80));
        colorSetTwo = EditorGUILayout.ColorField(new GUIContent(""), colorSetTwo,GUILayout.Width(120));
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Background", txtStyle, GUILayout.Width(80));
        windowColor = EditorGUILayout.ColorField(windowColor, GUILayout.Width(position.width - 120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Grid Material", txtStyle, GUILayout.Width(80));
        gridMat = (Material) EditorGUILayout.ObjectField(gridMat, typeof(Material), false,
            GUILayout.Width(position.width - 120));
        GUILayout.Space(10);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        GUI.color = colorSetOne;
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        if (GUILayout.Button("Create Map", EditorStyles.toolbarButton, GUILayout.Width(position.width - 40)))
            CreateMap(mapSize, HexSize, gridMat);
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
                    else if (chosen == 2)
                        SaveMapData("Assets/" + MapsDirectory, MapName, _map);
                }
            }
           
            DeleteMap();

        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(20);


        GUILayout.EndVertical();

#endregion
#region PrefabSection
        GUI.color = editorColor;
        GUI.backgroundColor = windowColor;

        GUILayout.Space(20);

        if (prefabSectionFoldout)
            GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(140));
        else
            GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(40));
        GUI.backgroundColor = editorColor;
        prefabSectionFoldout = EditorGUILayout.Foldout(prefabSectionFoldout,
            new GUIContent("Brush&Prefabs"), true);
        if (!prefabSectionFoldout)
            GUILayout.Label("Use this section to paint and create maps");
        else
        {
            GUILayout.Label("Prefabs", headingStyle);
            GUILayout.Space(10);


            GUILayout.BeginHorizontal();
            GUILayout.Label("Brush Radius", txtStyle, GUILayout.Width(150));
            brushRadius = EditorGUILayout.IntSlider(brushRadius,1,5,
                GUILayout.Width(position.width - 180));
            GUILayout.EndHorizontal();

           

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation Axis", txtStyle, GUILayout.Width(150));
            rotationType =
                (Enums.RotationType) EditorGUILayout.EnumPopup(rotationType, GUILayout.Width(position.width - 180));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Enable Random Rotation", txtStyle, GUILayout.Width(150));
            randomRotation = GUILayout.Toggle(randomRotation, "", GUILayout.Width(position.width - 180));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Inherit Rotation", txtStyle, GUILayout.Width(150));
            inheritRotation = GUILayout.Toggle(inheritRotation, "", GUILayout.Width(position.width - 180));
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();
            GUILayout.Label("Current GameObject", txtStyle, GUILayout.Width(150));
            currentGameObject = (GameObject) EditorGUILayout.ObjectField(currentGameObject, typeof(GameObject), false,
                GUILayout.Width(position.width - 180));
            GUILayout.EndHorizontal();

            /*GUILayout.BeginHorizontal();
            GUILayout.Label("Current Prefab", txtStyle, GUILayout.Width(150));
            currentPrefab = (Object)EditorGUILayout.ObjectField(currentPrefab, typeof(Object), false, GUILayout.Width(position.width - 180));
            GUILayout.EndHorizontal();*/

            GUILayout.BeginHorizontal();
            GUILayout.Label("Prefabs Directory", txtStyle, GUILayout.Width(150));
            PrefabsDirectory = GUILayout.TextField(PrefabsDirectory, GUILayout.Width(position.width - 220));
            if (GUILayout.Button(new GUIContent("F", "Get Default Directory"), EditorStyles.toolbarButton,
                GUILayout.Width(30)))
            {
                PrefabsDirectory = "Assets/HexWorld/Prefabs/Tiles";
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal(GUILayout.Width(position.width - 20));
            GUILayout.Space(10);
            GUI.color = colorSetTwo;

            if (GUILayout.Button("Load Prefabs", EditorStyles.toolbarButton,
                GUILayout.Width((position.width - 40) / 2)))
            {
                //hexWorldPrefabSet = LoadPrefabs(loader,PrefabsDirectory);
            }

            GUI.color = Color.red;

            if (GUILayout.Button("Remove Prefabs", EditorStyles.toolbarButton,
                GUILayout.Width((position.width - 40) / 2)))
            {
                UnloadPrefabs();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("Tools", EditorStyles.miniBoldLabel);

            GUI.backgroundColor = editorColor;
            GUI.color = editorColor;


        
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            selectedBrush = GUILayout.Toolbar(selectedBrush, brushContents, EditorStyles.toolbarButton,
                GUILayout.Width(position.width - 40));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            toggle_help = GUILayout.Toggle(toggle_help, "Toggle Help", EditorStyles.toolbarButton,
                GUILayout.Width(position.width - 40));
            GUILayout.EndHorizontal();

            if (toggle_help)
            {
                try
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(position.width - 40));
                    for (int k = 0; k < brushInfo.Length; k++)
                    {
                        GUILayout.BeginHorizontal();
                        if (k < brushIcons.Length)
                            GUILayout.Label(brushContents[k].image, GUILayout.Width(32), GUILayout.Height(32));
                        GUILayout.Label(brushInfo[k], EditorStyles.miniBoldLabel);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                catch (UnityException e)
                {
                }
            }

            GUILayout.EndVertical();


            if (prefabsLoaded)
            {
                GUI.color = windowColor;
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                selectedPrefabFolder = GUILayout.Toolbar(selectedPrefabFolder, folderContents, toolbarStyle,
                    GUILayout.Width(position.width - 40));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                prefabsScroll = GUILayout.BeginScrollView(prefabsScroll, GUI.skin.box, GUILayout.MinHeight(250),
                    GUILayout.MaxWidth(position.width - 20)
                    , GUILayout.Width(position.width - 20));


                GUI.backgroundColor = new Color(210F / 255F, 220F / 255F, 240F / 255F);

                if (prefabContents != null)
                {
                    if (prefabContents[selectedPrefabFolder].Length != 0)
                    {
                        selectedPrefab = GUILayout.SelectionGrid(selectedPrefab, prefabContents[selectedPrefabFolder],
                            3,
                            prefabStyle, GUILayout.Width(position.width - 40));
                    }

                    else
                    {
                        GUILayout.BeginVertical();
                        GUILayout.Label("No prefabs are present at : " + folderContents[selectedPrefabFolder].tooltip,
                            GUILayout.Width(position.width - 40));
                        GUILayout.Label(
                            "Enter a valid 'Prefabs Directory' and click on 'Load Prefabs'.\nThis will load the assets that are in the path.");
                        GUILayout.Space(20);
                        GUILayout.BeginHorizontal();
                        /*GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
                        GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
                        GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
                        GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));*/
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                }

                else
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label("No prefabs are present at : " + folderContents[selectedPrefabFolder].tooltip,
                        GUILayout.Width(position.width - 40));
                    GUILayout.Label(
                        "Enter a valid 'Prefabs Directory' and click on 'Load Prefabs'.\nThis will load the assets that are in the path.");
                    GUILayout.Space(20);
                    GUILayout.BeginHorizontal();
                    /*GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
                    GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
                    GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));
                    GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));*/
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }


                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }
            else
                GUILayout.Space(10);

            GUI.color = windowColor;
            GUI.backgroundColor = windowColor;


            GUILayout.Space(10);
            GUILayout.Label("Easy Maps", headingStyle);

            
           /* if (selectedPrefab != -1 && selectedPrefabFolder != -1 && hexWorldPrefabSet != null)
            {
                bool validPrefabSelected = hexWorldPrefabSet.ValidIndices(selectedPrefabFolder, selectedPrefab);
                if (validPrefabSelected)
                {
                    HexWorldPrefab selectedHexWorldPrefab = hexWorldPrefabSet.Get(selectedPrefabFolder, selectedPrefab);

                    GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.MaxWidth(position.width - 40));
                    if (selectedHexWorldPrefab.content.image == null)
                        GUILayout.Label(IconPack.GetHexworldEditorLogo(), EditorStyles.helpBox);
                    else
                        GUILayout.Label(selectedHexWorldPrefab.content.image);
                    GUILayout.Space(44);
                    GUI.color = colorSetTwo;
                    GUILayout.BeginVertical(GUILayout.MaxWidth(position.width - 40));
                    GUILayout.Space(44);
                    GUILayout.Label("Selected Prefab: " + selectedHexWorldPrefab.content.text
                        , fontStyle);
                    if (GUILayout.Button("Take a closer look", toolbarStyle, GUILayout.Width(position.width - 296)))
                    {
                        _EditorModelViewer.Init(selectedHexWorldPrefab.@object);
                    }

                    if (GUILayout.Button("Fill all with selected prefab", toolbarStyle,
                        GUILayout.Width(position.width - 296)))
                    {
                        if (_map != null)
                            _map.FillMap(selectedHexWorldPrefab, rotationType, randomRotation);
                        else
                        {
                            EditorUtility.DisplayDialog("Null Reference Exception", "Create a map first.", "Ok");
                        }
                    }

                    if (GUILayout.Button("Fill empty space with selected prefab", toolbarStyle,
                        GUILayout.Width(position.width - 296)))
                    {
                        if (_map != null)
                            _map.FillEmptyTiles(selectedHexWorldPrefab, rotationType, randomRotation);
                        else
                        {
                            EditorUtility.DisplayDialog("Null Reference Exception", "Create a map first.", "Ok");
                        }
                    }

                    GUILayout.EndVertical();
                    GUI.color = editorColor;
                    GUILayout.EndHorizontal();
                }
                
            }
            else
            {
                GUILayout.Label("Load prefabs to continue");
                GUILayout.Space(10);
            }*/
        }


        GUILayout.EndVertical();

#endregion
#region SaveLoadSection

        GUI.color = editorColor;
        GUILayout.Space(20);
        GUI.backgroundColor = windowColor;
        if (saveLoadSectionFoldout)
            GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(140));
        else
            GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(40));
        GUI.backgroundColor = editorColor;


        saveLoadSectionFoldout = EditorGUILayout.Foldout(saveLoadSectionFoldout,
            new GUIContent("Save&Load", AssetPreview.GetMiniTypeThumbnail(typeof(GameObject))), true);
        GUILayout.Space(5);
        if (!saveLoadSectionFoldout)
            GUILayout.Label("Use this section to save and load");
        else
        {
            GUILayout.BeginVertical(GUILayout.Height(186), GUILayout.MaxHeight(186),
                GUILayout.Width(position.width - 40));

            GUILayout.BeginHorizontal(GUI.skin.box);
            GUI.color = new Color(240F/255, 240F / 255, 240F / 255);
            saveLoadToolbar = GUILayout.Toolbar(saveLoadToolbar, new[] {"Save", "Load"}, EditorStyles.toolbarButton,
                GUILayout.Width(position.width - 40));
            GUI.color = editorColor;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (saveLoadToolbar == 1)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal(GUILayout.Width(position.width - 40));
                GUILayout.Space(10);
                GUILayout.Label("Static Map", txtStyle, GUILayout.Width(80));
                _serialized = (HexWorldSerialized) EditorGUILayout.ObjectField(_serialized, typeof(HexWorldSerialized),
                    false, GUILayout.Width(position.width - 135));
                GUILayout.EndHorizontal();


                if (_serialized == null)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(position.width - 40));
                    GUILayout.Space(10);
                    GUILayout.Label("Drag&Drop your saved maps to continue painting.", txtStyle,
                        GUILayout.Width(position.width - 40));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(GUILayout.Width(position.width - 40));
                    GUILayout.Space(position.width / 2-64);
                    //GUILayout.Label(IconPack.GetHexworldEditorLogo(), txtStyle,
                        //GUILayout.Width(128));
                    GUILayout.EndHorizontal();

                }
                else
                {
                    if (_serialized.GetMapData() == null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.Label("Seems like your map is empty. Maybe it is corrupted?", txtStyle);
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.BeginVertical();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.Label("Map Name: <b>" + _serialized + "</b>", txtStyle);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.Label("Description: <b>" + _serialized.GetDescription() + "</b>", txtStyle);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.Label("Map Size: <b>" + _serialized.GetMapData().mapSize + "</b>", txtStyle);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.Label("Hex Size: <b>" + _serialized.GetMapData().hexRadius + "</b>", txtStyle);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.Label("File Size: <b>" + _serialized.GetSize().ToString() + "</b> kb", txtStyle);
                        GUILayout.EndHorizontal();


                        GUILayout.Space(10);

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUI.color = colorSetOne;
                        /*if (GUILayout.Button("Load To Scene", EditorStyles.toolbarButton,
                            GUILayout.Width(position.width - 60)))
                            LoadMap(_serialized, gridMat);*/
                        GUI.color = editorColor;
                        GUILayout.EndHorizontal();

                        GUILayout.EndVertical();
                    }
                }
            }

            if (saveLoadToolbar == 0)
            {
                GUILayout.BeginVertical();
                //.Label(new GUIContent("Save Directory", IconPack.GetHexworldEditorLogo()),
                    //EditorStyles.boldLabel, GUILayout.Height(32), GUILayout.Width(200));
                GUILayout.BeginHorizontal();
                GUILayout.Label("Assets/", EditorStyles.miniLabel, GUILayout.Width(45));
                MapsDirectory = GUILayout.TextField(MapsDirectory, EditorStyles.textField,
                    GUILayout.Width(position.width - 380));
                GUI.color = colorSetOne;
                if (GUILayout.Button("Check Directory", EditorStyles.toolbarButton, GUILayout.Width(290)))
                    Utils.CheckIfDirectoryIsValid("Assets/" + MapsDirectory, true);
                GUI.color = editorColor;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Name:", EditorStyles.miniLabel, GUILayout.Width(45));
                MapName = GUILayout.TextField(MapName, EditorStyles.textField, GUILayout.Width(position.width - 380));
                GUI.color = colorSetOne;
                if (GUILayout.Button("Save Map", EditorStyles.toolbarButton, GUILayout.Width(290)))
                    SaveMapData("Assets/" + MapsDirectory, MapName, _map);
                GUI.color = editorColor;
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.Space(19);





                GUILayout.BeginVertical();
                GUILayout.Label(
                    new GUIContent("Export As Prefab", AssetPreview.GetMiniTypeThumbnail(typeof(GameObject))),
                    EditorStyles.boldLabel, GUILayout.Height(32), GUILayout.Width(200));
                GUILayout.BeginHorizontal();
                GUILayout.Label("Assets/", EditorStyles.miniLabel, GUILayout.Width(45));
                MapPrefabDirectory = GUILayout.TextField(MapPrefabDirectory, EditorStyles.textField,
                    GUILayout.Width(position.width - 380));
                GUI.color = colorSetOne;
                if (GUILayout.Button("Check Directory", EditorStyles.toolbarButton, GUILayout.Width(290)))
                    Utils.CheckIfDirectoryIsValid("Assets/" + MapPrefabDirectory, true);
                GUI.color = editorColor;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Name:", EditorStyles.miniLabel, GUILayout.Width(45));
                GameObjectName = GUILayout.TextField(GameObjectName, EditorStyles.textField,
                    GUILayout.Width(position.width - 380));
                GUI.color = colorSetOne;
                if (GUILayout.Button("Save as GameObject", EditorStyles.toolbarButton, GUILayout.Width(290)))
                    _EditorUtils.SavePrefab(GameObjectName, MapPrefabDirectory, _map);
                GUI.color = editorColor;
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();

#endregion
#region CameraSection

        GUI.color = editorColor;
        GUILayout.Space(20);

        GUI.backgroundColor = windowColor;
        if (cameraSectionFoldout)
            GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(140));
        else
            GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(40));
        GUI.backgroundColor = editorColor;
        cameraSectionFoldout = EditorGUILayout.Foldout(cameraSectionFoldout,
            new GUIContent("Rendering", AssetPreview.GetMiniTypeThumbnail(typeof(Light))), true);
       
        if (cameraSectionFoldout)
        {

            GUILayout.BeginVertical(GUILayout.Width(position.width-40));

            GUILayout.BeginVertical();
            GUIContent cameraContent = new GUIContent("Camera", AssetPreview.GetMiniTypeThumbnail(typeof(Camera)));


            GUILayout.BeginHorizontal();
            GUILayout.Label(cameraContent, EditorStyles.boldLabel, GUILayout.Height(32), GUILayout.Width(100));
            EditorGUILayout.HelpBox("You can use our camera to roam around the map and use it in your game", MessageType.Info);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Min Height:", EditorStyles.miniLabel, GUILayout.Width(100));
            minHeight = EditorGUILayout.FloatField(minHeight, EditorStyles.helpBox);
            GUILayout.Label("Max Height:", EditorStyles.miniLabel, GUILayout.Width(100));
            maxHeight = EditorGUILayout.FloatField(maxHeight, EditorStyles.helpBox);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Camera Speed:", EditorStyles.miniLabel, GUILayout.Width(100));
            Speed = EditorGUILayout.FloatField(Speed, EditorStyles.helpBox);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Rotation Speed:", EditorStyles.miniLabel, GUILayout.Width(100));
            rotSpeed = EditorGUILayout.FloatField(rotSpeed, EditorStyles.helpBox);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Scroll Sensitivity:", EditorStyles.miniLabel, GUILayout.Width(100));
            ScrollSensitivity = EditorGUILayout.FloatField(ScrollSensitivity, EditorStyles.helpBox);
            GUILayout.EndHorizontal();
            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUI.color = colorSetTwo;
            if (GUILayout.Button("Add Camera Controller", EditorStyles.toolbarButton, GUILayout.Width(position.width-40)))
            {
               // Utils.AddCameraController(minHeight,maxHeight,rotSpeed,Speed,ScrollSensitivity);
            }
            GUI.color = editorColor;
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();


            GUILayout.Space(15);

            GUILayout.EndVertical();
        }
        if (!cameraSectionFoldout)
            GUILayout.Label("Use this section to add strategy camera and post processing stack");
        else
        {
            GUIContent ppContent = null;
            Texture2D PPlogo = null;
            GUILayout.BeginHorizontal(GUILayout.Width(position.width - 40));

            ppContent = new GUIContent("Rendering ", PPlogo);

            GUILayout.Label(ppContent, EditorStyles.boldLabel, GUILayout.Height(32), GUILayout.Width(100));
            EditorGUILayout.HelpBox("You can use premade Light and Post Processing settings", MessageType.Info);
            GUILayout.EndHorizontal();
#if UNITY_2018_1_OR_NEWER && UNITY_POST_PROCESSING_STACK_V2
            GUILayout.BeginHorizontal();
            GUILayout.Label("Color choice", txtStyle, GUILayout.Width(100));
            colorScheme =
                (Enums.ColorScheme)EditorGUILayout.EnumPopup(colorScheme, EditorStyles.toolbarDropDown, GUILayout.Width(position.width - 336));
            if (GUILayout.Button("Add Effects", EditorStyles.toolbarButton, GUILayout.Width(194)))
            {
                HexWorldEffect effect = GetEffect(colorScheme);
                _EditorUtils.AddEffect(effect);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Use Custom", txtStyle, GUILayout.Width(100));
            customEffect =
                (HexWorldEffect)EditorGUILayout.ObjectField(customEffect,typeof(HexWorldEffect),false, GUILayout.Width(position.width - 340));
            if (GUILayout.Button("Add Effects", EditorStyles.toolbarButton, GUILayout.Width(194)))
            {
                _EditorUtils.AddEffect(customEffect);
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(10);
#elif !UNITY_2018_1_OR_NEWER

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("This property can be used in Unity versions 2018.1 or newer.");
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
#elif !UNITY_POST_PROCESSING_STACK_V2
GUILayout.Space(10);
           GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Please install Post-Processing Stack from Package Manager or AssetStore");
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            


#endif
        }

        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Space(position.width/2-15);
        GUILayout.Label("END", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(position.width / 2 - 26);
        if (GUILayout.Button("Go Top",EditorStyles.toolbarButton,GUILayout.Width(50)))
        {
            editorScroll = new Vector2(0, 0);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndScrollView();

#endregion




        Repaint();
    }


    void OnSceneGUI(SceneView sceneView)
    {
        BrushToolsKeyControl();
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        if (_map != null)
            if (!_map.gameObject)
                DeleteMap();
        if (isMapCreated)
            GUIDrawBordersAndArea(_map);
        else
            return;


        RaycastHit hit;
        int rayMask = (3 << 8);
        bool rayCast = Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, rayMask);
        if (rayCast)
        {
            if (hit.transform.tag.Equals("HexWorld"))
            {
                Vector3 mouseLoc = new Vector3(hit.point.x, 0, hit.point.z);

                Chunk currentChunk = _map.FindChunkWithPos(mouseLoc);
                if (currentChunk == null)
                    return;
                currentSelectedTile = currentChunk.Position2Tile(mouseLoc);

                /*if (selectedPrefab != -1 && selectedPrefabFolder != -1 && hexWorldPrefabSet != null)
                {
                    bool validPrefabSelected = hexWorldPrefabSet.ValidIndices(selectedPrefabFolder, selectedPrefab);
                    if (!validPrefabSelected)
                        selectedPrefab = 0;
                    if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDrag ||
                        Event.current.type == EventType.MouseDown)
                    {
                        if (Event.current.button == 0 &&
                            (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown))
                        {
                            /*_EditorBrush.ApplyStroke((Enums.BrushType) selectedBrush, currentSelectedTile, currentChunk,
                                hexWorldPrefabSet.Get(selectedPrefabFolder, selectedPrefab),
                                randomRotation, rotationType, out currentGameObject);*/
                /*
                                            HexWorldBrush.ApplyStroke(currentSelectedTile,
                                                hexWorldPrefabSet.Get(selectedPrefabFolder, selectedPrefab)
                                                , currentChunk, _map, (Enums.BrushType) selectedBrush, brushRadius, randomRotation, inheritRotation,
                                                rotationType);
                                        }

                                        SceneView.RepaintAll();
                                        Repaint();
                                    }
                                }

                                if (currentSelectedTile != null)
                                    _EditorBrush.DrawBrush(currentSelectedTile,_map, (Enums.BrushType) selectedBrush, HexSize,brushRadius);*/
                SceneView.RepaintAll();
                Repaint();
            }
        }


        //TODO:Check if this is costly.
        sceneView.Repaint();
        Repaint();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    /*private HexWorldEffect GetEffect(Enums.ColorScheme scheme)
    {
        string path = "Assets/HexWorld/Data/Effects/HexWorldEffects/";
        switch (scheme)
        {
            case Enums.ColorScheme.COLD:
                path += "Cold.asset";
                break;
            case Enums.ColorScheme.HOT:
                path += "Hot.asset";
                break;
            case Enums.ColorScheme.SUNNY:
                path += "Sunny.asset";
                break;
            case Enums.ColorScheme.DARK:
                path += "Dark.asset";
                break;
        }

        HexWorldEffect effect = (HexWorldEffect)AssetDatabase.LoadAssetAtPath(path, typeof(HexWorldEffect));
        return effect;
    }*/

#endregion
}