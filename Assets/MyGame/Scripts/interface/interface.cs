using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


/// <summary>
/// 現在のリソースの秒間生産力を提供してほしい
/// </summary>
public interface IFacilityPowerProvider
{
    ReactiveProperty<decimal> CurrentFacilityPower { get;}
}
/// <summary>
/// 現在の1クリック生産力を提供してほしい
/// </summary>
public interface IClickPowerProvider
{
    ReactiveProperty<decimal> CurrentClickPower { get; }
}
