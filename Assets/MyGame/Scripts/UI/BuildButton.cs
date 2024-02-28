using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 押すとGridSelectManagerの建設するものを切り替えるボタン
/// </summary>
public class BuildButton : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] private TMP_Text _buildingCostText;
    [SerializeField] private TMP_Text _buildingNameText;
    [SerializeField] private TMP_Text _buildingStock;
    [SerializeField] BuildingType _buildingType;
    private GridSerectManager _gridSelectManager;
    private ResourceManager _resourceManager;
    private BuildingManager _buildingManager;

    private string _buildingName;
    private float _currentResource;
    private static float _buildingPrice;
    private float _currentBuildingStock;

    private float _maxBuildingStock;
    // Start is called before the first frame update
    void Start()
    {
        _buildingManager = BuildingManager.Instance;
        _resourceManager = ResourceManager.Instance;
        
        _buildingPrice = _buildingManager.BuildingPrices[_buildingType];
        _buildingName = _buildingManager.BuildingNames[_buildingType];
        _maxBuildingStock = _buildingManager.MaxBuildingStocks[_buildingType];
        _buildingCostText.text = $"{_buildingPrice}";
        _buildingNameText.text = $"{_buildingName}";
        
        _resourceManager.ObserveEveryValueChanged(x => x.CurrentResources).Subscribe(_ => CheckAvailable()).AddTo(this);
        _buildingManager.ObserveEveryValueChanged(x => x.BuildingList[_buildingType].Count)
            .Subscribe(x =>
            {
                _buildingStock.text = $"  {x} / {_maxBuildingStock}";
            }).AddTo(this);
        
        _gridSelectManager = FindObjectOfType<GridSerectManager>();
        _button.onClick.AddListener(() =>
        {
            _gridSelectManager.ChangeSetBuildingType(_buildingType);
        });
    }

    void CheckAvailable()
    {
        if (_resourceManager.IsUseResources(_buildingPrice))
        {
            _button.interactable = true;
        }
        else
        {
            if (!_button.interactable)
            {
                _button.interactable = false;
            }
        }
    }
}
