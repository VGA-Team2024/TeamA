using System;
using UnityEngine;
using UniRx;


/// <summary>
/// 現在のリソースの秒間生産量を提供してほしい
/// </summary>
public interface IRpsProvider
{
    ReactiveProperty<float> CurrentRPS { get; set; }
}
public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    /// <summary>
    /// ここでIRpsProviderを継承したコンポーネントを受け取りたい。
    /// </summary>
    [SerializeField] 
    private IRpsProvider _rpsProvider;
    private void Awake()
    {
        _rpsProvider.CurrentRPS.Subscribe(x => _resourceGeneratePerSecond = x);
    }
    
    private float _resourceGeneratePerSecond;
    private decimal _currentResources = 0;
    
    /// <summary>
    /// 現在のリソース値が変更された場合に呼ばれる
    /// </summary>
    public Action<decimal> OnResourceChanged;
    /// <summary>
    /// 現在のリソース生成量が変更された場合に呼ばれる
    /// </summary>
    public Action<float> OnResourceGenerateChanged;

    /// <summary>
    /// 現在のクッキー量のプロパティ
    /// </summary>
    public decimal CurrentResources
    {
        get
        {
            return _currentResources;
        }
        private set
        {
            OnResourceChanged.Invoke(value);
            _currentResources = value;
        }
    }
    /// <summary>
    /// 現在のCPSのプロパティ
    /// </summary>
    public float ResourceGeneratePerSecond
    {
        get
        {
            return _resourceGeneratePerSecond;
        }
        set
        {
            OnResourceGenerateChanged.Invoke(value);
            _resourceGeneratePerSecond = value;
        }
    }

    /// <summary>
    /// クッキーを使う
    /// </summary>
    /// <param name="resources"></param>
    public void UseResources(decimal resources)
    {
        if(_currentResources >= resources)
            _currentResources -= resources;
        else
        {
            Debug.LogWarning("クッキーが足りないので買えませんでした");
        }
    }
    
    


    private void FixedUpdate()
    {
        CurrentResources += (decimal)(_resourceGeneratePerSecond * Time.fixedDeltaTime);
    }

    public void OnClick()
    {
        //_currentTestRPS += 0.1f;
    }


}
