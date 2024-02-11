using System.Collections.Generic;
using UniRx;

// 作成 : 五島
/// <summary>
/// 施設の種類と数、生産量を管理する機能を提供します
/// アタッチする必要はありません
/// </summary>
public class FacilityManager : IFacilityPowerProvider
{
    /// <summary>FacilityManagerのインスタンス</summary>
    public static readonly FacilityManager Instance = new();

    /// <summary>
    /// FacilityData.Facility : 施設の種類
    /// FacilityCurrentData : 現在の施設の数と生産量
    /// </summary>
    private Dictionary<int, FacilityCurrentData> _facilityDictionary = new();

    /// <summary>現在の施設の合計生産量</summary>
    private float _currentTotalProducePower = 0;

    /// <summary>現在のリソースの秒間生産力を提供してほしい</summary>
    private ReactiveProperty<float> _currentFacilityPower = new();

    /// <summary>
    /// FacilityData.Facility : 施設の種類
    /// FacilityCurrentData : 現在の施設の数と生産量
    /// </summary>
    public Dictionary<int, FacilityCurrentData> FacilityDictionary => _facilityDictionary;

    /// <summary>現在の施設の合計生産量</summary>
    public float CurrentTotalProducePower => _currentTotalProducePower;

    /// <summary>現在のリソースの秒間生産力を提供してほしい</summary>
    public ReactiveProperty<float> CurrentFacilityPower
    {
        get
        {
            _currentFacilityPower.Value = _currentTotalProducePower;
            return _currentFacilityPower;
        }
    }

    /// <summary>新しく施設の種類と数を追加できます</summary>
    /// <param name="data">追加する施設のデータ</param>
    /// <param name="count">追加する施設の現在の数</param>
    public void AddFacilityDictionary(FacilityData data, int count)
    {
        var facilityStruct = new FacilityCurrentData();
        facilityStruct.count = count;
        facilityStruct.producePower = data.BaseProducePower * count;
        FacilityDictionary.Add((int)data.FacilityType, facilityStruct);
    }

    /// <summary>引数の施設の種類の現在の数を変更できます</summary>
    /// <param name="type">施設の種類</param>
    /// <param name="count">施設の現在の数</param>
    public void SetFacilityDictionaryCount(FacilityData.Facility type, int count)
    {
        if (!_facilityDictionary.ContainsKey((int)type))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"キーに{type}は存在しません。");
#endif
            return;
        }

        var facilityCurrentData = _facilityDictionary[(int)type];
        facilityCurrentData.count = count;
    }

    /// <summary>引数の施設の種類の現在の生産量を変更できます</summary>
    /// <param name="type">施設の種類</param>
    /// <param name="producePower">施設の現在の生産量</param>
    public void SetFacilityDictionaryProducePower(FacilityData.Facility type, float producePower)
    {
        if (!_facilityDictionary.ContainsKey((int)type))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"キーに{type}は存在しません。");
#endif
            return;
        }

        var facilityCurrentData = _facilityDictionary[(int)type];
        facilityCurrentData.producePower = producePower;
    }

    /// <summary>引数の施設の現在の生産量を算出します</summary>
    /// <param name="data">施設のデータ</param>
    /// <returns>現在の施設の生産量</returns>
    public float MulProducePower(FacilityData data)
    {
        var mul = data.BaseProducePower;
        
        if (_facilityDictionary.ContainsKey((int)data.FacilityType))
        {
            mul *= _facilityDictionary[(int)data.FacilityType].count;
        }

        if (UpGradeManager.Instance.UpGradeDictionary.ContainsKey((int)data.FacilityType))
        {
            mul *= UpGradeManager.Instance.MulUpGradeTypeMagnificationRate((int)data.FacilityType);
        }

        // 32 : All
        if (UpGradeManager.Instance.UpGradeDictionary.TryGetValue(32, out var value))
        {
            mul *= value.magnificationRate;
        }

        return mul;
    }

    /// <summary>現在の施設の合計生産量を算出します</summary>
    /// <returns>現在の施設の合計生産量</returns>
    public float SumTotalProducePower()
    {
        _currentTotalProducePower = 0;

        foreach (var value in _facilityDictionary.Values)
        {
            _currentTotalProducePower += value.producePower;
        }

        return _currentTotalProducePower;
    }
}

/// <summary>現在の施設のデータを記録します</summary>
[System.Serializable]
public class FacilityCurrentData
{
    /// <summary>施設の現在の数</summary>
    public int count;

    /// <summary>施設の現在の生産量</summary>
    public float producePower;
}
