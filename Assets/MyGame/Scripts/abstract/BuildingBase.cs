using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
    [SerializeField , Header("建物データ")] private BuildingData _buildingData;
    [SerializeField, Header("大工の設置判定をとる")] private Collider _buildingTrigger;
    
    /// <summary>
    /// 建物のデータ
    /// </summary>
    public BuildingData BuildingData => _buildingData;
    /// <summary>
    /// 建設が完了した際に呼び出される
    /// </summary>
    public event Action OnBuildingComplete ;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        _isBuilding = false;
        _isActivate = false;
        _currentBuildTime = 0;
    }

    
    public abstract void OnClick();
    
    private bool _isBuilding;
    private bool _isActivate;
    private float _currentBuildTime;
    protected abstract void OnAwake();
    protected abstract void OnFixedUpdate();
    
    public void StartBuilding()
    {
        _isBuilding = true;
    }
    public void EndBuilding()
    {
        _isBuilding = false;
    }

    private void Awake()
    {
        //Triggerバインド
        _buildingTrigger.OnTriggerEnterAsObservable().Where(x => x.CompareTag("Builder"))
            .Subscribe(_ => StartBuilding()).AddTo(this);
        _buildingTrigger.OnTriggerExitAsObservable().Where(x => x.CompareTag("Builder"))
            .Subscribe(_ => EndBuilding()).AddTo(this);
        
        OnAwake();
    }

    private void FixedUpdate()
    {
        
        if (_isActivate)
        {
            OnFixedUpdate();
        }
        else
        {
            if (_isBuilding)
            {
                //建設する
                _currentBuildTime += Time.fixedDeltaTime;
                if (_currentBuildTime > _buildingData.BuildTime)
                {
                    Debug.Log("建設終了");
                    OnBuildingComplete?.Invoke();
                    _isActivate = true;
                }
            }
        }
    }

    private void OnSave()
    {
        //下の状態を保存する
        // _isBuilding = false;
        // _isActivate = false;
        // _currentBuildTime = 0;
    }
    private void OnLoad()
    {
        //下の状態をロードする
        // _isBuilding = false;
        // _isActivate = false;
        // _currentBuildTime = 0;
    }
}
