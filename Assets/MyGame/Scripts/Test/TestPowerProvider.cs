using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestPowerProvider :  IPowerProvider
{
    [SerializeField, Header("テスト用秒間資源生成量")]  private float _testFacilityPower;
    [SerializeField, Header("テスト用秒間1クリック生成量")]  private float _testClickPower;
    public ReactiveProperty<float> CurrentFacilityPower { get; } = new();
    public ReactiveProperty<float> CurrentClickPower { get; } = new();
    private CompositeDisposable _disposables;
    public TestPowerProvider()
    {
        _disposables = new();
        this.ObserveEveryValueChanged(x => x._testClickPower)
            .Subscribe(x => CurrentFacilityPower.Value = x).AddTo(_disposables);
        this.ObserveEveryValueChanged(x => x._testClickPower)
            .Subscribe(x => CurrentClickPower.Value = x).AddTo(_disposables);
    }

    ~TestPowerProvider()
    {
        _disposables.Dispose();
    }
}
