using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グリッドを管理するクラス
///
/// 
/// 建物をグリッド上に配置するクラス
/// UIから設定状態をオフにできるように
/// オフの時に選択したら選択した建物のイベントを発火
/// </summary>
public class GridSerectManager : MonoBehaviour
{
    [SerializeField,Header("グリッドのサイズ")] Vector2Int _gridSize;
    [SerializeField, Header("カーソル用のオブジェクト")] private GameObject _cursorObj;
    [SerializeField] private GameObject _testObj;
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private SelectType _selectType = SelectType.SetBuildingMode;
    private List<Vector3> _gridList = new List<Vector3>();
    private Vector3 _currentCursorPos;
    
    public enum  SelectType
    {
        SelectBuildingMode,
        SetBuildingMode,
    }

    private void Start()
    {
        _cursorObj = Instantiate(_cursorObj);
    }
    
    /// <summary>
    /// 選択モードを変更する
    /// </summary>
    /// <param name="selectType"></param>
    public void ChangeSelectType(SelectType selectType)
    {
        _selectType = selectType;
    }

    private void Update()
    {
        MoveCursor();

        if (Input.GetMouseButtonDown(0))
        {
            if (_selectType == SelectType.SetBuildingMode)
            {
                SetBuilding(_buildingType);
            }
            else if (_selectType == SelectType.SelectBuildingMode)
            {
                SelectBuilding();
            }
        }
    }
    
    /// <summary>
    /// カーソルを動かす
    /// </summary>
    private void MoveCursor()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var gridPos = new Vector3(Mathf.Floor(hit.point.x), 0, Mathf.Floor(hit.point.z));
            _currentCursorPos = gridPos;
            _cursorObj.transform.position = gridPos;
        }
    }
    
    private void SelectBuilding()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            hit.collider.gameObject.GetComponent<BuildingBase>()?.OnClick();
        }
    }
 
    /// <summary>
    /// 引き数のオブジェクトを配置する
    /// </summary>
    /// <param name="building"></param>
    public void SetBuilding(BuildingType buildingType)
    {
        var obj = Instantiate(BuildingManager.Instance.InstantiateBuilding(buildingType , transform ));
        if (_gridList.Contains(_currentCursorPos))
        {
            Debug.LogWarning("すでに建物があります。");
            return;
        }

        obj.transform.position = _currentCursorPos;
        _gridList.Add(_currentCursorPos);
        
        FindObjectOfType<Builder>().AddTarget(obj.transform);
    }

    /// <summary>
    /// 指定した種類のプレハブを取得する
    /// </summary>
    /// <param name="buildingType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private GameObject GetBuildingPrefab(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Test:
                return _testObj;
            default:
                return _testObj;
        }
    }
}
