using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;


public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    [SerializeField] private float _defaultResources;
    private float _currentResources;
    private int _currentUnitsCount;
    private int _maxUnitCount;

    /// <summary>
    ///     現在のリソース値が変更された場合に呼ばれる
    /// </summary>
    public static Action<float> OnResourceChanged;
    
    /// <summary>
    ///     現在のユニット(兵士数)が変更された場合に呼ばれる
    /// </summary>
    public static Action<int> OnUnitChanged;
    
    /// <summary>
    ///     現在のユニット(兵士数)の最大数が変更された場合に呼ばれる
    /// </summary>
    public static Action<int> OnMaxUnitChanged;
    
    
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
    ///     現在ユニット数プロパティ
    /// </summary>
    public int CurrentUnitsCount
    {
        get => _currentUnitsCount;
        private set
        {
            OnUnitChanged?.Invoke(value);
            _currentUnitsCount = value;
        }
    }
    
    /// <summary>
    ///     現在ユニット数プロパティ
    /// </summary>
    public int MaxUnitCount
    {
        get => _maxUnitCount;
        private set
        {
            OnMaxUnitChanged?.Invoke(value);
            _maxUnitCount = value;
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
            Debug.Log("gold不足で買えませんでした");
            return false;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.MaxUnit.Subscribe(x => MaxUnitCount = x).AddTo(this); 
        CurrentResources = _defaultResources;
    }

    /// <summary>
    /// ユニットを追加する
    /// </summary>
    public void AddUnits(int units)
    {
        CurrentUnitsCount += units;
    }
    
    /// <summary>
    /// ユニットをへらす/戦争を行う
    /// </summary>
    public bool TryUseUnitsForWar(int units)
    {
        if (CurrentUnitsCount >= units)
        {
            CurrentUnitsCount -= units;
            Debug.Log("戦争に勝ちました");
            return true;
        }
        else
        {
            Debug.Log("戦争に負けました");
            //ゴールドを減らす等の処理
            return false;
        }
    }
    
}
