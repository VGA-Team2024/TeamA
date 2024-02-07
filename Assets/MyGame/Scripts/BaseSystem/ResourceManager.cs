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
    private float _cookiesGeneratePerSecond;
    private decimal _currentCookies = 0;
    //Calculate Classが現在のCPSを計算してほしい
    private float _currentTestCPS;
    
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
    public decimal CurrentCookies
    {
        get
        {
            return _currentCookies;
        }
        private set
        {
            OnResourceChanged.Invoke(value);
            _currentCookies = value;
        }
    }
    /// <summary>
    /// 現在のCPSのプロパティ
    /// </summary>
    public float CookiesGeneratePerSecond
    {
        get
        {
            return _cookiesGeneratePerSecond;
        }
        set
        {
            OnResourceGenerateChanged.Invoke(value);
            _cookiesGeneratePerSecond = value;
        }
    }

    /// <summary>
    /// クッキーを使う
    /// </summary>
    /// <param name="cookies"></param>
    public void UseCoolies(decimal cookies)
    {
        if(_currentCookies >= cookies)
            _currentCookies -= (decimal) cookies;
        else
        {
            Debug.LogWarning("クッキーが足りないので買えませんでした");
        }
    }
    
    
    private void Start()
    {
        //this.ObserveEveryValueChanged(x => x._currentTestCPS).Subscribe(x => CookiesGeneratePerSecond = x);
    }

    private void FixedUpdate()
    {
        CurrentCookies += (decimal)(_cookiesGeneratePerSecond * Time.fixedDeltaTime);
    }

    public void OnClick()
    {
        _currentTestCPS += 0.1f;
    }


}
