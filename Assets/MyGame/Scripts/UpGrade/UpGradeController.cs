using UnityEngine;

// 作成 : 五島
/// <summary>アップグレードの種類とフラグを変更する機能を提供します</summary>
public class UpGradeController : MonoBehaviour
{
    [SerializeField] private FacilityManager _facilityManager;
    [SerializeField] private UpGradeManager _upGradeManager;

    /// <summary>引数のアップグレードのフラグをtrueにします</summary>
    /// <param name="data">アップグレードのデータ</param>
    public void OnUpGradeButtonClick(UpGradeData data)
    {
        // 施設の有無を判定
        if (!_upGradeManager.UpGradeDictionary.ContainsKey((int)data.TargetFacilityType))
        {
            // 無ければ
            _upGradeManager.AddUpGradeDictionary(data, true);
            _upGradeManager.MulUpGradeTypeMagnificationRate((int)data.TargetFacilityType);
            _facilityManager.SetFacilityDictionaryProducePower((int)data.TargetFacilityType,
                _facilityManager.CalculateProducePower((int)data.TargetFacilityType));
            _facilityManager.SumTotalProducePower();

#if UNITY_EDITOR
            // なんか計算順序が違う？
            Debug.Log($"アップグレードの種類 : {data.UpGradeType}\n" +
                      $"アップグレードの数 : {_upGradeManager.UpGradeDictionary[(int)data.TargetFacilityType].IsUsed}\n" +
                      $"アップグレードの生産量 : {_upGradeManager.UpGradeDictionary[(int)data.TargetFacilityType].MagnificationRate}\n" +
                      $"施設の合計生産量 : {_facilityManager.SumTotalProducePower()}");
#endif
        }
        else
        {
            // 有れば
            _upGradeManager.SetUpGradeDictionaryUsedFlag(data.TargetFacilityType, true);
            _upGradeManager.MulUpGradeTypeMagnificationRate((int)data.TargetFacilityType);
            _facilityManager.SetFacilityDictionaryProducePower((int)data.TargetFacilityType,
                _facilityManager.CalculateProducePower((int)data.TargetFacilityType));
            _facilityManager.SumTotalProducePower();

#if UNITY_EDITOR
            Debug.Log($"アップグレードの種類 : {data.UpGradeType}\n" +
                      $"アップグレードのフラグ : {_upGradeManager.UpGradeDictionary[(int)data.TargetFacilityType].IsUsed}\n" +
                      $"アップグレードの倍率 : {_upGradeManager.UpGradeDictionary[(int)data.TargetFacilityType].MagnificationRate}\n" +
                      $"施設の合計生産量 : {_facilityManager.SumTotalProducePower()}");
#endif
        }
    }
}
