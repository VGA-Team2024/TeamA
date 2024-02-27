using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドのグリッドを管理するクラス
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField,Header("グリッド1マスのサイズ")] Vector2Int _gridSize;
    [SerializeField, Header("フィールドのサイズ")] Vector2Int _fieldSize;
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
    /// フィールドから建物がないグリッドを取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomEmptyGridPos()
    {
        Vector3 randomDestination = Vector3.zero;
        bool found = false;
        while (!found)
        {
            var randomPos = new Vector3(Random.Range(-_fieldSize.x / 2, _fieldSize.x / 2), 0, Random.Range(-_fieldSize.y / 2, _fieldSize.y / 2));
            // その位置が建物のないグリッドにあるかをチェック
            if (_gridList.Contains(randomPos) == false)
            {
                randomDestination = randomPos;
                found = true;
            }
        }
        return randomDestination;
    }
}
