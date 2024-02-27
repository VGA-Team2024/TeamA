using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 鉱山
/// </summary>
public class Mine : BuildingBase
{
    [SerializeField, Header("TestUI / Gold表示")] private Text _goldText;
    [SerializeField, Header("gold生産量/毎秒")] private float _generateGold;
    [SerializeField, Header("保持上限")] private float _maxStorage;
    private float _currentGold;
    private ResourceManager _resourceManager;
    protected override void OnStart()
    {
        _resourceManager = ResourceManager.Instance;
        ShowCurrentGold();
    }

    protected override void OnFixedUpdate()
    {
        if (_currentGold < _maxStorage)
        {
            _currentGold += _generateGold * Time.fixedDeltaTime;
            ShowCurrentGold();
        }
    }

    public override void OnClick()
    {
        if (!CurrentCondition.IsActivate) return;
        _resourceManager.AddResources(_currentGold);
        _currentGold = 0;
    }

    public override BuildingSaveData MakeSaveData()
    {
        return new MineSaveData(base.MakeSaveData(), _currentGold);
    }

    public override void LoadSaveData(BuildingSaveData saveData)
    {
        base.LoadSaveData(saveData);
        if (saveData is MineSaveData mineSaveData)
        {
            _currentGold = mineSaveData.CurrentGold;
            
        }
        else
        {
            Debug.LogError("Cast失敗");
        }
    }

    private void ShowCurrentGold()
    {
        _goldText.text = _currentGold.ToString("0");
    }
}

[Serializable]
public class MineSaveData : BuildingSaveData
{
    public float CurrentGold;

    public MineSaveData(int buildingID, BuildingType buildingType, Vector3 position, BuildingCondition currentCondition, float currentGold) 
        : base(buildingID, buildingType, position, currentCondition)
    {
        CurrentGold = currentGold;
    }

    public MineSaveData(BuildingSaveData makeSaveData, float currentGold) : base(makeSaveData)
    {
        CurrentGold = currentGold;
    }
}
