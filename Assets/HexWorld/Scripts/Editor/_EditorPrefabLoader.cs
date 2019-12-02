using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexWorldPrefabLoader))]
public class _EditorPrefabLoader : Editor
{

 

    HexWorldPrefabLoader _instance;



    private float labelWidth = 120;
    private bool defaultInspector;


    private int selectedPrefabFolder = 0;
    private GUIContent[][] prefabContents;
    private GUIContent[] folderContents;
    private bool showPrefabs = false;

    public void OnEnable()
    {
        _instance = (HexWorldPrefabLoader)target;
    }

    public override void OnInspectorGUI()
    {

        GUIStyle labels = new GUIStyle(EditorStyles.miniLabel)
        {
            fontSize = 12,
            richText = true,
            margin=new RectOffset(0,0,-2,0)
        };
        GUI.color = Color.white;
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Root path:", labels,GUILayout.Width(labelWidth));
        _instance.path = GUILayout.TextField(_instance.path,120);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Ecosystem:", labels, GUILayout.Width(labelWidth));
        _instance.ecosystemName = GUILayout.TextField(_instance.ecosystemName, 120);
        GUILayout.EndHorizontal();

   

    

      


        GUI.color = Color.green;
        GUILayout.Space(15);
        if (GUILayout.Button("Create",EditorStyles.toolbarButton))
            LoadData();
        GUI.color = Color.white;


        if (_instance.hexWorldPrefabSet != null)
        {
            prefabContents = _instance.hexWorldPrefabSet.GetPrefabContents();
            folderContents = _instance.hexWorldPrefabSet.GetFolderContents();
        }

        showPrefabs = GUILayout.Toggle(showPrefabs, "Show Prefabs");
        if(showPrefabs)
            ShowGrids();
       







        GUILayout.Space(20);
        GUIStyle foldout = new GUIStyle(EditorStyles.foldout)
        {
            font=EditorStyles.boldFont,
        };
        defaultInspector = EditorGUILayout.Foldout(defaultInspector, "Debug", false, foldout);
       
        if (defaultInspector)
            DrawDefaultInspector();
        GUILayout.EndVertical();


       

        EditorUtility.SetDirty(_instance);
    }

    private void ShowGrids()
    {
        GUIStyle prefabStyle = new GUIStyle(GUI.skin.window)
        {
            imagePosition = ImagePosition.ImageAbove,
            padding = new RectOffset(0, 0, 40, 0),
            font = EditorStyles.miniBoldFont,
            fontStyle = FontStyle.Italic,
            fontSize = 12
        };
        GUILayout.BeginVertical(GUI.skin.box);
        selectedPrefabFolder =GUILayout.Toolbar(selectedPrefabFolder, folderContents, EditorStyles.toolbarButton);
        GUILayout.Space(3);
        int rows = 4;
        if (prefabContents != null)
        {
            if (prefabContents.Length < selectedPrefabFolder - 1)
                selectedPrefabFolder = 0;
            if (prefabContents[selectedPrefabFolder].Length != 0)
            {
                /*selectedPrefab = GUILayout.SelectionGrid(selectedPrefab, prefabContents[selectedPrefabFolder],
                    3, prefabStyle,GUILayout.Height(400));*/

                GUILayout.BeginVertical();
                for (int i = 0; i < prefabContents[selectedPrefabFolder].Length; i++)
                {
                    if (i % rows == 0)
                    {
                        if(i!=0)
                            GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                    }
                   

                    GUIContent content = prefabContents[selectedPrefabFolder][i];
                    GUILayout.BeginVertical(GUI.skin.box,GUILayout.Width(75));
                    GUILayout.Label(content.image,EditorStyles.helpBox);
                    GUILayout.Label(content.text,EditorStyles.boldLabel);

                    EditorGUILayout.Separator();
                    GUILayout.Label("Appearance Rate %99");
                   
                    Rect rect = GUILayoutUtility.GetRect(140, 20);
                    rect.width = 140;
                    rect.height = 20;
                    EditorGUI.ProgressBar(rect, .3f, "Rate");

                    if (GUILayout.Button("Delete",EditorStyles.toolbarButton,GUILayout.Width(rect.width)))
                        _instance.hexWorldPrefabSet.Get(selectedPrefabFolder).DeletePrefab(i);

                    GUILayout.Space(3);
                    GUILayout.EndVertical();

                    if (i == prefabContents[selectedPrefabFolder].Length - 1)
                        GUILayout.EndHorizontal();

                }

                  
                GUILayout.EndVertical();
            }

            else
            {
                GUILayout.BeginVertical();
                GUILayout.Label("No prefabs are present at : " + folderContents[selectedPrefabFolder].tooltip);
                GUILayout.Label(
                    "Enter a valid 'Prefabs Directory' and click on 'Load Prefabs'.\nThis will load the assets that are in the path.");
                GUILayout.Space(20);
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();
 
    }
    private void LoadData()
    {
        _instance.hexWorldPrefabSet = Factory.create_dataset(_instance.path);
        _instance.hexWorldPrefabSet.Create();
        prefabContents = _instance.hexWorldPrefabSet.GetPrefabContents();
        folderContents = _instance.hexWorldPrefabSet.GetFolderContents();
        showPrefabs = true;
        selectedPrefabFolder = 0;
    }
}