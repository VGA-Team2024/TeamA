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
    [SerializeField] private BuilderState _state = BuilderState.Idle;
    [SerializeField] private float _targetDistance = 0.1f;
    private Queue<Transform> _targets = new Queue<Transform>();
    
    public enum BuilderState
    {
        Idle,
        Moving,
        Building,
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        IsArrived();
    }

    private void SetTarget()
    {
        if (_targets.Count > 0 && _state == BuilderState.Idle)
        {
            Debug.Log("ターゲットがある");
            _target = _targets.Dequeue();
            _state = BuilderState.Moving;
            if (_target) _agent.SetDestination(_target.position);
        }
    }

    /// <summary>
    /// 建設予定地に到着したかどうかを判定し、到着したら建設を開始する
    /// 建築中は他の建築予定地に移動しない
    /// </summary>
    private void IsArrived()
    {
        if (_target == null) return;
        if (_agent.remainingDistance > _targetDistance) return;
        if (_target.TryGetComponent<BuildingBase>(out var buildingBase))
        {
            buildingBase.StartBuilding();
            _state = BuilderState.Building;
            buildingBase.OnBuildingComplete += () => _state = BuilderState.Idle;
            _target = null;
        }
    }

    public void AddTarget(Transform target)
    {
        _targets.Enqueue(target);
    }
}
