using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuildingBase : MonoBehaviour
{
    [SerializeField, Header("建物の当たり判定")] private SphereCollider _buildingCollider;
    [SerializeField , Header("建物データ")] private BuildingData _buildingData;
    [SerializeField, Header("TestUI / 建築時間の表示")] private Text _buildingTimeText;

    #region 公開箇所

    /// <summary>
    /// 建物のサイズ/半径
    /// </summary>
    public float BuildingRadius => _buildingCollider.radius;
    
    /// <summary>
    /// 建物の位置
    /// </summary>
    public Vector3 Position => transform.position;
    
    
    /// <summary>
    /// 建物のタイプ
    /// </summary>
    public BuildingType BuildingType => _buildingData.BuildingType;
    
    /// <summary>
    /// 建設が完了した際に呼び出される
    /// </summary>
    public event Action OnBuildingComplete ;
    
    /// <summary>
    /// クリックイベント
    /// </summary>
    public abstract void OnClick();
    
    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        _isBuilding = false;
        _isActivate = false;
        _currentBuildTime = 0;
    }

    #endregion


    #region 施設実装部分
    
    private bool _isBuilding;
    private bool _isActivate;
    private float _currentBuildTime;
    protected abstract void OnStart();
    protected abstract void OnFixedUpdate();
    
    public void StartBuilding()
    {
        print("作業開始");
        _isBuilding = true;
    }
    public void EndBuilding()
    {
        _isBuilding = false;
    }

    private void Start()
    {
        OnStart();
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
                _buildingTimeText.text = (_buildingData.BuildTime - _currentBuildTime).ToString("0");
                if (_currentBuildTime > _buildingData.BuildTime)
                {
                    Debug.Log("建設終了");
                    OnBuildingComplete?.Invoke();
                    BuildingManager.Instance.RegisterBuilding(this);
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
    #endregion
}
