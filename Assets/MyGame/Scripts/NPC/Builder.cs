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
    [SerializeField] private Animator _animator;
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
        MoveToTarget();
        BuildTarget();
    }

    /// <summary>
    /// 移動先候補がある場合に移動するように
    /// </summary>
    private void MoveToTarget()
    {
        if (_targets.Count > 0 && _state == BuilderState.Idle)
        {
            Debug.Log("ターゲットがある");
            _target = _targets.Dequeue();
            _state = BuilderState.Moving;
            _agent.SetDestination(_target.position);
            _animator.SetFloat("Speed_f", 1);
        }
    }

    /// <summary>
    /// 建設予定地に到着したかどうかを判定し、到着したら建設を開始する
    /// 建築中は他の建築予定地に移動しない
    /// </summary>
    private void BuildTarget()
    {
        if (_target == null) return;
        if (_agent.remainingDistance > _targetDistance) return;
        if (_target.TryGetComponent<BuildingBase>(out var buildingBase))
        {
            _animator.SetFloat("Speed_f", 0);
            _animator.SetBool("Melee", true);
            buildingBase.StartBuilding();
            _state = BuilderState.Building;
            buildingBase.OnBuildingComplete += () =>
            {
                _animator.SetBool("Melee", false);
                _state = BuilderState.Idle;
            };
            _target = null;
        }
    }

    /// <summary>
    /// 建築候補を追加する処理
    /// </summary>
    /// <param name="target"></param>
    public void AddTarget(Transform target)
    {
        _targets.Enqueue(target);
    }
}
