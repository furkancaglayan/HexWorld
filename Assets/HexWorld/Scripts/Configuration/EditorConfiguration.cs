using UnityEngine;  

public class EditorConfiguration:Configuration
{
    public Texture2D hexWorldLogo;
    public Texture2D birchGamesLogo;
    public Texture2D defaultAssetPreview;
    public Texture2D postProcessLogo;
    [Space(10)]
    public Texture placementToolTexture;
    public Texture deleteToolTexture;
    public Texture selectToolTexture;
    public Texture rotateLeftToolTexture;
    public Texture rotateRightToolTexture;
    [Space(10)]
    public string prefabsDirectory = "Assets/HexWorld/Prefabs/Tiles";
    public string saveDirectory = "Assets/HexWorld/SavedMaps";

}
