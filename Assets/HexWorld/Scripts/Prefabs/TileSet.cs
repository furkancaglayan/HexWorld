using System;
using UnityEngine;

namespace HexWorld
{
    [Serializable]
    public abstract class TileSet : ScriptableObject
    {
        public new string name;
        public string ecosystem;
        public abstract int GetPropCount();
        public abstract GUIContent[] GetFolderContents();
        public abstract GUIContent[] GetFileContentsInFolder(int folderIndex);

    }

}
