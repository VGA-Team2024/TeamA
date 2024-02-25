using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarSystemManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _NextUnitCountText;
    [SerializeField] private TMP_Text _warTimeText;
    /// <summary>
    /// 戦争の時間
    /// </summary>
    private readonly TimeSpan _warTimeSpan = new(0, 0, 10);
    private DateTime _preTimeSpan;
    private ResourceManager _resourceManager;

    private int _nextUnitCount = 1;
    void Start()
    {
        
        _resourceManager = ResourceManager.Instance;
        _preTimeSpan = DateTime.Now;
        _nextUnitCount = 1;
        _NextUnitCountText.text = _nextUnitCount.ToString("0");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeSpan span =  DateTime.Now - _preTimeSpan;
        TimeSpan warTime = _warTimeSpan - span;
        _warTimeText.text = $"{warTime.Hours}時{warTime.Minutes}分{warTime.Seconds}秒";
        if (warTime.Seconds == 0)
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
        _resourceManager.TryUseUnitsForWar(_nextUnitCount);
        _nextUnitCount++;
        _NextUnitCountText.text = _nextUnitCount.ToString("0");
    }
}
