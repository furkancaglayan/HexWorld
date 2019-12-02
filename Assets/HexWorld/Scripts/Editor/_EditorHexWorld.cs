using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MapSize = HexWorld.Enums.MapSize;

namespace HexWorld
{
    public class _EditorHexWorld : EditorWindow
    {
        #region Fields
        #region Editor
        private readonly Color _editorColor = Color.white;
        private Color _colorSetOne = new Color(255F / 255F, 255F / 255F, 148F / 255F);
        private Color _colorSetTwo = new Color(.388f, .658f, 1);
        private Color _windowColor = new Color(241F / 255F, 235F / 255F, 235F / 255F);
        private Vector2 _editorScrollVal;
        private bool prefabSectionFoldout;

        #endregion

        #region StylingOptions
        GUISkin globalSceneSkin;
        GUIStyle YellowBox => globalSceneSkin.customStyles[142];
        GUIStyle RedBox => globalSceneSkin.customStyles[146];
        GUIStyle BlueBox => globalSceneSkin.customStyles[136];
        GUIStyle GrayBox => globalSceneSkin.customStyles[134];
        GUIStyle GrayBoxHighLighted => globalSceneSkin.customStyles[135];
        GUIStyle OrangeBox => globalSceneSkin.customStyles[144];
        GUIStyle SeaGreenBox => globalSceneSkin.customStyles[138];
        GUIStyle GrayButton => globalSceneSkin.customStyles[515];

        #endregion
        #region Map
        private Material _gridMaterial;
        private Map _map;
        private GameObject mapObject;
        private MapSize _mapSize = MapSize.Small;
        private float _hexSize = 1F;
        private bool IsMapCreated => _map != null;
        #endregion
        #region Directories
        #endregion
        #region BrushNPrefabs

        private int brushRadius = 1;
        private bool randomRotation = true;
        private bool inheritRotation;
        private TileSet _tileSet;
        private GameObject currentGameObject;
        private GUIContent[] folderContents;
        private GUIContent[][] prefabContents;
        private int selectedFolder = 0;
        private int selectedPrefab = 0;
        private Vector2 _prefabsScrollVal;
        #endregion
        #endregion
        #region Configuration

        private static EditorConfiguration _configuration;
        #endregion
        #region Built-in Functions
        [MenuItem("HexWorld/Map Generator", priority = -1)]
        public static void Init()
        {
            _EditorUtility.AddTag("HexWorld");
            _EditorHexWorld window = (_EditorHexWorld)GetWindow(typeof(_EditorHexWorld));
            _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));

