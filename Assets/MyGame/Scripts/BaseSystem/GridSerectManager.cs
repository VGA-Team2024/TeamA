using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField, Header("カーソル用のオブジェクト")] private GameObject _cursorObj;
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private SelectType _selectType = SelectType.SetBuildingMode;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private Builder _builder;
    [SerializeField] private GridManager _gridManager;
    private Vector3 _currentCursorPos;
    private float _buildingYOffect = 0.4f;
    
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
        ShowCursor();
        MoveCursor();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // ここでreturnすることで、UI要素上でのクリックの場合は以降の処理を行わない
                return;
            }
            
            if (_selectType == SelectType.SetBuildingMode)
            {
                SetBuilding(_buildingType);
                
            }
            else if (_selectType == SelectType.SelectBuildingMode)
            {   //施設を選択した場合の処理を呼び出す
                SelectBuilding();
            }
        }
    }
    
    private void ShowCursor()
    {
        if (_selectType == SelectType.SelectBuildingMode)
        {
            _cursorObj.SetActive(false);
            return;
        }
        _cursorObj.SetActive(true);
    }
    
    /// <summary>
    /// カーソルを動かす
    /// </summary>
    private void MoveCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        // カメラから地面までの距離を考慮
        float distanceToGround = Mathf.Abs(Camera.main.transform.position.y - 0); // 地面がY=0の場合
        mousePos.z = distanceToGround;
        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (!Physics.Raycast(ray, out var hit)) return;
        var gridPos = new Vector3(Mathf.Floor(hit.point.x), 0, Mathf.Floor(hit.point.z));
        _currentCursorPos = gridPos;
        _cursorObj.transform.position = gridPos;
    }
    
    /// <summary>
    /// 建設する建物を変更する
    /// </summary>
    /// <param name="buildingType"></param>
    public void ChangeSetBuildingType(BuildingType buildingType)
    {
        _buildingType = buildingType;
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
        if (!_buildingManager.IsBuildable(_buildingType)) return;
           
        if (_gridManager.GridList.Contains(_currentCursorPos))
        {
            Debug.LogWarning("すでに建物があります。");
            return;
        }
        var obj = _buildingManager.CreateBuilding(_buildingType , new Vector3(_currentCursorPos.x, _buildingYOffect, _currentCursorPos.z)); 
        _gridManager.AddObjectPos(_currentCursorPos);
        _builder.AddTarget(obj.transform);
    }
    
}
