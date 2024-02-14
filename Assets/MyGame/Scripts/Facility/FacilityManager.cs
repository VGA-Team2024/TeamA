using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 作成 : 五島
/// <summary>
/// 施設の種類と数、生産量を管理する機能を提供します
/// アタッチする必要はありません
/// </summary>
public class FacilityManager : PowerProviderBase
{
    [SerializeField] private UpGradeManager _upGradeManager;

    /// <summary>FacilityManagerのインスタンス</summary>
    //public static readonly FacilityManager Instance = new();

    /// <summary>
    /// FacilityData.Facility : 施設の種類
    /// FacilityCurrentData : 現在の施設の数と生産量
    /// </summary>
    private Dictionary<int, FacilityCurrentData> _facilityDictionary = new();

    /// <summary>現在の施設の合計生産量</summary>
    private ReactiveProperty<decimal> _currentTotalProducePower = new();

    /// <summary>現在のリソースの秒間生産力を提供してほしい</summary>
    //private ReactiveProperty<float> _currentFacilityPower = new();

    /// <summary>
    /// FacilityData.Facility : 施設の種類
    /// FacilityCurrentData : 現在の施設の数と生産量
    /// </summary>
    public Dictionary<int, FacilityCurrentData> FacilityDictionary => _facilityDictionary;

    /// <summary>現在の施設の合計生産量</summary>
    //public decimal CurrentTotalProducePower => _currentTotalProducePower;

    /// <summary>現在のリソースの秒間生産力を提供してほしい</summary>
    public override ReactiveProperty<decimal> CurrentFacilityPower => _currentTotalProducePower;

    public override ReactiveProperty<decimal> CurrentClickPower { get; }

    /// <summary>新しく施設の種類と数を追加できます</summary>
    /// <param name="data">追加する施設のデータ</param>
    /// <param name="count">追加する施設の現在の数</param>
    public void AddFacilityDictionary(FacilityData data, int count)
    {
        var facilityCurrentData = new FacilityCurrentData();
        facilityCurrentData.baseData = data;
        facilityCurrentData.count = count;
        facilityCurrentData.producePower = (decimal)data.BaseProducePower * count;
        FacilityDictionary.Add((int)data.FacilityType, facilityCurrentData);
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
    public void SetFacilityDictionaryProducePower(FacilityData.Facility type, decimal producePower)
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

    public void SetFacilityDictionaryProducePower(int type, decimal producePower)
    {
        if (!_facilityDictionary.ContainsKey(type))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"キーに{type}は存在しません。");
#endif
            return;
        }

        var facilityCurrentData = _facilityDictionary[type];
        facilityCurrentData.producePower = producePower;
    }

    /// <summary>引数の施設の現在の生産量を算出します</summary>
    /// <param name="data">施設のデータ</param>
    /// <returns>現在の施設の生産量</returns>
    public decimal CalculateProducePower(FacilityData data)
    {
        var mul = (decimal)data.BaseProducePower;

        if (_facilityDictionary.ContainsKey((int)data.FacilityType))
        {
            mul *= _facilityDictionary[(int)data.FacilityType].count;
        }

        if (_upGradeManager.UpGradeDictionary.ContainsKey((int)data.FacilityType))
        {
            mul *= (decimal)_upGradeManager.MulUpGradeTypeMagnificationRate((int)data.FacilityType);
        }

        // 32 : All
        if (_upGradeManager.UpGradeDictionary.TryGetValue(32, out var value))
        {
            mul *= (decimal)value.magnificationRate;
        }

        return mul;
    }

    public decimal CalculateProducePower(int type)
    {
        var mul = (decimal)_facilityDictionary[type].baseData.BaseProducePower;

        if (_facilityDictionary.ContainsKey(type))
        {
            mul *= _facilityDictionary[type].count;
        }

        if (_upGradeManager.UpGradeDictionary.ContainsKey(type))
        {
            mul *= (decimal)_upGradeManager.MulUpGradeTypeMagnificationRate(type);
        }

        // 32 : All
        if (_upGradeManager.UpGradeDictionary.TryGetValue(32, out var value))
        {
            mul *= (decimal)value.magnificationRate;
        }

        return mul;
    }

    /// <summary>現在の施設の合計生産量を算出します</summary>
    /// <returns>現在の施設の合計生産量</returns>
    public decimal SumTotalProducePower()
    {
        _currentTotalProducePower.Value = 0;

        foreach (var value in _facilityDictionary.Values)
        {
            _currentTotalProducePower.Value += value.producePower;
        }

        return _currentTotalProducePower.Value;
    }
}

/// <summary>現在の施設のデータを記録します</summary>
[System.Serializable]
public class FacilityCurrentData
{
    /// <summary>施設の基本データ</summary>
    public FacilityData baseData;

    /// <summary>施設の現在の数</summary>
    public int count;

    /// <summary>施設の現在の生産量</summary>
    public decimal producePower;
}
