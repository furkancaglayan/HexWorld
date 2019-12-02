using UnityEngine;
#pragma warning disable 0219
[System.Serializable]
public class HexWorldStaticData : ScriptableObject
{
    [SerializeField, HideInInspector] private HexWorldMap data;
    [SerializeField, HideInInspector] private long size;
    [SerializeField, TextArea] private string Description = "You can write here to remember what is it.";

    public void LoadData(HexWorldMap data)
    {
        this.data = data;
    }

    public void SetSize(long size)
    {
        this.size = size;
    }
    public long GetSize()
    {
        return size;
    }
    public HexWorldMap GetMapData()
    {
        return data;
    }

    public string getDescription()
    {
        return Description;
    }


}