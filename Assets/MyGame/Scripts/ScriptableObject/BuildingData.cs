using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>施設の基本データの設定をします</summary>
[System.Serializable , CustomData]
[CreateAssetMenu(fileName = "BuildingData", menuName = "CreateBuildingData")]
public class BuildingData : ScriptableObject
{
    [SerializeField, Header("種類")]　private BuildingType _buildingType = 0;
    [SerializeField, Header("設置するプレハブ")] private BuildingBase _building;
    [SerializeField, Header("名前")] private string _name = "";
    [SerializeField, Header("値段")] private int _price = 10;
    [SerializeField, Header("設置上限")] private int _maxAmount;
    [SerializeField, Header("建設時間")] private float _buildTime;
    /// <summary>種類</summary>
    public BuildingType BuildingType => _buildingType;
    /// <summary>設置するプレハブ</summary>
    public BuildingBase Building => _building;
    /// <summary>名前</summary>
    public string Name => _name;

    /// <summary>値段</summary>
    public int Price => _price;

    /// <summary>設置上限</summary>
    public int MaxAmount => _maxAmount;

    public float BuildTime => _buildTime;
}

/// <summary>施設の種類</summary>
[MyEnumCustom]
public enum BuildingType
{
    BaseCamp = 0,
    ArmyCamp = 1,
    Barrack = 2,
    Mine = 3,
    Test = 4,
}

[MyEnumCustom]
public enum TesTes
{
    aaaaa = 0,
}
