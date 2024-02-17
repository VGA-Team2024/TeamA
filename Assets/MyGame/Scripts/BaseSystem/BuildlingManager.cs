using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildlingManager : SingletonMonoBehavior<BuildlingManager>
{
    [SerializeField] private ResourceManager _resourceManager;
    /// <summary>
    /// TODO インスタンス化されてないからどうする
    /// </summary>
    [SerializeField] private List<BuildingBase> _buildingList;
    private readonly Dictionary<BuildingType, float> _buildingPrices = new();
    private readonly Dictionary<BuildingType, int> _currentBuildingStocks = new();
    private readonly Dictionary<BuildingType, int> _maxBuildingStocks = new();

    protected override void OnAwake()
    {
        InitializeDictionary();
    }
    
    /// <summary>
    /// 建築可能かどうか
    /// </summary>
    public bool IsBuildable(BuildingType buildingType)
    {
        return _resourceManager.CurrentResources >= _buildingPrices[buildingType] &&
               _currentBuildingStocks[buildingType] < _maxBuildingStocks[buildingType];
    }

    public void Build(BuildingType buildingType)
    {
        _resourceManager.TryUseResources(_buildingPrices[buildingType]);
        _currentBuildingStocks[buildingType]++;
        
    }

    /// <summary>
    ///     辞書の初期化
    /// </summary>
    private void InitializeDictionary()
    {
        foreach (var building in _buildingList)
        {
            var data = building.BuildingData;
            _currentBuildingStocks.Add(data.BuildingType, 0);
            _maxBuildingStocks.Add(data.BuildingType, data.MaxAmount);
            _buildingPrices.Add(data.BuildingType, data.Price);
        }
        //ベースキャンプを追加
        _currentBuildingStocks[BuildingType.BaseCamp]++;
    }
}
