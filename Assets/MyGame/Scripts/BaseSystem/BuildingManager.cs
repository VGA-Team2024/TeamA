using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class BuildingManager : SingletonMonoBehavior<BuildingManager>
{
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private BuildingDataSet _buildingDataSet;
    
    [SerializeField , Header("初期拠点")] private Transform _firstBaseCampTransform;
    [SerializeField , Header("初期鉱山")] private Transform _firstMineTransform;
    


    #region 公開用
    
    /// <summary>
    /// 施設価格
    /// </summary>
    public Dictionary<BuildingType, float> BuildingPrices => _buildingPrices;
    
    /// <summary>
    /// 施設所持上限
    /// </summary>
    public  Dictionary<BuildingType, int> MaxBuildingStocks => _maxBuildingStocks;
    
    /// <summary>
    /// 施設名
    /// </summary>
    public Dictionary<BuildingType, string> BuildingNames => _buildingNames;
    
    /// <summary>
    /// 現在の施設リスト
    /// </summary>
    public SortedDictionary<BuildingType, List<BuildingBase>> BuildingList => _buildingList;
    
    /// <summary>
    /// 現在のUnit上限
    /// </summary>
    public IReadOnlyReactiveProperty<int> MaxUnit => _maxUnit;
    
    /// <summary>
    /// ユニットが生成可能か
    /// </summary>
    /// <returns></returns>
    public bool IsUnitCreatable()
    {
        return _resourceManager.CurrentUnitsCount < _maxUnit.Value;
    }
    
    /// <summary>
    /// 建物を呼び出す。
    /// </summary>
    public BuildingBase CreateBuilding(BuildingType buildingType)
    {
        //先にリソースを消費する
        _resourceManager.UseResources(_buildingPrices[buildingType]);
        return InstantiateBuilding(buildingType);
    }

    private BuildingBase InstantiateBuilding(BuildingType buildingType , BuildingCondition buildingCondition = default)
    {
        BuildingBase building = Instantiate(_buildingDataSet.Buildings[(int)buildingType].BuildingBase);
        building.Initialize(_nextBuildingID ,buildingCondition);
        _buildingList[buildingType].Add(building);
        _nextBuildingID += 1;
        return building;
    }
    
    /// <summary>
    /// 建築が終了した際に呼び出される　。TODO 建築スタート時に呼ばれて欲しい。
    /// </summary>
    public void RegisterBuilding(BuildingBase building)
    {
        UpdateBuildings();
    }
    
    /// <summary>
    /// 建築可能かどうか
    /// </summary>
    public bool IsBuildable(BuildingType buildingType)
    {
        if (_resourceManager.CurrentResources < _buildingPrices[buildingType])
        {
            Debug.Log("資源が足りません");
            return false;
        }
        if (_buildingList[buildingType].Count >= _maxBuildingStocks[buildingType])
        {
            Debug.Log("建築上限です");
            return false;
        }

        return true;
    }

    
    #endregion

    #region 非公開

    /// <summary>
    /// 現在の施設リスト
    /// </summary>
    private readonly SortedDictionary<BuildingType , List<BuildingBase>> _buildingList = new ();

    /// <summary>
    /// 次の施設ID
    /// </summary>
    private static int _nextBuildingID = 0;
    
    /// <summary>
    /// キャッシュ用
    /// </summary>
    private static readonly Dictionary<BuildingType, float> _buildingPrices = new();
    private static readonly Dictionary<BuildingType, int> _maxBuildingStocks = new();
    private static readonly Dictionary<BuildingType, string> _buildingNames = new();
    /// <summary>
    /// 現在のUnit上限、ResourceManagerに渡す用
    /// </summary>
    private readonly ReactiveProperty<int> _maxUnit = new(0);

    private BuildingsSaveData _buildingsSaveData;
    
    
    /// <summary>
    /// ユニットの生成時にArmyCamp(待機所)の目的地を返す、生成はBarrack(兵士育成所)をクリック
    /// </summary>
    /// <returns>ArmyCampの中で待機可能な位置を返す。</returns>
    public Transform CreateUnit(GameObject soldier)
    {
        _resourceManager.AddUnits(1);
        return _buildingList[BuildingType.ArmyCamp].OfType<ArmyCamp>().FirstOrDefault(x => x.IsUnitCreatable())?.AddUnit(soldier);
    }
    
    public void RemoveUnit(int unitCount)
    {
        var armyCamps =  _buildingList[BuildingType.ArmyCamp].OfType<ArmyCamp>();
        for (int i = 0; i < unitCount; i++)
        {
            armyCamps.FirstOrDefault(x => x.IsUnitRemovable()).RemoveUnit();
        }
    }
    
    /// <summary>
    /// 初期化
    /// </summary>
    protected override void OnAwake()
    {
        InitializeDictionary();
        
        if (SaveDataManagement.LoadJson<BuildingsSaveData>(out var data))
        {
            _buildingsSaveData = data;
            StartUp();
        }//セーブデータロード時
        else
        {
            _buildingsSaveData = new();
            StartUpFirstTime();
        }//初回ロード時
        
        Application.quitting += OnSave;
    }

    /// <summary>
    /// 初回起動時
    /// </summary>
    private void StartUpFirstTime()
    {
        _nextBuildingID = 0;
        
        var obj = InstantiateBuilding(BuildingType.BaseCamp , new BuildingCondition(true , true, 0 ));
        obj.transform.position = _firstBaseCampTransform.position;
        
        var obj2 =  InstantiateBuilding(BuildingType.Mine , new BuildingCondition(true , true, 0 ));
        obj2.transform.position = _firstMineTransform.position;
    }

    /// <summary>
    /// 初回以外、ゲームロード
    /// </summary>
    private void StartUp()
    {
        _nextBuildingID = _buildingsSaveData.NextBuildingID;
    }
    
    private void OnSave()
    {
        _buildingsSaveData.SaveBuildings(_buildingList ,_nextBuildingID);
        
        SaveDataManagement.SaveJson(_buildingsSaveData);
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
            BuildingType buildingType = buildingData.BuildingType;
            _buildingList.Add(buildingType, new List<BuildingBase>());
            _maxBuildingStocks.Add(buildingType, buildingData.MaxAmount);
            _buildingPrices.Add(buildingType, buildingData.Price);
            _buildingNames.Add(buildingType, buildingData.Name);
        }
    }
    #endregion
    

}

/// <summary>
/// 建物セーブクラス
/// </summary>
[Serializable]
public class BuildingsSaveData : SaveData
{
    public int NextBuildingID;
    [SerializeField] private BuildingSaveData[] _buildingsSaveData;
    [SerializeField] private MineSaveData[] _minesSaveData;
    
    public void SaveBuildings(SortedDictionary<BuildingType , List<BuildingBase>> buildingList ,int nextBuildingID)
    {
        NextBuildingID = nextBuildingID;
        List<BuildingSaveData>  buildingsDataList = new();
        foreach (var kv in buildingList)
        {
            foreach (var buildingBase in kv.Value)
            {
                buildingsDataList.Add(buildingBase.MakeSaveData());
            }
        }
        _buildingsSaveData = buildingsDataList.ToArray();
        _minesSaveData = buildingsDataList.OfType<MineSaveData>().ToArray();
    }
}
