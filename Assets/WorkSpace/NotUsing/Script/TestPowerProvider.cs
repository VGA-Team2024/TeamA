using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestPowerProvider : PowerProviderBase
{
    [SerializeField, Header("テスト用秒間資源生成量")]  private float _testFacilityPower;
    [SerializeField, Header("テスト用秒間1クリック生成量")]  private float _testClickPower;

    public override ReactiveProperty<decimal> CurrentFacilityPower { get; } = new();
    public override ReactiveProperty<decimal> CurrentClickPower { get; } = new();

    private void FixedUpdate()
    {
        CurrentFacilityPower.Value = (decimal)_testFacilityPower;
        CurrentClickPower.Value = (decimal)_testClickPower;
    }

    
}
