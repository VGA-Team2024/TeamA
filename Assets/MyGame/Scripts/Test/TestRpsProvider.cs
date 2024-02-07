using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class TestRpsProvider : MonoBehaviour , IRpsProvider
{
    [SerializeField, Header("テスト用秒間資源生成量")]
    private float _testRPS;
    public ReactiveProperty<float> CurrentRPS { get; set; }

    public TestRpsProvider()
    {
        CurrentRPS = new(_testRPS);
    }
}
