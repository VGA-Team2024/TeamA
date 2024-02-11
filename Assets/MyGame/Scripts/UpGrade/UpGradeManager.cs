using System.Collections.Generic;

// 作成 : 五島
/// <summary>
/// アップグレードの種類とフラグ、倍率を管理する機能を提供します
/// アタッチする必要はありません
/// </summary>
public class UpGradeManager
{
    /// <summary>FacilityManagerのインスタンス</summary>
    public static readonly UpGradeManager Instance = new();

    /// <summary>
    /// int : アップグレードの効果対象の施設
    /// FacilityStruct : 現在のアップグレードの種類とフラグ、倍率
    /// </summary>
    private Dictionary<int, UpGradeCurrentData> _upGradeDictionary = new();

    /// <summary>
    /// int : アップグレードの効果対象の施設
    /// FacilityStruct : 現在のアップグレードの種類とフラグ、倍率
    /// </summary>
    public Dictionary<int, UpGradeCurrentData> UpGradeDictionary => _upGradeDictionary;

    /// <summary>新しくアップグレードの種類とフラグを追加できます</summary>
    /// <param name="data">追加するアップグレードのデータ</param>
    /// <param name="flag">追加するアップグレードの現在のフラグ</param>
    public void AddUpGradeDictionary(UpGradeData data, bool flag)
    {
        var upGradeCurrentData = new UpGradeCurrentData();
        upGradeCurrentData.targetFacilityType = data.TargetFacilityType;
        upGradeCurrentData.isUsed = flag;
        upGradeCurrentData.magnificationRate = data.MagnificationRate;
        _upGradeDictionary.Add((int)data.TargetFacilityType, upGradeCurrentData);
    }

    /// <summary>引数のアップグレードの種類の現在のフラグを変更できます</summary>
    /// <param name="type">アップグレードのデータ</param>
    /// <param name="flag">アップグレードの現在のフラグ</param>
    public void SetUpGradeDictionaryUsedFlag(UpGradeData.TargetFacility type, bool flag)
    {
        if (!_upGradeDictionary.ContainsKey((int)type))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"キーに{type}は存在しません。");
#endif
            return;
        }

        var upGradeCurrentData = _upGradeDictionary[(int)type];
        upGradeCurrentData.isUsed = flag;
    }

    /// <summary>現在のアップグレードの合計倍率を算出します</summary>
    /// <returns>現在のアップグレードの合計倍率</returns>
    public float MulUpGradeTypeMagnificationRate(int type)
    {
        var mul = 1f;

        foreach (var value in _upGradeDictionary.Values)
        {
            if (value.isUsed)
            {
                mul *= value.magnificationRate;
            }
        }

        return mul;
    }
}

/// <summary>現在のアップグレードのデータを記録します</summary>
[System.Serializable]
public class UpGradeCurrentData
{
    /// <summary>アップグレードの効果対象の施設</summary>
    public UpGradeData.TargetFacility targetFacilityType;

    /// <summary>アップグレードの現在のフラグ</summary>
    public bool isUsed;

    /// <summary>アップグレードの現在の倍率</summary>
    public float magnificationRate;
}
