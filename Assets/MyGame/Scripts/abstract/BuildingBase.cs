using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 現在の建築状況
/// </summary>
[Serializable]
public class BuildingCondition : IEquatable<BuildingCondition>
{
    public bool IsBuilding;
    public bool IsActivate;
    public float CurrentBuildTime;

    public BuildingCondition(bool isBuilding, bool isActivate, float currentBuildTime)
    {
        IsBuilding = isBuilding;
        IsActivate = isActivate;
        CurrentBuildTime = currentBuildTime;
    }

    public bool Equals(BuildingCondition other)
    {
        return IsBuilding.Equals(other.IsActivate) && IsActivate.Equals(other.IsActivate) && CurrentBuildTime.Equals(other.CurrentBuildTime) ;
    }
}


[Serializable]
public class BuildingSaveData : SaveData ,IEquatable<BuildingSaveData>
{
    //publicかSerializeField属性を持つもの出ないと保存できない
    public int BuildingID;
    public Vector3 Position;
    public BuildingType Type;
    public BuildingCondition CurrentCondition;
   

    public BuildingSaveData(int buildingID, BuildingType buildingType , Vector3 position, BuildingCondition currentCondition)
    {
        BuildingID = buildingID;
        Position = position;
        Type = buildingType;
        CurrentCondition = currentCondition;
    }
    
    public bool Equals(BuildingSaveData other)
    {
        return Type.Equals(other.Type) && CurrentCondition.Equals(other.CurrentCondition) && Position.Equals(other.Position);
    }
}

public abstract class BuildingBase : MonoBehaviour
{
    //TODO 生成時に固有のIDをつけないとデータの連携が取りにくい。
    [SerializeField, Header("建物の当たり判定")] private SphereCollider _buildingCollider;
    [SerializeField , Header("建物データ")] private BuildingData _buildingData;
    [SerializeField, Header("TestUI / 建築時間の表示")] private Text _buildingTimeText;

    /// <summary>
    /// 建物の固有ID
    /// </summary>
    private int _buildingID;
    
    private BuildingCondition _currentCondition;
    
    #region 公開箇所
    
    /// <summary>
    /// 建物の固有ID
    /// </summary>
    public int BuildingID => _buildingID;
    
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

    /// <summary>
    /// 建物の生成時にIDをセットする
    /// </summary>
    public void Initialize(int id ,  BuildingCondition currentCondition = default)
    {
        _buildingID = id;
        _currentCondition = currentCondition ?? new BuildingCondition(false , false ,0);
        
        if (_currentCondition.IsActivate)
            _buildingTimeText.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// コンディションの変更 / 初回起動時、ロード時等
    /// </summary>
    
    #endregion


    #region 施設実装部分

    
    
    protected virtual void OnStart(){}
    protected virtual void OnFixedUpdate(){}
    
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
                    _buildingTimeText.gameObject.SetActive(false);
                }
            }
        }
    }

    public virtual BuildingSaveData MakeSaveData()
    {
        return new BuildingSaveData(_buildingID , BuildingType, transform.position, _currentCondition);
        //下の状態を保存する
        // _currentCondition.IsBuilding = false;
        // _currentCondition.IsActivate = false;
        // _currentCondition.CurrentBuildTime = 0;
    }
    public virtual void OnLoad()
    {
        //下の状態をロードする
        // _currentCondition.IsBuilding = false;
        // _currentCondition.IsActivate = false;
        // _currentCondition.CurrentBuildTime = 0;
    }
    #endregion
}

