using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 兵士の管理クラス
/// 生成、削除、移動先指定などを行う
/// </summary>
public class SoliderManager : MonoBehaviour
{
    [SerializeField, Header("フィールドに表示する兵士の最大数")] int _maxFieldSoldierCount;
    [SerializeField] GameObject _soldierPrefab;
    [SerializeField] private float _instantiateSpan = 0.1f;
    private List<GameObject> _soldierList = new List<GameObject>();
    

    /// <summary>
    /// 兵士を生成する。引き数で生成数を指定できる。
    /// 生成は非同期で行い、指定した秒数ごとに生成される。
    /// フィールドに表示する兵士の最大数を超えた場合は生成されない。
    /// </summary>
    /// <param name="instantiateNum">生成数(デフォルトは1)</param>
    public async UniTask AddSoldier(int instantiateNum = 1)
    {
        for (int i = 0; i < instantiateNum; i++)
        {
            var soldier = Instantiate(_soldierPrefab);
            _soldierList.Add(soldier);
            if (_soldierList.Count >= _maxFieldSoldierCount)
            {
                Debug.Log("フィールドに表示する兵士の最大数を超えています");
                return;
            }
            await UniTask.Delay(System.TimeSpan.FromSeconds(_instantiateSpan));
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
