using UnityEngine;
using UnityEngine.Serialization;

/// <summary>アップグレードの基本データの設定をします</summary>
[System.Serializable]
[CreateAssetMenu(fileName = "UpGradeData", menuName = "CreateUpGradeData")]
public class UpGradeData : ScriptableObject
{
    [SerializeField, Header("アップグレードの種類")]　private UpGrade _upGradeType = 0;
    [SerializeField, Header("アップグレードの名前")] private string _name = "";
    [SerializeField, Header("アップグレードの値段")] private long _basePrice = 10;  // コンパイル時に毎回リセットされる？
    [SerializeField, Header("アップグレードの倍率")] private float _magnificationRate = 1f;
    
    /// <summary>アップグレードの種類</summary>
    public UpGrade UpGradeType => _upGradeType;
    /// <summary>アップグレードの名前</summary>
    public string Name => _name;
    /// <summary>アップグレードの値段</summary>
    public decimal BasePrice => _basePrice;
    /// <summary>アップグレードの倍率</summary>
    public float MagnificationRate => _magnificationRate;
    
    /// <summary>アップグレードの種類</summary>
    public enum UpGrade
    {
        DoubleTheEarnings = 0,
    }
}
