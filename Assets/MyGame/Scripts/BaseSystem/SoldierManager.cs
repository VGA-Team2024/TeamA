using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 兵士の管理クラス
/// 生成、削除、移動先指定などを行う
/// </summary>
public class SoldierManager : MonoBehaviour
{
    [SerializeField, Header("フィールドに表示する兵士の最大数")] int _maxFieldSoldierCount;
    [SerializeField] GameObject _soldierPrefab;
    [SerializeField] private float _instantiateSpan = 0.1f;
    private List<GameObject> _soldierList = new List<GameObject>();
    

    /// <summary>
    /// 兵士を生成する。
    /// フィールドに表示する兵士の最大数を超えた場合は生成されない。
    /// </summary>
    public void AddSoldier( Vector3 pos)
    {
        var soldier = Instantiate(_soldierPrefab, pos, Quaternion.identity);
        _soldierList.Add(soldier);
        if (_soldierList.Count >= _maxFieldSoldierCount)
        {
            Debug.Log("フィールドに表示する兵士の最大数を超えています");
            return;
        }
    }

    /// <summary>
    /// 兵士を削除する。引き数で削除数を指定できる。
    /// 一瞬で消える
    /// </summary>
    /// <param name="removeNum"></param>
    public void RemoveSoldier(int removeNum = 1)
    {
        for (int i = 0; i < removeNum; i++)
        {
            if (_soldierList.Count <= 0)
            {
                Debug.Log("兵士がいません");
                return;
            }
            Destroy(_soldierList[0]);
            _soldierList.RemoveAt(0);
        }
    }
}
