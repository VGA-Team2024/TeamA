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

    [SerializeField] private GameObject _testObj;
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           SetBuilding(_testObj);
        }
    }

    public void SetBuilding(GameObject building)
    {
        var obj = Instantiate(building);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var gridPos = new Vector3(Mathf.Floor(hit.point.x), 0, Mathf.Floor(hit.point.z));
            obj.transform.position = gridPos;
        }
    }
}
