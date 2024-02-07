using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
/// <summary>
/// フィールドにアタッチされていればResourceManager.Instance.○○で利用できるクラス。
/// シングルトンを継承しています。
/// </summary>
public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    [SerializeField,Header("リソーステスト　1秒間あたりのリソース生成量")] 
    private float _resourceGeneratePerSecond;
    private decimal _currentResources = 0;
    //Calculate Classが現在のRPSを計算してほしい
    private float _currentTestRPS;
    
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
    
    
    private void Start()
    {
        //this.ObserveEveryValueChanged(x => x._currentTestRPS).Subscribe(x => ResourceGeneratePerSecond = x);
    }

    private void FixedUpdate()
    {
        CurrentResources += (decimal)(_resourceGeneratePerSecond * Time.fixedDeltaTime);
    }

    public void OnClick()
    {
        _currentTestRPS += 0.1f;
    }


}
