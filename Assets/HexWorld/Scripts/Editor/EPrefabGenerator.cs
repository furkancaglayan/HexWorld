using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
public class EPrefabGenerator : EditorWindow
{
    [MenuItem("HexWorld/Prefab Generator")]
    static void Init()
    {
        EPrefabGenerator window = (EPrefabGenerator)GetWindow(typeof(EPrefabGenerator));
        window.autoRepaintOnSceneChange = true;
        window.minSize = new Vector2(1000,600);
        window.maxSize = window.minSize;
        window.Show(false);

    }
 
    private Vector2 _mainScrollView;
    private PlaceHolderTile _tile;
    private string path = "Assets";
    private string saveName = "prefab1";

    private GameObject gameObject;
    private Editor gameObjectEditor;
    private void OnEnable()
    {
        _tile = new PlaceHolderTile();
    }


   

    private void OnGUI()
    {
        GUIStyle labelstyle = new GUIStyle(EditorStyles.miniBoldLabel)
        {
            fontSize=10,
            margin=new RectOffset(0,0,2,0),
        };

        GUILayout.Label("HexWorld Prefab Generator", labelstyle);



        _mainScrollView=GUILayout.BeginScrollView(_mainScrollView);
        ShowHeaders();


      


        ShowPrefabSelection();


        Rect settings = new Rect(60,240,400,320);
        
        GUILayout.BeginArea(settings,EditorStyles.helpBox);

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Path:",labelstyle,GUILayout.Width(80));
        path=GUILayout.TextField(path,EditorStyles.helpBox);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", labelstyle, GUILayout.Width(80));
        saveName = GUILayout.TextField(saveName, EditorStyles.helpBox);
        GUILayout.EndHorizontal();


        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Combine", EditorStyles.toolbarButton))
            gameObject=_EditorPrefabUtility.Combine(_tile,path,saveName);
        GUILayout.Button("Clear", EditorStyles.toolbarButton);
        GUILayout.EndHorizontal();



        GUILayout.EndVertical();
        GUILayout.EndArea();


        Rect preview = new Rect(520, 240, 400, 320);
        GUILayout.BeginArea(preview, EditorStyles.helpBox);
        ShowPreview();
        GUILayout.EndArea();

        GUILayout.EndScrollView();

    }

    private void ShowPrefabSelection()
    {
        GUIStyle txt = new GUIStyle(EditorStyles.miniLabel)
        {
            fontStyle=FontStyle.BoldAndItalic
        };


        GUILayout.BeginVertical();
        if (_tile != null)
        {
        
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);


            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Choose an Object", txt);
            _tile.upgrade = EditorGUILayout.ObjectField(_tile.upgrade, typeof(GameObject), false, GUILayout.Width(180));
            GUILayout.BeginVertical(GUI.skin.box);
            if (_tile.upgrade != null)
                GUILayout.Label(AssetPreview.GetAssetPreview(_tile.upgrade), GUILayout.Width(128), GUILayout.Height(128));
            else
                GUILayout.Label(AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)), GUILayout.Width(128), GUILayout.Height(128));
            GUILayout.EndVertical();

            if (_tile.upgrade != null)
                GUILayout.Label("Name: " + _tile.upgrade.name, txt);
            else
                GUILayout.Label("Name: ", txt);


            GUILayout.EndVertical();


            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Choose an Object", txt);
            _tile.tile = EditorGUILayout.ObjectField(_tile.tile, typeof(GameObject), false, GUILayout.Width(180));
            GUILayout.BeginVertical(GUI.skin.box);
            if (_tile.tile != null)
                GUILayout.Label(AssetPreview.GetAssetPreview(_tile.tile), GUILayout.Width(128), GUILayout.Height(128));
            else
                GUILayout.Label(AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)), GUILayout.Width(128), GUILayout.Height(128));
            GUILayout.EndVertical();

            if (_tile.tile != null)
                GUILayout.Label("Name: " + _tile.tile.name, txt);
            else
                GUILayout.Label("Name: ", txt);


            GUILayout.EndVertical();



            for (int i1 = 0; i1 < _tile.layers.Length; i1++)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Choose an Object", txt);
                _tile.layers[i1] = EditorGUILayout.ObjectField(_tile.layers[i1], typeof(GameObject), false, GUILayout.Width(180));
                GUILayout.BeginVertical(GUI.skin.box);
                if (_tile.layers[i1] != null)
                    GUILayout.Label(AssetPreview.GetAssetPreview(_tile.layers[i1]),GUILayout.Width(128),GUILayout.Height(128));
                else
                    GUILayout.Label(AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)), GUILayout.Width(128), GUILayout.Height(128));
                GUILayout.EndVertical();

               
                if (_tile.layers[i1] != null)
                    GUILayout.Label("Name: "+ _tile.layers[i1].name,txt);
                else
                    GUILayout.Label("Name: ",txt);
                GUILayout.EndVertical();

            }

            GUILayout.EndHorizontal();


        }
        GUILayout.EndVertical();
        

    }

    private void ShowHeaders()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Tile", GUILayout.Width(180));
        GUILayout.Space(16);
        GUILayout.Label("Tile Upgrade", GUILayout.Width(180));
        GUILayout.Space(16);
        GUILayout.Label("Layer 1", GUILayout.Width(180));
        GUILayout.Space(16);
        GUILayout.Label("Layer 2", GUILayout.Width(180));
        GUILayout.Space(16);
        GUILayout.Label("Layer 3", GUILayout.Width(180));
        GUILayout.EndHorizontal();
    }

    private void ShowPreview()
    {
        if (gameObject != null)
        {
            if (gameObjectEditor == null)
                gameObjectEditor = Editor.CreateEditor(gameObject);
            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(400, 290), EditorStyles.helpBox);
        }
    }

   
}

public class PlaceHolderTile
{
    public Object upgrade;
    public Object tile;
    //pass road/river for now
    public Object[] layers;

    public PlaceHolderTile()
    {
        layers = new Object[3];
    }
}


