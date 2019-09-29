using UnityEditor;
using UnityEngine;
namespace HexWorld
{

    public class _EditorAbout : EditorWindow
    {

        private static EditorConfiguration _configuration;

        [MenuItem("HexWorld/About Us")]
        public static void Init()
        {
            _configuration = (EditorConfiguration)AssetDatabase.LoadAssetAtPath("Assets/HexWorld/Configuration/BaseSettings.asset", typeof(EditorConfiguration));
            _EditorAbout window = (_EditorAbout)GetWindow(typeof(_EditorAbout));
            window.autoRepaintOnSceneChange = true;
            window.titleContent = new GUIContent("_EditorAbout Us", _configuration.birchGamesLogo);
            window.Show(false);

            window.maxSize = new Vector2(450, 600);
            window.minSize = window.maxSize;


        }



        private void OnGUI()
        {
            GUIStyle uIStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                richText = true,
                fontSize = 11
            };
            uIStyle.wordWrap = true;
            GUIStyle urlStyle = new GUIStyle()
            {
                richText = true,
                fontSize = 12

            };
            GUIStyle logoStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(0, 0, 8, 0)
            };
            GUILayout.BeginVertical();

            GUI.color = new Color(255F / 255F, 211F / 255F, 89F / 255F, .8F);
            GUILayout.BeginHorizontal(logoStyle, GUILayout.Width(position.width), GUILayout.Height(128));
            GUILayout.Space(position.width / 2 - 148);
            /*GUILayout.Label(IconPack.GetBirchgamesLogo(), GUILayout.Width(128), GUILayout.Height(128));
            GUILayout.Label(IconPack.GetHexworldLogo(), GUILayout.Width(128), GUILayout.Height(128));*/
            GUILayout.EndHorizontal();


            GUILayout.Label("BIRCHGAMES", EditorStyles.boldLabel);
            GUILayout.Label("We are creating assets and games. Check out at: ", uIStyle);

            Link("www.birchgames.com", "birchgames.com", urlStyle);


            GUILayout.Label("For any problem, advice or feature request drop an email: ", uIStyle);
            Link("mailto:connect@birchgames.com", "connect@birchgames.com", urlStyle);

            GUILayout.Space(10);

            GUILayout.Label("HexWorld v1.2", EditorStyles.boldLabel);
            GUILayout.Label("We added some useful guides to HexWorld/Documentation. Check them out to start with HexWorld.", uIStyle);
            GUILayout.Label("Also, we decided to add users to our private repo to get realtime updates and see the progress." +
                            "If you are interested in this, drop a mail.", uIStyle);
            GUILayout.Label("For more info go to:", uIStyle);


            Link("https://birchgames.com/hexworld/", "birchgames/hexworld", urlStyle);
            GUILayout.Label("You can also watch demos on our youtube channel:", uIStyle);
            Link("https://www.youtube.com/channel/UCpbaG9ieaTReZYD3kaP3U6g", "Our Youtube Channel", urlStyle);

            GUILayout.Space(20);
            GUILayout.Label("Our other social accounts:", uIStyle);
            Link("https://www.patreon.com/birchgames", "Patreon", urlStyle);
            Link("https://twitter.com/GamesBirch", "Twitter", urlStyle);
            GUILayout.EndVertical();

            Repaint();
        }

        private void Link(string url, string name, GUIStyle urlStyle)
        {
            Rect rect3 = EditorGUILayout.GetControlRect();
            if (Event.current.type == EventType.MouseUp && rect3.Contains(Event.current.mousePosition))
                Application.OpenURL(url);
            if (rect3.Contains(Event.current.mousePosition))
                GUI.Label(rect3, "<color=blue>" + name + "</color>", urlStyle);
            else
                GUI.Label(rect3, "<color=green>" + name + "</color>", urlStyle);
        }
    }

}