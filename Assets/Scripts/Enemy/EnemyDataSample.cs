using Alchemy.Inspector;
using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDataSample : ScriptableObject
{
    [LabelText("��������G�l�~�[�̃v���n�u")]
    [SerializeField] private GameObject _enemyPrefab;

    [LabelText("�����ł���ő吔")]
    [SerializeField] private int _maxEnemyCnt;

    [LabelText("�G�l�~�[�̋���")]
    [SerializeField, SerializeReference] private IMovePatternEnemy _movePatern;
}

public interface IMovePatternEnemy { }

[Serializable]
/// <summary>���񂷂���</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3[] _patrolPositions;
    /// <summary>
    /// �Q�b�^�[
    /// </summary>
    public Vector3[] GetPatrolPositions => _patrolPositions;
    [ReadOnly] private int _lastPositionIndex;
}


[Serializable]
/// <summary>�ꂩ���������</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3 _guardPosition;
}


[Serializable]
/// <summary>���R�ɓ��������</summary>
public sealed class FreeMoveEnemy : IMovePatternEnemy
{
}