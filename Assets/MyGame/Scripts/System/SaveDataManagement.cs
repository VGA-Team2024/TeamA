using System;
using System.IO;
using UnityEngine;

public static class SaveDataManagement
{
    private static readonly string DataPath = Application.dataPath + "/StreamingAssets/";

    public static bool LoadJson<T>(out T data) where T : ISaveData, new()
    {
        data = new T();
        if (File.Exists(DataPath + data.FileName))
            using (var reader = new StreamReader(DataPath + data.FileName))
            {
                var jsonData = reader.ReadToEnd();
                data = JsonUtility.FromJson<T>(jsonData);
                return true;
            }
        Debug.LogError("ロード可能なデータが存在しません。");
        return false;
    }

    
    public static void SaveJson<T>(T data) where T : ISaveData
    {
        var jsonData = JsonUtility.ToJson(data , true);
        File.WriteAllText(DataPath + data.FileName, jsonData);
    }
}

public interface ISaveData
{
    string FileName { get; }
}
[Serializable]
public abstract class SaveData : ISaveData
{
    private static string _fileName;
    public string FileName => _fileName ??= GetType().FullName + ".json";
}

[Serializable]
public class BuildingSaveData : SaveData 
{

    //publicかSerializeField属性を持つもの出ないと保存できない
    public BuildingType Type;
    public BuildingCondition CurrentCondition;
    public Vector3 Position;
    
    public BuildingSaveData( BuildingType buildingType , Vector3 position, BuildingCondition currentCondition)
    {
        Position = position;
        Type = buildingType;
        CurrentCondition = currentCondition;
    }


}

