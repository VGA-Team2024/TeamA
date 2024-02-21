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
    /// 現在の建築状況
    /// </summary>
    public BuildingCondition CurrentCondition => _currentCondition;
    
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
    

    #endregion


    #region 施設実装部分

    private BuildingCondition _currentCondition = new BuildingCondition();
    
    protected abstract void OnStart();
    protected abstract void OnFixedUpdate();
    
    public void StartBuilding()
    {
        print("作業開始");
        _currentCondition.IsBuilding = true;
    }
    public void EndBuilding()
    {
        _currentCondition.IsBuilding = false;
    }

    private void Start()
    {
        OnStart();
    }

    private void FixedUpdate()
    {
        
        if (_currentCondition.IsActivate)
        {
            OnFixedUpdate();
        }
        else
        {
            if (_currentCondition.IsBuilding)
            {
                //建設する
                _currentCondition.CurrentBuildTime += Time.fixedDeltaTime;
                _buildingTimeText.text = (_buildingData.BuildTime - _currentCondition.CurrentBuildTime).ToString("0");
                if (_currentCondition.CurrentBuildTime > _buildingData.BuildTime)
                {
                    Debug.Log("建設終了");
                    OnBuildingComplete?.Invoke();
                    BuildingManager.Instance.RegisterBuilding(this);
                    _currentCondition.IsActivate = true;
                }
            }
        }
    }

    private void OnSave()
    {
        
        //下の状態を保存する
        // _currentCondition.IsBuilding = false;
        // _currentCondition.IsActivate = false;
        // _currentCondition.CurrentBuildTime = 0;
    }
    private void OnLoad()
    {
        //下の状態をロードする
        // _currentCondition.IsBuilding = false;
        // _currentCondition.IsActivate = false;
        // _currentCondition.CurrentBuildTime = 0;
    }
    #endregion
}

/// <summary>
/// 現在の建築状況
/// </summary>
[Serializable]
public class BuildingCondition
{
    public bool IsBuilding;
    public bool IsActivate;
    public float CurrentBuildTime;
}
