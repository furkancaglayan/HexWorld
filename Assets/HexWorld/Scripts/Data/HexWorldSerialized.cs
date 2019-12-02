using UnityEngine;
#pragma warning disable 0219
[System.Serializable]
public class HexWorldSerialized : ScriptableObject
{
    [SerializeField] private HexWorldMap _data;
    [SerializeField, HideInInspector] private long _size;
    [SerializeField, TextArea] private string Description = "You can write here to remember what is it.";

    public void LoadData(HexWorldMap data) => _data = data;
    public void SetSize(long size) => _size = size;
    public long GetSize() => _size;
    public HexWorldMap GetMapData() => _data;
    public string GetDescription() => Description;


}