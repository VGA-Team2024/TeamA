using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 押すとGridSelectManagerの建設するものを切り替えるボタン
/// </summary>
public class BuildButton : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] BuildingType _buildingType;
    private GridSerectManager _gridSelectManager;
    // Start is called before the first frame update
    void Start()
    {
        _gridSelectManager = FindObjectOfType<GridSerectManager>();
        _button.onClick.AddListener(() =>
        {
            _gridSelectManager.ChangeSetBuildingType(_buildingType);
        });
    }
}
