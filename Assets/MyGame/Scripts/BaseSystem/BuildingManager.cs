using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class BuildingManager : SingletonMonoBehavior<BuildingManager>
{
    [SerializeField , Header("初期拠点")] private BaseCamp _baseCamp;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private BuildingDataSet _buildingDataSet;
    
    private readonly Dictionary<BuildingType , List<BuildingBase>> _buildingList = new ();
    /// <summary>
    /// キャッシュ用
    /// </summary>
    private static readonly Dictionary<BuildingType, float> _buildingPrices = new();
    private static readonly Dictionary<BuildingType, int> _maxBuildingStocks = new();
    
    /// <summary>
    /// 現在のUnit上限、ResourceManagerに渡す用
    /// </summary>
    private readonly ReactiveProperty<int> _maxUnit = new(0);
    public IReadOnlyReactiveProperty<int> MaxUnit => _maxUnit;
    
    /// <summary>
    /// 初期化
    /// </summary>
    protected override void OnAwake()
    {
        InitializeDictionary();
    }
    
    public bool IsUnitCreatable()
    {
        return _resourceManager.CurrentUnitsCount < _maxUnit.Value;
    }
    
    /// <summary>
    /// ユニットの生成時にArmyCamp(待機所)の目的地を返す、生成はBarrack(兵士育成所)をクリック
    /// </summary>
    /// <returns></returns>
    public Transform CreateUnit()
    {
        _resourceManager.AddUnits(1);
        // ArmyCampの中で待機可能な位置を返す。
        return _buildingList[BuildingType.ArmyCamp].OfType<ArmyCamp>().FirstOrDefault(x => x.IsUnitCreatable()).AddUnit();
    }
    

    /// <summary>
    /// 建築可能かどうか
    /// </summary>
    public bool IsBuildable(BuildingType buildingType)
    {
        return _resourceManager.CurrentResources >= _buildingPrices[buildingType] &&
               _buildingList[buildingType].Count < _maxBuildingStocks[buildingType];
    }

    /// <summary>
    /// 建物を呼び出す。
    /// </summary>
    public BuildingBase InstantiateBuilding(BuildingType buildingType , Transform parentTransform)
    {
        //先にリソースを消費する
        _resourceManager.UseResources(_buildingPrices[buildingType]);
        return Instantiate(_buildingDataSet.Buildings[(int)buildingType].BuildingBase , parentTransform);
    }
    
    /// <summary>
    /// 建築が終了した際に呼び出される　。
    /// </summary>
    public void RegisterBuilding(BuildingBase building)
    {
        if (_buildingList[building.BuildingType] == null)
        {
            _buildingList[building.BuildingType] = new List<BuildingBase>();
        }
        _buildingList[building.BuildingType].Add(building);
        UpdateBuildings();
    }
    
    /// <summary>
    /// 建築物リストに変更があった際に呼び出す。
    /// </summary>
    private void UpdateBuildings()
    { 
        _maxUnit.Value = _buildingList[BuildingType.ArmyCamp].OfType<ArmyCamp>().Select(x => x.MaxUnit).Sum();
    }
    
    /// <summary>
    ///     辞書の初期化
    /// </summary>
    private void InitializeDictionary()
    {
        foreach (var buildingData in _buildingDataSet.Buildings)
        {
            _buildingList.Add(buildingData.BuildingType, new List<BuildingBase>());
            _maxBuildingStocks.Add(buildingData.BuildingType, buildingData.MaxAmount);
            _buildingPrices.Add(buildingData.BuildingType, buildingData.Price);
        }
        //ベースキャンプを追加
        //_buildingList[BuildingType.BaseCamp].Add(_baseCamp);
    }
}
