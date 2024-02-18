using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildlingManager : SingletonMonoBehavior<BuildlingManager>
{
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private BuildingDataSet _buildingDataSet;
    private readonly List<BuildingBase> _buildingList = new ();
    /// <summary>
    /// アクセス用
    /// </summary>
    private static readonly Dictionary<BuildingType, float> _buildingPrices = new();
    private static readonly Dictionary<BuildingType, int> _maxBuildingStocks = new();
    /// <summary>
    /// 現在の施設保持数
    /// </summary>
    private readonly Dictionary<BuildingType, int> _currentBuildingStocks = new();
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

    /// <summary>
    /// 建物を呼び出す。
    /// </summary>
    public BuildingBase InstantiateBuilding(BuildingType buildingType , Transform parentTransform)
    {
        var building = Instantiate(_buildingDataSet.Buildings[(int)buildingType].Building , parentTransform);
        _buildingList.Add(building);
        return building;
    }
    /// <summary>
    /// 建築する。
    /// </summary>
    /// <param name="building"></param>
    public void Build(BuildingBase building)
    {
        _resourceManager.TryUseResources(_buildingPrices[building.BuildingType]);
        _currentBuildingStocks[building.BuildingType]++;
    }

    /// <summary>
    ///     辞書の初期化
    /// </summary>
    private void InitializeDictionary()
    {
        foreach (var buildingData in _buildingDataSet.Buildings)
        {
            _currentBuildingStocks.Add(buildingData.BuildingType, 0);
            _maxBuildingStocks.Add(buildingData.BuildingType, buildingData.MaxAmount);
            _buildingPrices.Add(buildingData.BuildingType, buildingData.Price);
        }
        //ベースキャンプを追加
        _currentBuildingStocks[BuildingType.BaseCamp]++;
    }
}
