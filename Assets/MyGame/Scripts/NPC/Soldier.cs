using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// フィールドを歩き回るソルジャークラス
/// </summary>
public class Soldier : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    private GridManager _gridManager;
    private float _waitTime = 0f; // 次の移動までの待機時間
    private bool _isWaiting = false; //
    
    // Start is called before the first frame update

    private void Start()
    {
        _gridManager = FindObjectOfType<GridManager>();
        SetNewDestination();
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
    
    void Update()
    {
        if (_isWaiting)
        {
            // 待機時間を減らす
            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0)
            {
                _isWaiting = false; // 待機状態を終了
                SetNewDestination(); // 新しい目的地を設定
            }
        }
        else if (!_agent.pathPending && _agent.remainingDistance <= 0.1f)
        {
            // 目的地に到着したら待機状態にする
            _isWaiting = true;
            _waitTime = UnityEngine.Random.Range(1f, 5f); // 1秒から5秒の間でランダムに待機
        }
    }
    
    private void SetNewDestination()
    {
        Vector3 destination = _gridManager.GetRandomEmptyGridPos();
        _agent.SetDestination(destination);
    }
}
