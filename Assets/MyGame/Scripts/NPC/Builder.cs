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
    private Queue<Transform> _targets = new Queue<Transform>();
    
    public enum BuilderState
    {
        Idle,
        Moving,
        Building,
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_targets.Count > 0 && _state == BuilderState.Idle)
        {
            _target = _targets.Dequeue();
            _state = BuilderState.Moving;
        }
        _agent.SetDestination(_target.position);
    }
    
    public void AddTarget(Transform target)
    {
        _targets.Enqueue(target);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<BuildingBase>().StartBuilding();
        _state = BuilderState.Building;
        other.gameObject.GetComponent<BuildingBase>().OnBuildingComplete += () => _state = BuilderState.Idle;
    }
}
