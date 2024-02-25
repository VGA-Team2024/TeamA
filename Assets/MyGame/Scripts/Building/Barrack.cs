using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 兵士育成所
/// </summary>
public class Barrack : BuildingBase
{
    [SerializeField,Header("兵士プレハブ")] private GameObject _soldier;
    
    [SerializeField,Header("兵士作成コスト")] private float _cost;
    
    private ResourceManager _resourceManager;
    private BuildingManager _buildingManager;
    
    public override void OnClick()
    {
        if (!_buildingManager.IsUnitCreatable())
        {
            Debug.Log("もう作成できません");
            return;
        }
        if (_resourceManager.IsUseResources(_cost))
        {
            _resourceManager.UseResources(_cost);
            //兵士の生成
            var soldier = Instantiate(_soldier);
            //TODO 兵士プレハブの目的地を設定したい。
            var armyTransform = _buildingManager.CreateUnit(soldier);
            
            soldier.transform.SetParent(armyTransform);
            soldier.transform.position = armyTransform.position + Vector3.back;
        }
    }

    protected override void OnStart()
    {
        _resourceManager = ResourceManager.Instance;
        _buildingManager = BuildingManager.Instance;
    }

    protected override void OnFixedUpdate()
    {

    }
    
}
