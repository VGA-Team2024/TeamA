using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 兵士育成所
/// </summary>
public class Barrack : BuildingBase
{
    
    [SerializeField, Header("生成間隔")] private float _createSpan;
    
    [SerializeField,Header("兵士作成コスト")] private float _cost;
    
    private ResourceManager _resourceManager;
    private BuildingManager _buildingManager;
    private SoldierManager _soldierManager;
    private float _createTimer;
    public override void OnClick()
    {
        if (!CurrentCondition.IsActivate) return;
        if (_createTimer < _createSpan)
        {
            Debug.Log("生成間隔が早すぎます");
            return;
        }
        if (!_buildingManager.IsUnitCreatable())
        {
            Debug.Log("もう作成できません");
            return;
        }
        if (_resourceManager.IsUseResources(_cost))
        {
            _resourceManager.UseResources(_cost); 
            _buildingManager.CreateUnit();
            _createTimer = 0f;
        }
    }

    protected override void OnStart()
    {
        _resourceManager = ResourceManager.Instance;
        _buildingManager = BuildingManager.Instance;
        _soldierManager = FindObjectOfType<SoldierManager>();
    }

    protected override void OnFixedUpdate()
    {
        _createTimer += Time.fixedDeltaTime;
    }
    
}
