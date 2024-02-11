using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    [SerializeField , Header("生産力を実装するクラス")] private PowerProviderBase _powerProvider　;
    
    private decimal _currentResources;
    private float _currentFacilityPower;
    private float _currentClickPower;

    /// <summary>
    ///     現在のリソース値が変更された場合に呼ばれる
    /// </summary>
    public static Action<decimal> OnResourceChanged;

    /// <summary>
    ///     現在のリソース生成量が変更された場合に呼ばれる
    /// </summary>
    public static Action<float> OnFacilityPowerChanged;

    /// <summary>
    ///     現在のリソース生成量が変更された場合に呼ばれる
    /// </summary>
    public static Action<float> OnClickPowerChanged;
    
    /// <summary>
    ///     現在リソース量プロパティ
    /// </summary>
    public decimal CurrentResources
    {
        get => _currentResources;
        private set
        {
            OnResourceChanged?.Invoke(value);
            _currentResources = value;
        }
    }

    /// <summary>
    /// 現在の生産力のプロパティ
    /// </summary>
    public float CurrentFacilityPower
    {
        get => _currentFacilityPower;
        private set
        {
            OnFacilityPowerChanged?.Invoke(value);
            _currentFacilityPower = value;
        }
    }
    
    /// <summary>
    ///     現在の１クリック生産力のプロパティ
    /// </summary>
    public float CurrentClickPower
    {
        get => _currentClickPower;
        private set
        {
            OnClickPowerChanged?.Invoke(value);
            _currentClickPower = value;
        }
    }
    /// <summary>
    /// 自動生産
    /// </summary>
    private void FixedUpdate()
    {
        CurrentResources += (decimal)(_currentFacilityPower * Time.fixedDeltaTime);
    }

    protected override void OnAwake()
    {
        _powerProvider.CurrentFacilityPower.Subscribe(x => CurrentFacilityPower = x).AddTo(this);
        _powerProvider.CurrentClickPower.Subscribe(x => CurrentClickPower = x).AddTo(this);
    }

    /// <summary>
    /// クッキーを消費する。
    /// </summary>
    /// <param name="resources"></param>
    public bool TryUseResources(decimal resources)
    {
        if (CurrentResources >= resources)
        {
            CurrentResources -= resources;
            return true;
        }
        else
        {
            Debug.LogWarning("クッキーが足りないので買えませんでした");
            return false;
        }
    }
    
    /// <summary>
    /// クリックするときに呼ばれる
    /// </summary>
    public void OnClick()
    {
        CurrentResources += (decimal)CurrentClickPower;
    }
}
