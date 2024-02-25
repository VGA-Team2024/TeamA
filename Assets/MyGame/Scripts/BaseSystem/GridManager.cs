using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドのグリッドを管理するクラス
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField,Header("グリッド1マスのサイズ")] Vector2Int _gridSize;
    private List<Vector3> _gridList = new List<Vector3>();
    public List<Vector3> GridList => _gridList;
    
    /// <summary>
    /// 管理しているグリッドにオブジェクトを追加する
    /// </summary>
    /// <param name="pos"></param>
    public void AddObjectPos(Vector3 pos)
    {
        _gridList.Add(pos);
    }
    
    /// <summary>
    /// 現在のグリッドの位置を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomEmptyGridPos()
    {
        return Vector3.zero;
    }
}
