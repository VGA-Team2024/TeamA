using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建物をグリッド上に配置するクラス
/// </summary>
public class GridSetManager : MonoBehaviour
{
    [SerializeField,Header("グリッドのサイズ")] Vector2Int _gridSize;
    private List<Vector3> _gridList = new List<Vector3>();
    [SerializeField, Header("カーソル用のオブジェクト")] private GameObject _cursorObj;
    [SerializeField] private GameObject _testObj;
    // Start is called before the first frame update

    private void Start()
    {
        _cursorObj = Instantiate(_cursorObj);
    }

    private void Update()
    {
        SetCursor();
        if (Input.GetMouseButtonDown(0))
        {
           SetBuilding(BuildingType.Test);
        }
    }
    
    /// <summary>
    /// カーソルを配置する
    /// </summary>
    private void SetCursor()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var gridPos = new Vector3(Mathf.Floor(hit.point.x), 0, Mathf.Floor(hit.point.z));
            _cursorObj.transform.position = gridPos;
        }
    }
 
    /// <summary>
    /// 引き数のオブジェクトを配置する
    /// </summary>
    /// <param name="building"></param>
    public void SetBuilding(BuildingType buildingType)
    {
        var obj = Instantiate(GetBuildingPrefab(buildingType));
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var gridPos = new Vector3(Mathf.Floor(hit.point.x), 0, Mathf.Floor(hit.point.z));
            if (_gridList.Contains(gridPos))
            {
                Debug.LogWarning("すでに建物があります。");
            }
            obj.transform.position = gridPos;
            _gridList.Add(gridPos);
        }
    }

    /// <summary>
    /// 指定した
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
                throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, null);
        }
    }
}
