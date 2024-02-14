using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    private float _currentResources;

    /// <summary>
    ///     現在のリソース値が変更された場合に呼ばれる
    /// </summary>
    public static Action<float> OnResourceChanged;

    /// <summary>
    ///     現在リソース量プロパティ
    /// </summary>
    public float CurrentResources
    {
        get => _currentResources;
        private set
        {
            OnResourceChanged?.Invoke(value);
            _currentResources = value;
        }
    }
    /// <summary>
    /// リソースを追加する
    /// </summary>
    /// <param name="gold"></param>
    public void AddResources(float gold)
    {
        CurrentResources += gold;
    }
    
    /// <summary>
    /// リソースを消費する。
    /// </summary>
    /// <param name="gold"></param>
    public bool TryUseResources(float gold)
    {
        if (CurrentResources >= gold)
        {
            CurrentResources -= gold;
            return true;
        }
        else
        {
            Debug.LogWarning("gold不足で買えませんでした");
            return false;
        }
    }
    
    // /// <summary>
    // /// クリックするときに呼ばれる
    // /// </summary>
    // public void OnClick()
    // {
    //     CurrentResources += (decimal)CurrentClickPower;
    // }
}
