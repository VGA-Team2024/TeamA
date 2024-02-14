using System;
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
    private BuilderState _state = BuilderState.Idle;
    
    public enum BuilderState
    {
        Idle,
        Moving,
        Building,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_state == BuilderState.Building || _state == BuilderState.Idle) return;
        _agent.SetDestination(_target.position);
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<BuildingBase>().StartBuilding();
        _state = BuilderState.Building;
        other.gameObject.GetComponent<BuildingBase>().OnBuildingComplete += () => _state = BuilderState.Idle;
    }
}
