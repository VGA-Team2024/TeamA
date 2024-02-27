using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarSystemManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _nextUnitCountText;
    [SerializeField] private TMP_Text _warTimeText;
    
    private static readonly TimeSpan WarTimeSpan = new(6, 0, 0);

    private DateTime _preTimeSpan;
    private TimeSpan _remainingWarTimeSpan;
    private TimeSpan _currentWarTimeSpan;
    private ResourceManager _resourceManager;

    private int _nextNeedUnitCount = 1;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        TimeSpan span =  DateTime.Now - _preTimeSpan;
        _currentWarTimeSpan = _remainingWarTimeSpan - span;
        _warTimeText.text = $"{_currentWarTimeSpan.Hours}時{_currentWarTimeSpan.Minutes}分{_currentWarTimeSpan.Seconds}秒";
        if (_currentWarTimeSpan.TotalSeconds <= 0)
        {
            AwakeWar();
        }
    }
    
    /// <summary>
    /// 戦争開始
    /// </summary>
    public void AwakeWar()
    {
        _preTimeSpan = DateTime.Now;
        _remainingWarTimeSpan = WarTimeSpan;
        ResourceManager.Instance.TryUseUnitsForWar(_nextNeedUnitCount);
        _nextNeedUnitCount++;
        _nextUnitCountText.text = _nextNeedUnitCount.ToString("0");
    }
    
    private WarSaveData _warSaveData;
    /// <summary>
    /// ゲーム開始時
    /// </summary>
    private void Start()
    {
        
        if (SaveDataManagement.LoadJson<WarSaveData>(out var data))
        {
            _warSaveData = data;
            StartUp();
        }//セーブデータロード時
        else
        {
            _warSaveData = new();
            StartUpFirstTime();
        }//初回ロード時
        
        Application.quitting += OnSave;
        
    }
    private void StartUp()
    {
        _nextNeedUnitCount = _warSaveData.NextNeedUnitCount;
        _preTimeSpan = _warSaveData.ExitTime;
        _remainingWarTimeSpan = _warSaveData.WarTimeSpan;
        Debug.Log($"前回のログインから{(DateTime.Now - _preTimeSpan).ToString(@"dd\:hh\:mm\:ss")} 立ちました  ");
        
        _nextUnitCountText.text = _nextNeedUnitCount.ToString("0");
    }
    private void StartUpFirstTime()
    {
        _preTimeSpan = DateTime.Now;
        _remainingWarTimeSpan = WarTimeSpan;
        _nextNeedUnitCount = 1;
        _nextUnitCountText.text = _nextNeedUnitCount.ToString("0");
    }
    private void OnSave()
    {
        _warSaveData.SaveWarInfo(DateTime.Now ,_currentWarTimeSpan, _nextNeedUnitCount);
        SaveDataManagement.SaveJson(_warSaveData);
    }
    
    
}

[Serializable]
public class WarSaveData : SaveData , ISerializationCallbackReceiver
{
    private DateTime _exitTime ;
    private TimeSpan _warTimeSpan;
    [SerializeField] private string _exitTimeText;
    [SerializeField] private string _warTimeText;

    public TimeSpan WarTimeSpan => _warTimeSpan;

    [SerializeField] private int _nextNeedUnitCount;

    public int NextNeedUnitCount => _nextNeedUnitCount;

    public DateTime ExitTime => _exitTime;
    

    public void SaveWarInfo(DateTime exitTime ,TimeSpan warTimeSpan ,int nextNeedUnitCount)
    {
        _exitTime = exitTime;
        _warTimeSpan = warTimeSpan;
        _nextNeedUnitCount = nextNeedUnitCount;
    }

    public void OnBeforeSerialize()
    {
        _exitTimeText = _exitTime.ToString("yyyy/MM/dd HH:mm:ss");
        _warTimeText = _warTimeSpan.ToString(@"dd\:hh\:mm\:ss");
    }

    public void OnAfterDeserialize()
    {
        if (!string.IsNullOrEmpty(_exitTimeText))
        {
            if (DateTime.TryParse(_exitTimeText, out _exitTime))
            {
            }
        }
        if (!string.IsNullOrEmpty(_warTimeText))
        {
            if (TimeSpan.TryParse(_warTimeText, out _warTimeSpan))
            {
            }
        }
    }
}
