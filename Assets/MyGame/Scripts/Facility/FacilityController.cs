using UnityEngine;

// 作成 : 五島
/// <summary>施設の種類と数、生産量を変更する機能を提供します</summary>
public class FacilityController : MonoBehaviour
{
    /// <summary>引数の施設の数を増やせます</summary>
    /// <param name="data">施設のデータ</param>
    public void OnFacilityButtonClick(FacilityData data)
    {
        // 施設の有無を判定
        if (!FacilityManager.Instance.FacilityDictionary.ContainsKey((int)data.FacilityType))
        {
            // 無ければ
            FacilityManager.Instance.AddFacilityDictionary(data, 1);
            FacilityManager.Instance.SetFacilityDictionaryProducePower(data.FacilityType, FacilityManager.Instance.MulProducePower(data));
            FacilityManager.Instance.SumTotalProducePower();

#if UNITY_EDITOR
            Debug.Log($"施設の種類 : {data.FacilityType}\n" +
                      $"施設の数 : {FacilityManager.Instance.FacilityDictionary[(int)data.FacilityType].count}\n" +
                      $"施設の生産量 : {FacilityManager.Instance.FacilityDictionary[(int)data.FacilityType].producePower}\n" +
                      $"施設の合計生産量 : {FacilityManager.Instance.SumTotalProducePower()}");
#endif
        }
        else
        {
            // 有れば
            int count = FacilityManager.Instance.FacilityDictionary[(int)data.FacilityType].count;
            count += 1;
            FacilityManager.Instance.SetFacilityDictionaryCount(data.FacilityType, count);
            FacilityManager.Instance.SetFacilityDictionaryProducePower(data.FacilityType, FacilityManager.Instance.MulProducePower(data));
            FacilityManager.Instance.SumTotalProducePower();

#if UNITY_EDITOR
            Debug.Log($"施設の種類 : {data.FacilityType}\n" +
                      $"施設の数 : {FacilityManager.Instance.FacilityDictionary[(int)data.FacilityType].count}\n" +
                      $"施設の生産量 : {FacilityManager.Instance.FacilityDictionary[(int)data.FacilityType].producePower}\n" +
                      $"施設の合計生産量 : {FacilityManager.Instance.SumTotalProducePower()}");
#endif
        }
    }
}
