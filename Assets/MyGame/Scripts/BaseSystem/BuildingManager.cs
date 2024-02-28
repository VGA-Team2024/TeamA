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
    public BuildingBase CreateBuilding(BuildingType buildingType ,Vector3 position)
    {
        //先にリソースを消費する
        _resourceManager.UseResources(_buildingPrices[buildingType]);
        var building = InstantiateBuilding(_nextBuildingID , buildingType , position);
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

        
    /// <summary>
    /// ユニットの生成時にArmyCamp(待機所)の目的地を返す、生成はBarrack(兵士育成所)をクリック
    /// </summary>
    /// <returns>ArmyCampの中で待機可能な位置を返す。</returns>
    public void CreateUnit()
    {
        _buildingList[BuildingType.ArmyCamp].OfType<ArmyCamp>().FirstOrDefault(x => x.IsUnitCreatable())?.CreateUnit();
    }
    
    /// <summary>
    /// ユニットをArmycampから削除する
    /// </summary>
    /// <param name="unitCount"></param>
    public void RemoveUnit(int unitCount)
    {
        var armyCamps =  _buildingList[BuildingType.ArmyCamp].OfType<ArmyCamp>();
        for (int i = 0; i < unitCount; i++)
        {
            armyCamps.FirstOrDefault(x => x.IsUnitRemovable())?.RemoveUnit();
        }
    }

    public void ReleaseGold()
    {
        foreach (var mine in _buildingList[BuildingType.Mine].OfType<Mine>())
        {
            mine.ReleaseGold();
        }
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
    private static int _nextBuildingID;
    
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
    /// 初期化
    /// </summary>
    protected override void OnAwake()
    {
        InitializeDictionary();
    }
    /// <summary>
    /// ゲーム開始時
    /// </summary>
    private void Start()
    {
        
        
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
        InstantiateBuilding(0 ,BuildingType.BaseCamp ,_firstBaseCampTransform.position , new BuildingCondition(false , true, 0 ));
        InstantiateBuilding(1 ,BuildingType.Mine ,_firstMineTransform.position, new BuildingCondition(false , true, 0 ));
        _nextBuildingID = 2;
    }

    /// <summary>
    /// 初回以外、ゲームロード
    /// </summary>
    private void StartUp()
    {
        _nextBuildingID = _buildingsSaveData.NextBuildingID;
        //セーブデータの連結（和集合）、重複削除
        var buildingsSaveData = _buildingsSaveData.ArmyCampsArray.Union<BuildingSaveData>(_buildingsSaveData.MinesArray).Union(_buildingsSaveData.BuildingsArray);
        List<BuildingBase> buildings = new List<BuildingBase>();
        foreach (var saveData in buildingsSaveData)
        {
            buildings.Add(LoadBuildings(saveData));
        }

        var builder = FindObjectOfType<Builder>();

        var isBuildingObj = buildings.FirstOrDefault(x => x.CurrentCondition.IsBuilding);
        if (isBuildingObj != null)
        {
            isBuildingObj.CurrentCondition.IsBuilding = false;
            builder.AddTarget(isBuildingObj.transform);
        }
        foreach (var building in buildings.Where(x => !x.CurrentCondition.IsActivate)
                     .OrderBy(x => x.CurrentCondition.CurrentBuildTime))
        {
            builder.AddTarget(building.transform);
        }
        UpdateBuildings();
    }

    private const float BuildingYOffset = 0.4f;
    private BuildingBase InstantiateBuilding(int buildingID , BuildingType buildingType , Vector3 position , BuildingCondition buildingCondition = default)
    {
        var cloneObj = _buildingDataSet.Buildings[(int)buildingType].BuildingBase;
        BuildingBase building = Instantiate(cloneObj , new Vector3(position.x ,BuildingYOffset , position.z) , cloneObj.transform.rotation);
        building.Initialize(buildingID ,buildingCondition);
        _buildingList[buildingType].Add(building);
        return building;
    }

    private BuildingBase LoadBuildings(BuildingSaveData saveData)
    {
        BuildingBase building = Instantiate(_buildingDataSet.Buildings[(int)saveData.Type].BuildingBase);
        building.LoadSaveData(saveData);
        _buildingList[saveData.Type].Add(building);
        return building;
    }
    private void OnSave()
    {
        _buildingsSaveData.SaveBuildings(_buildingList ,_nextBuildingID);
        SaveDataManagement.SaveJson(_buildingsSaveData);
    }
    
    /// <summary>
    /// 建築物に変更がある際に計測しなおす。
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
    [SerializeField] private  int _nextBuildingID;
    [SerializeField] private BuildingSaveData[] _buildingsArray;
    /// <summary>
    /// JsonUtilityでは派生クラスのままでは保存できないため、派生クラス用の配列を用意
    /// </summary>
    [SerializeField] private MineSaveData[] _minesArray;
    [SerializeField] private ArmyCampSaveData[] _armyCampsArray;

    
    public int NextBuildingID => _nextBuildingID;

    public BuildingSaveData[] BuildingsArray => _buildingsArray;

    public MineSaveData[] MinesArray => _minesArray;
    
    public ArmyCampSaveData[] ArmyCampsArray => _armyCampsArray;

    
    public void SaveBuildings(SortedDictionary<BuildingType , List<BuildingBase>> buildingList ,int nextBuildingID)
    {
        _nextBuildingID = nextBuildingID;
        List<BuildingSaveData>  buildingsDataList = new();
        foreach (var kv in buildingList)
        {
            foreach (var buildingBase in kv.Value)
            {
                buildingsDataList.Add(buildingBase.MakeSaveData());
            }
        }

        buildingsDataList = buildingsDataList.OrderBy(x => x.BuildingID).ToList();
        
        _minesArray = buildingsDataList.OfType<MineSaveData>().ToArray();
        _armyCampsArray = buildingsDataList.OfType<ArmyCampSaveData>().ToArray();
        //派生クラスのセーブデータを除去
        _buildingsArray = buildingsDataList.Where(x => x is not (MineSaveData or ArmyCampSaveData)).ToArray();
        
    }
}