            window.autoRepaintOnSceneChange = true;
            window.titleContent = new GUIContent("HexWorld", _configuration.hexWorldLogo);
            window.Show();
        }

        private void OnEnable()
        {
#if UNITY_2019_1_OR_NEWER
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
#endif


            globalSceneSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
            // brushContents = CreateBrushContents(brushes, brushIcons);
        }
        void OnSceneGUI(SceneView sceneView)
        {
            //BrushToolsKeyControl();
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (_map != null)
                if (!_map.gameObject)
                    DeleteMap();
            if (IsMapCreated)
                GuiDrawBordersAndArea(_map);
            else
                return;

            /*
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

                   if (selectedPrefab != -1 && selectedPrefabFolder != -1 && hexWorldPrefabSet != null)
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
                                _EditorBrush.DrawBrush(currentSelectedTile,_map, (Enums.BrushType) selectedBrush, HexSize,brushRadius);
            SceneView.RepaintAll();
            Repaint();
        }
    }*/


            //TODO:Check if this is costly.
            sceneView.Repaint();
            Repaint();
        }
        private void OnGUI()
        {

            if (_configuration == null)
                _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));

            if (globalSceneSkin == null)
                globalSceneSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
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
                richText = true,
                wordWrap = true
            };
            GUIStyle headingStyle = new GUIStyle(EditorStyles.miniBoldLabel)
            {
                fontSize = 14,
                fontStyle = FontStyle.BoldAndItalic,
            };


            GUIStyle prefabStyle = new GUIStyle(globalSceneSkin.customStyles[50])
            {
                imagePosition = ImagePosition.ImageAbove,

                padding = new RectOffset(0, 0, 32, 0),
                font = EditorStyles.miniBoldFont,
                fontStyle = FontStyle.Italic,
                fontSize = 12,

            };

            GUIStyle toolbarStyle = new GUIStyle(GrayButton)
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
            _colorSetOne = EditorGUILayout.ColorField(new GUIContent(""), _colorSetOne, GUILayout.Width(120));
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
            #region Brush&Prefabs
            GUI.color = _editorColor;
            GUI.backgroundColor = _windowColor;

            GUILayout.Space(20);

            if (prefabSectionFoldout)
                GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(140),
                    GUILayout.Width(position.width));
            else
                GUILayout.BeginVertical(currentStyle, GUILayout.Height(20), GUILayout.MaxHeight(40),
                    GUILayout.Width(position.width));
            GUI.backgroundColor = _editorColor;
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
                brushRadius = EditorGUILayout.IntSlider(brushRadius, 1, 5,
                    GUILayout.Width(position.width - 180));
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
                currentGameObject = (GameObject)EditorGUILayout.ObjectField(currentGameObject, typeof(GameObject), false,
                    GUILayout.Width(position.width - 180));
                GUILayout.EndHorizontal();

                /*GUILayout.BeginHorizontal();
                GUILayout.Label("Current Prefab", txtStyle, GUILayout.Width(150));
                currentPrefab = (Object)EditorGUILayout.ObjectField(currentPrefab, typeof(Object), false, GUILayout.Width(position.width - 180));
                GUILayout.EndHorizontal();*/


                GUILayout.BeginHorizontal();
                GUILayout.Label("Tile Set:", txtStyle, GUILayout.Width(150));
                _tileSet = (TileSet)EditorGUILayout.ObjectField(_tileSet, typeof(TileSet), false,
                    GUILayout.Width(position.width - 180));
                GUILayout.EndHorizontal();


                if (_tileSet != null)
                {
                    /* GUILayout.BeginHorizontal();
                     GUILayout.Space(10);
                     GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(position.width-40));



                     GUILayout.BeginVertical();


                     GUILayout.BeginHorizontal();
                     GUILayout.Label("Tileset Name:", txtStyle, GUILayout.Width(100));
                     GUILayout.Label(_tileSet.name, txtStyle, GUILayout.Width(position.width - 180));
                     if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), EditorStyles.boldLabel, GUILayout.Width(24)))
                         Debug.LogError("Tusa vastın");
                     GUILayout.EndHorizontal();


                     GUILayout.BeginHorizontal();
                     GUILayout.Label("Tileset Type:", txtStyle, GUILayout.Width(100));
                     string type = "Combined";
                     if (_tileSet.GetType() == typeof(LayeredTileSet))
                         type = "Layered";
                     GUILayout.Label(type, txtStyle, GUILayout.Width(position.width - 180));
                     GUILayout.EndHorizontal();

                     GUILayout.BeginHorizontal();
                     GUILayout.Label("Tileset Ecosystem:", txtStyle, GUILayout.Width(100));
                     GUILayout.Label(_tileSet.ecosystem, txtStyle, GUILayout.Width(position.width - 180));
                     GUILayout.EndHorizontal();

                     GUILayout.EndVertical();



                     GUILayout.EndVertical();

                     GUILayout.EndHorizontal();*/

                }
                else
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUIContent gUIContent = new GUIContent("Please assign a valid Tileset or create a new one in TileSet Generator.", _configuration.coloredBirchGamesLogo);
                    GUILayout.Label(gUIContent, txtStyle, GUILayout.Width(position.width - 20), GUILayout.Height(48));
                    GUILayout.EndVertical();


                }
                GUILayout.Space(10);


                if (_tileSet != null && folderContents != null && prefabContents != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);

                    GUILayout.BeginVertical(OrangeBox, GUILayout.Width(position.width - 65));
                    selectedFolder = GUILayout.Toolbar(selectedFolder, folderContents, GrayButton, GUILayout.Width(position.width - 55));
                    _prefabsScrollVal = GUILayout.BeginScrollView(_prefabsScrollVal, GUILayout.MinHeight(500), GUILayout.MaxHeight(500), GUILayout.Height(500));

                    GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(position.width - 55));

                    if (selectedFolder > prefabContents.Length)
                        selectedFolder = 0;
                    GUIContent[] currentContents = prefabContents[selectedFolder];
                    if (selectedPrefab > currentContents.Length)
                        selectedPrefab = 0;
                    selectedPrefab = GUILayout.SelectionGrid(selectedPrefab, currentContents, 3,
                    prefabStyle, GUILayout.Width(position.width - 60));
                    GUILayout.EndVertical();

                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                }




                GUILayout.BeginHorizontal(GUILayout.Width(position.width - 20));
                GUILayout.Space(10);
                GUI.color = _colorSetOne;

                if (GUILayout.Button("Import Tileset", EditorStyles.toolbarButton,
                    GUILayout.Width((position.width - 40) / 2)))
                {
                    folderContents = ImportTileSet(_tileSet);
                    //hexWorldPrefabSet = LoadPrefabs(loader,PrefabsDirectory);
                }


                if (GUILayout.Button("Go to Tileset", EditorStyles.toolbarButton,
                    GUILayout.Width((position.width - 40) / 2)))
                    _EditorUtility.FocusOnObject(_tileSet);

                GUILayout.EndHorizontal();
                GUILayout.Space(20);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            #endregion


            GUILayout.EndScrollView();
        }
        #endregion
        #region Helper Functions

        private GUIContent[] ImportTileSet(TileSet set)
        {
            if (set == null)
            {
                _EditorPopups.ShowMessage("Tileset is null!", "Please assign a valid Tileset.");
                return null;

            }

            if (set.GetPropCount() == 0)
            {
                _EditorPopups.ShowMessage("Tileset has no props!", "Please assign a valid Tileset.");
                return null;
            }
            int folderCount = typeof(CombinedTileSet) == set.GetType() ? ((CombinedTileSet)set).folders.Count : 4;
            prefabContents = new GUIContent[folderCount][];
            for (int i = 0; i < folderCount; i++)
                prefabContents[i] = set.GetFileContentsInFolder(i);

            return set.GetFolderContents();
        }
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

        /// <summary>
        /// For the given Map, draws to borders with Handles.
        /// </summary>
        /// <param name="hexWorldMap"></param>
        private void GuiDrawBordersAndArea(Map hexWorldMap)
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
            Handles.color = new Color(1, 1, 1, .1F);
            //Handles.DrawAAConvexPolygon(corners);

            Handles.color = Color.red;
            Handles.DrawLine(A, B);
            Handles.DrawLine(C, D);
            Handles.color = Color.blue;
            Handles.DrawLine(D, A);
            Handles.DrawLine(B, C);
        }
        #endregion
    }
}
