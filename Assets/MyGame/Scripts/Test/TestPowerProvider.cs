using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestPowerProvider : PowerProviderBase
{
    [SerializeField, Header("テスト用秒間資源生成量")]  private float _testFacilityPower;
    [SerializeField, Header("テスト用秒間1クリック生成量")]  private float _testClickPower;

    public override ReactiveProperty<float> CurrentFacilityPower { get; } = new();
    public override ReactiveProperty<float> CurrentClickPower { get; } = new();

    private void FixedUpdate()
    {
        CurrentFacilityPower.Value = _testFacilityPower;
        CurrentClickPower.Value = _testClickPower;
    }

    
}
