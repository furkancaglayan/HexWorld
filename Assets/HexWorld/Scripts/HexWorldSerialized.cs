using UnityEngine;
#pragma warning disable 0219
[System.Serializable]
public class HexWorldSerialized : ScriptableObject
{
    [SerializeField] private Map _data;
    [SerializeField, HideInInspector] private long _size;
    [SerializeField, TextArea] private string Description = "You can write here to remember what is it.";

    public void LoadData(Map data) => _data = data;
    public void SetSize(long size) => _size = size;
    public long GetSize() => _size;
    public Map GetMapData() => _data;
    public string GetDescription() => Description;


}