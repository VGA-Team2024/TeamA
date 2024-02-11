using UnityEngine;

// 作成 : 五島
/// <summary>アップグレードの種類とフラグを変更する機能を提供します</summary>
public class UpGradeController : MonoBehaviour
{
    /// <summary>引数のアップグレードのフラグをtrueにします</summary>
    /// <param name="data">アップグレードのデータ</param>
    public void OnUpGradeButtonClick(UpGradeData data)
    {
        // 施設の有無を判定
        if (!UpGradeManager.Instance.UpGradeDictionary.ContainsKey((int)data.TargetFacilityType))
        {
            // 無ければ
            UpGradeManager.Instance.AddUpGradeDictionary(data, true);
            UpGradeManager.Instance.MulUpGradeTypeMagnificationRate((int)data.TargetFacilityType);
            FacilityManager.Instance.SumTotalProducePower();

#if UNITY_EDITOR
            Debug.Log($"アップグレードの種類 : {data.UpGradeType}\n" +
                      $"アップグレードの数 : {UpGradeManager.Instance.UpGradeDictionary[(int)data.TargetFacilityType].isUsed}\n" +
                      $"アップグレードの生産量 : {UpGradeManager.Instance.UpGradeDictionary[(int)data.TargetFacilityType].magnificationRate}\n" +
                      $"施設の合計生産量 : {FacilityManager.Instance.SumTotalProducePower()}");
#endif
        }
        else
        {
            // 有れば
            UpGradeManager.Instance.SetUpGradeDictionaryUsedFlag(data.TargetFacilityType, true);
            UpGradeManager.Instance.MulUpGradeTypeMagnificationRate((int)data.TargetFacilityType);
            FacilityManager.Instance.SumTotalProducePower();

#if UNITY_EDITOR
            // なんか計算順序が違う？
            Debug.Log($"アップグレードの種類 : {data.UpGradeType}\n" +
                      $"アップグレードのフラグ : {UpGradeManager.Instance.UpGradeDictionary[(int)data.TargetFacilityType].isUsed}\n" +
                      $"アップグレードの倍率 : {UpGradeManager.Instance.UpGradeDictionary[(int)data.TargetFacilityType].magnificationRate}\n" +
                      $"施設の合計生産量 : {FacilityManager.Instance.SumTotalProducePower()}");
#endif
        }
    }
}
