using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 生産力を提供するクラス
/// </summary>
public abstract class PowerProviderBase : MonoBehaviour , IFacilityPowerProvider , IClickPowerProvider
{
    public abstract ReactiveProperty<decimal> CurrentFacilityPower { get; }
    public abstract ReactiveProperty<decimal> CurrentClickPower { get; }
}
