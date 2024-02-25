using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// フィールドを歩き回るソルジャークラス
/// </summary>
public class Soldier : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] NavMeshAgent _agent;
    // Start is called before the first frame update
    
    public void SetTarget(Transform target)
    {
        _target = target;
        _agent.SetDestination(_target.position);
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
