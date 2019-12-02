using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using DataType=Enums.DataType;
using g = UnityEngine.GUILayout;
using ge = UnityEditor.EditorGUILayout;

public class _EditorDatasetGenerator : EditorWindow
{
    #region Init
    private static EditorConfiguration _configuration;
    [MenuItem("HexWorld/Dataset Generator", priority = 2)]
    static void Init()
    {
        _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));
        _EditorDatasetGenerator window = (_EditorDatasetGenerator)GetWindow(typeof(_EditorDatasetGenerator));
        window.autoRepaintOnSceneChange = true;
        window.titleContent = new GUIContent("Dataset Generator", _configuration.birchGamesLogo);
        window.Show(false);

    }

    private void OnEnable()
    {
        if(_configuration==null)
            _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));
    }
    #endregion
    #region Fields
    private string _datasetName = "Dataset1";
    private string _datasetEcosystem = "Awesome Ecosystem";
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
    private bool _loaded = false;
    private bool _singleFolderSet = false;
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
        g.Label("HexWorld Dataset Generator", labelstyle);
        g.BeginVertical();

        g.Space(10);

        g.BeginHorizontal();
        g.Space(10);
        g.Label("Dataset Name:", textstyle, g.Width(labelWidth));
        _datasetName = g.TextField(_datasetName, textfield, g.Width(FieldWidth));
        g.EndHorizontal();


        g.BeginHorizontal();
        g.Space(10);
        g.Label("Save Directory:", textstyle, g.Width(labelWidth));
        _savePath = g.TextField(_savePath, textfield, g.Width(FieldWidth));
        g.EndHorizontal();


        g.BeginHorizontal();
        g.Space(10);
        g.Label("Ecosystem:", textstyle, g.Width(labelWidth));
        _datasetEcosystem = g.TextField(_datasetEcosystem, textfield, g.Width(FieldWidth));
        g.EndHorizontal();

        g.BeginHorizontal();
        g.Space(10);
        g.Label("Dataset Type:", textstyle, g.Width(labelWidth));
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
            g.Label("Combined Dataset", labelstyle);


            g.BeginHorizontal(EditorStyles.helpBox, g.Width(position.width-30));
            g.Space(10);
            g.Label("Combined Dataset allows you to buraya bilgi gelecek ehehehhehe", textstyle);
            g.EndHorizontal();
            g.Space(10);

            g.BeginHorizontal();
            g.Space(10);
            g.Label("Dataset Path:", textstyle, g.Width(labelWidth));
            _path = g.TextField(_path, textfield, g.Width(FieldWidth-50));
            g.Space(10);
            GUI.color = _color2;
            if (g.Button(new GUIContent("F", "Check if directory is valid!"), EditorStyles.toolbarButton, g.Width(40)))
                _EditorUtility.CheckIfDirectoryIsValid(_path,true);
            GUI.color = Color.white;
            g.EndHorizontal();


            g.BeginHorizontal();
            g.Space(10);
            g.Label(new GUIContent("Single Folder:", "Single Folder set uses prefabs only from the root folder. " +
                                                    "If set to false, dataset will contain prefabs from root folder and all of its 'first' subfolders"), textstyle, g.Width(labelWidth));
            _singleFolderSet = g.Toggle(_singleFolderSet,"");
            g.EndHorizontal();



            g.Space(4);
            g.BeginHorizontal();
            g.Space(10);
            GUI.color = _color1;
            if (g.Button("Create Dataset", EditorStyles.toolbarButton, g.Width(SecondFieldWidth)))
                _EditorDatasetUtility.CreateCombinedDataSet(_path,_datasetName,_datasetEcosystem,_savePath,_singleFolderSet
                    
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
            g.Label("Layered Dataset", labelstyle);

            g.BeginHorizontal(EditorStyles.helpBox, g.Width(position.width - 30));
            g.Space(10);
            g.Label("Layered Dataset allows you to buraya bilgi gelecek ehehehhehe", textstyle);
            g.EndHorizontal();
            g.Space(10);


            g.EndVertical();
        }
        #endregion

        g.EndVertical();
        g.EndScrollView();
    }
    #endregion

}
