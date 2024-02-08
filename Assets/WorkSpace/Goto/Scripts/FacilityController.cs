using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 施設の種類と数を変更します
/// </summary>
public class FacilityController : MonoBehaviour
{
    /// <summary>
    /// 施設の数を増やせます
    /// </summary>
    /// <param name="data">施設のデータ</param>
    public void OnFacilityButtonClick(FacilityData data)
    {
        if (!FacilityManager.Instance.FacilityCountDictionary.ContainsKey(data.FacilityType))
        {
            FacilityManager.Instance.AddFacilityCountDictionary(data.FacilityType, 1);
            
#if UNITY_EDITOR
            Debug.Log($"施設の種類 : {data.FacilityType}\n" +
                      $"施設の数 : {FacilityManager.Instance.FacilityCountDictionary[data.FacilityType]}");
#endif
        }
        else
        {
            int count = FacilityManager.Instance.FacilityCountDictionary[data.FacilityType];
            count += 1;
            FacilityManager.Instance.SetFacilityCountDictionary(data.FacilityType, count);
            
#if UNITY_EDITOR
            Debug.Log($"施設の種類 : {data.FacilityType}\n" +
                      $"施設の数 : {FacilityManager.Instance.FacilityCountDictionary[data.FacilityType]}");
#endif
        }
    }
}
