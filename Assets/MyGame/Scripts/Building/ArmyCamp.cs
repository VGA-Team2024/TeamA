using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ArmyCamp : BuildingBase
{
    [SerializeField,Header("兵士プレハブ")] private GameObject _soldier;
    [SerializeField, Header("現在の収容状況")]　private TMP_Text _unitText;
    [SerializeField , Header("1つ当たりの最大保持ユニット数")] private int _maxUnit;
    private readonly Stack<GameObject> _unitList = new Stack<GameObject>(50);

    private int _currentMaxUnit = 0;
    /// <summary>
    /// Unitの最大保持数
    /// </summary>
    public int MaxUnit => _currentMaxUnit;
    /// <summary>
    /// Unitの数
    /// </summary>
    public int CurrentUnit => _unitList.Count;

    public bool IsUnitCreatable() => _unitList.Count < _maxUnit;
    public bool IsUnitRemovable() => _unitList.Count > 0;
    
    public void CreateUnit()
    {
        ResourceManager.Instance.AddUnits(1);
        var soldier = Instantiate(_soldier , transform.position - transform.forward , _soldier.transform.rotation);
        _unitList.Push(soldier);
        _unitText.text = $"{_unitList.Count} / {_currentMaxUnit}";
        //navMeshの目的地用
        //TODOここでsoldierをSetDestinationしてもいい
    }

    public void RemoveUnit()
    {
        var soldier = _unitList.Pop();
        _unitText.text = $"{_unitList.Count} / {_currentMaxUnit}";
        Destroy(soldier);
    }

    public override void OnBuildFinish()
    {
        _currentMaxUnit = _maxUnit;
        _unitText.gameObject.SetActive(true);
        _unitText.text = $"{_unitList.Count} / {_currentMaxUnit}";
    }
    protected override void OnStart()
    {
        
    }

    protected override void OnFixedUpdate()
    {

    }

    public override BuildingSaveData MakeSaveData()
    {
        return new ArmyCampSaveData(base.MakeSaveData() , _maxUnit ,_unitList.Count);
    }

    public override void LoadSaveData(BuildingSaveData saveData)
    {
        base.LoadSaveData(saveData);
        if (saveData is ArmyCampSaveData armyCampSaveData)
        {
            _maxUnit = armyCampSaveData.MaxUnit;
            for (int i = 0; i < armyCampSaveData.CurrentUnit; i++)
            {
                CreateUnit();
            }
        }
        else
        {
            Debug.LogError("Cast失敗");
        }
    }
}
[Serializable]
public class ArmyCampSaveData : BuildingSaveData
{
    [SerializeField] private int _maxUnit;
    [SerializeField] private int _currentUnit;
    public ArmyCampSaveData(int buildingID, BuildingType buildingType, Vector3 position, BuildingCondition currentCondition, int maxUnit, int currentUnit) 
        : base(buildingID, buildingType, position, currentCondition)
    {
        _maxUnit = maxUnit;
        _currentUnit = currentUnit;
    }

    public ArmyCampSaveData(BuildingSaveData makeSaveData,  int maxUnit, int currentUnit) : base(makeSaveData)
    {
        _maxUnit = maxUnit;
        _currentUnit = currentUnit;
    }

    public int CurrentUnit => _currentUnit;

    public int MaxUnit => _maxUnit;
}
