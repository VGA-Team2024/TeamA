using System.Collections.Generic;

/// <summary>
/// 施設の種類と数を管理します
/// </summary>
public class FacilityManager : SingletonMonoBehavior<FacilityManager>
{
    /// <summary>
    /// FacilityData.Facility : 施設の種類
    /// int : 施設の数
    /// </summary>
    private Dictionary<FacilityData.Facility, int> _facilityCountDictionary = new Dictionary<FacilityData.Facility, int>();
    
    /// <summary>
    /// FacilityData.Facility : 施設の種類
    /// int : 施設の数
    /// </summary>
    public Dictionary<FacilityData.Facility, int> FacilityCountDictionary => _facilityCountDictionary;
    
    /// <summary>
    /// 施設の種類と現在の数を変更できます
    /// </summary>
    /// <param name="type">施設の種類</param>
    /// <param name="count">施設の現在の数</param>
    public void SetFacilityCountDictionary(FacilityData.Facility type, int count)
        => _facilityCountDictionary[type] = count;
    
    /// <summary>
    /// 新しく施設の種類と数を追加できます
    /// </summary>
    /// <param name="type">施設の種類</param>
    /// <param name="count">施設の現在の数</param>
    public void AddFacilityCountDictionary(FacilityData.Facility type, int count)
        => _facilityCountDictionary.Add(type, count);
}
