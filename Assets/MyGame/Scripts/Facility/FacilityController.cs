using UnityEngine;

// 作成 : 五島
/// <summary>施設の種類と数、生産量を変更する機能を提供します</summary>
public class FacilityController : MonoBehaviour
{
    [SerializeField] private FacilityManager _facilityManager;

    /// <summary>引数の施設の数を増やせます</summary>
    /// <param name="data">施設のデータ</param>
    public void OnFacilityButtonClick(FacilityData data)
    {
        // 施設の有無を判定
        if (!_facilityManager.FacilityDictionary.ContainsKey((int)data.FacilityType))
        {
            // 無ければ
            _facilityManager.AddFacilityDictionary(data, 1);
            _facilityManager.SetFacilityDictionaryProducePower(data.FacilityType,
                _facilityManager.CalculateProducePower(data));
            _facilityManager.SumTotalProducePower();

#if UNITY_EDITOR
            Debug.Log($"施設の種類 : {data.FacilityType}\n" +
                      $"施設の数 : {_facilityManager.FacilityDictionary[(int)data.FacilityType].count}\n" +
                      $"施設の生産量 : {_facilityManager.FacilityDictionary[(int)data.FacilityType].producePower}\n" +
                      $"施設の合計生産量 : {_facilityManager.SumTotalProducePower()}");
#endif
        }
        else
        {
            // 有れば
            int count = _facilityManager.FacilityDictionary[(int)data.FacilityType].count;
            count += 1;
            _facilityManager.SetFacilityDictionaryCount(data.FacilityType, count);
            _facilityManager.SetFacilityDictionaryProducePower(data.FacilityType,
                _facilityManager.CalculateProducePower(data));
            _facilityManager.SumTotalProducePower();

#if UNITY_EDITOR
            Debug.Log($"施設の種類 : {data.FacilityType}\n" +
                      $"施設の数 : {_facilityManager.FacilityDictionary[(int)data.FacilityType].count}\n" +
                      $"施設の生産量 : {_facilityManager.FacilityDictionary[(int)data.FacilityType].producePower}\n" +
                      $"施設の合計生産量 : {_facilityManager.SumTotalProducePower()}");
#endif
        }
    }
}
