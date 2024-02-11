using UnityEngine;

// 作成 : 五島
/// <summary>施設の基本データの設定をします</summary>
[System.Serializable]
[CreateAssetMenu(fileName = "FacilityData", menuName = "CreateFacilityData")]
public class FacilityData : ScriptableObject
{
    [SerializeField, Header("施設の種類")]　private Facility upGradeType = 0;
    [SerializeField, Header("施設の名前")] private string _name = "";
    [SerializeField, Header("施設の値段")] private long _basePrice = 10;  // コンパイル時に毎回リセットされる？
    [SerializeField, Header("施設の生産量")] private float _baseProduceVolume = 1f;

    /// <summary>施設の種類</summary>
    public Facility FacilityType => upGradeType;

    /// <summary>施設の名前</summary>
    public string Name => _name;

    /// <summary>施設の値段</summary>
    public decimal BasePrice => _basePrice;

    /// <summary>施設の生産量</summary>
    public float BaseProduceVolume => _baseProduceVolume;

    /// <summary>施設の種類</summary>
    public enum Facility
    {
        Cursor = 0,
        GrandMother = 1,
    }
}
