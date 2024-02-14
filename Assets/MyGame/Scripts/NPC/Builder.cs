using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

/// <summary>
/// 工員のスクリプト
/// </summary>
public class Builder : MonoBehaviour
{
    [SerializeField] Transform _target; 
    [SerializeField] NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_target.position);
    }
}
